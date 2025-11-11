import { HubConnectionBuilder, HubConnection, LogLevel } from '@microsoft/signalr';
import { API_BASE_URL } from '../config/constants';

export interface DashboardMetrics {
  totalRooms: number;
  occupiedRooms: number;
  availableRooms: number;
  roomsUnderMaintenance: number;
  todayCheckIns: number;
  todayCheckOuts: number;
  pendingReservations: number;
  todayRevenue: number;
  occupancyRate: number;
  averageRoomRate: number;
  staffOnDuty: number;
  tasksPending: number;
  lastUpdated: string;
}

export interface ActivityItem {
  type: string;
  description: string;
  timestamp: string;
  userId?: string;
}

export interface AlertItem {
  type: string;
  message: string;
  priority: string;
  timestamp: string;
  isResolved: boolean;
}

export interface NotificationItem {
  id: string;
  title: string;
  message: string;
  category: string;
  timestamp: string;
  isHighPriority: boolean;
  metadata?: Record<string, any>;
}

class SignalRService {
  private connection: HubConnection | null = null;
  private reconnectAttempts = 0;
  private maxReconnectAttempts = 5;
  private reconnectDelay = 3000;

  async connect(token: string): Promise<void> {
    try {
      this.connection = new HubConnectionBuilder()
        .withUrl(`${API_BASE_URL}/hotelHub`, {
          accessTokenFactory: () => token,
        })
        .withAutomaticReconnect({
          nextRetryDelayInMilliseconds: (retryContext) => {
            if (retryContext.previousRetryCount >= this.maxReconnectAttempts) {
              return null;
            }
            return this.reconnectDelay;
          }
        })
        .configureLogging(LogLevel.Information)
        .build();

      // Set up event handlers
      this.setupEventHandlers();

      // Start the connection
      await this.connection.start();
      console.log('SignalR connected successfully');

      // Join department groups based on user role
      await this.joinDepartmentGroups();
    } catch (error) {
      console.error('Error connecting to SignalR:', error);
      throw error;
    }
  }

  async disconnect(): Promise<void> {
    if (this.connection) {
      await this.connection.stop();
      this.connection = null;
    }
  }

  private setupEventHandlers(): void {
    if (!this.connection) return;

    // Connection events
    this.connection.onreconnecting(error => {
      console.log('SignalR reconnecting...', error);
      this.reconnectAttempts++;
    });

    this.connection.onreconnected(connectionId => {
      console.log('SignalR reconnected with connection ID:', connectionId);
      this.reconnectAttempts = 0;
    });

    this.connection.onclose(error => {
      console.log('SignalR connection closed:', error);
    });

    // Hotel events
    this.connection.on('ReceiveNotification', (notification: NotificationItem) => {
      this.handleNotification(notification);
    });

    this.connection.on('ReceiveDepartmentNotification', (notification: NotificationItem) => {
      this.handleDepartmentNotification(notification);
    });

    this.connection.on('ReceiveHighPriorityNotification', (notification: NotificationItem) => {
      this.handleHighPriorityNotification(notification);
    });

    this.connection.on('RoomStatusUpdated', (roomUpdate: any) => {
      this.handleRoomStatusUpdate(roomUpdate);
    });

    this.connection.on('ReservationUpdated', (reservationUpdate: any) => {
      this.handleReservationUpdate(reservationUpdate);
    });

    this.connection.on('GuestUpdated', (guestUpdate: any) => {
      this.handleGuestUpdate(guestUpdate);
    });

    this.connection.on('PaymentUpdated', (paymentUpdate: any) => {
      this.handlePaymentUpdate(paymentUpdate);
    });

    this.connection.on('HousekeepingTaskAssigned', (task: any) => {
      this.handleHousekeepingTask(task);
    });

    this.connection.on('SystemAlert', (alert: any) => {
      this.handleSystemAlert(alert);
    });

    this.connection.on('DashboardUpdated', (update: any) => {
      this.handleDashboardUpdate(update);
    });

    this.connection.on('MetricsUpdated', (metrics: DashboardMetrics) => {
      this.handleMetricsUpdated(metrics);
    });
  }

  private async joinDepartmentGroups(): Promise<void> {
    if (!this.connection) return;

    // Get user role from token or API
    const userRole = await this.getUserRole();
    
    // Join relevant department groups
    switch (userRole) {
      case 'FrontDesk':
        await this.connection.invoke('JoinDepartmentGroup', 'FrontDesk');
        break;
      case 'Housekeeping':
        await this.connection.invoke('JoinDepartmentGroup', 'Housekeeping');
        break;
      case 'Management':
        await this.connection.invoke('JoinDepartmentGroup', 'Management');
        break;
      case 'Accounting':
        await this.connection.invoke('JoinDepartmentGroup', 'Accounting');
        break;
    }
  }

  private async getUserRole(): Promise<string> {
    // This would typically come from the auth token or a user profile API call
    return 'FrontDesk'; // Placeholder
  }

  // Event handler methods
  private handleNotification(notification: NotificationItem): void {
    // Dispatch custom event for components to listen to
    window.dispatchEvent(new CustomEvent('hotelNotification', {
      detail: notification
    }));
  }

  private handleDepartmentNotification(notification: NotificationItem): void {
    window.dispatchEvent(new CustomEvent('departmentNotification', {
      detail: notification
    }));
  }

  private handleHighPriorityNotification(notification: NotificationItem): void {
    window.dispatchEvent(new CustomEvent('highPriorityNotification', {
      detail: notification
    }));
  }

  private handleRoomStatusUpdate(roomUpdate: any): void {
    window.dispatchEvent(new CustomEvent('roomStatusUpdate', {
      detail: roomUpdate
    }));
  }

  private handleReservationUpdate(reservationUpdate: any): void {
    window.dispatchEvent(new CustomEvent('reservationUpdate', {
      detail: reservationUpdate
    }));
  }

  private handleGuestUpdate(guestUpdate: any): void {
    window.dispatchEvent(new CustomEvent('guestUpdate', {
      detail: guestUpdate
    }));
  }

  private handlePaymentUpdate(paymentUpdate: any): void {
    window.dispatchEvent(new CustomEvent('paymentUpdate', {
      detail: paymentUpdate
    }));
  }

  private handleHousekeepingTask(task: any): void {
    window.dispatchEvent(new CustomEvent('housekeepingTask', {
      detail: task
    }));
  }

  private handleSystemAlert(alert: any): void {
    window.dispatchEvent(new CustomEvent('systemAlert', {
      detail: alert
    }));
  }

  private handleDashboardUpdate(update: any): void {
    window.dispatchEvent(new CustomEvent('dashboardUpdate', {
      detail: update
    }));
  }

  private handleMetricsUpdated(metrics: DashboardMetrics): void {
    window.dispatchEvent(new CustomEvent('metricsUpdated', {
      detail: metrics
    }));
  }

  // Public methods for manual updates
  async joinBranchGroup(branchId: number): Promise<void> {
    if (this.connection) {
      await this.connection.invoke('JoinBranchGroup', branchId);
    }
  }

  async leaveBranchGroup(branchId: number): Promise<void> {
    if (this.connection) {
      await this.connection.invoke('LeaveBranchGroup', branchId);
    }
  }

  async getOnlineUsersCount(): Promise<number> {
    if (this.connection) {
      return await this.connection.invoke('GetOnlineUsersCount');
    }
    return 0;
  }

  getConnectionState(): string {
    return this.connection?.state || 'Disconnected';
  }

  isConnected(): boolean {
    return this.connection?.state === 'Connected';
  }
}

export const signalRService = new SignalRService();