import * as signalR from '@microsoft/signalr';
import { logger } from '@/utils/logger';

export interface RoomStatusUpdate {
  roomId: number;
  branchId: number;
  newStatus: number;
  timestamp: string;
  data: any;
}

export interface RoomUpdate {
  roomId: number;
  branchId: number;
  timestamp: string;
  data: any;
}

export interface BulkRoomsUpdate {
  branchId: number;
  timestamp: string;
  data: any;
}

export interface SystemNotification {
  branchId: number;
  message: string;
  type: string;
  timestamp: string;
}

export class SignalRService {
  private connection: signalR.HubConnection | null = null;
  private reconnectAttempts = 0;
  private maxReconnectAttempts = 5;
  private reconnectDelay = 5000; // 5 seconds
  private isConnecting = false;
  private currentBranchId: number | null = null;

  // Event handlers
  private onRoomStatusChangedHandlers: ((update: RoomStatusUpdate) => void)[] = [];
  private onRoomUpdatedHandlers: ((update: RoomUpdate) => void)[] = [];
  private onRoomCreatedHandlers: ((data: any) => void)[] = [];
  private onRoomDeletedHandlers: ((roomId: number) => void)[] = [];
  private onBulkRoomsUpdatedHandlers: ((update: BulkRoomsUpdate) => void)[] = [];
  private onMaintenanceScheduleChangedHandlers: ((data: any) => void)[] = [];
  private onSystemNotificationHandlers: ((notification: SystemNotification) => void)[] = [];
  private onConnectionStateChangedHandlers: ((state: signalR.HubConnectionState) => void)[] = [];

  constructor(private readonly apiBaseUrl: string, private readonly getAuthToken: () => string | null) {}

  async connect(): Promise<void> {
    if (this.isConnecting || this.connection?.state === signalR.HubConnectionState.Connected) {
      return;
    }

    this.isConnecting = true;

    try {
      const token = this.getAuthToken();
      if (!token) {
        throw new Error('No authentication token available');
      }

      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(`${this.apiBaseUrl}/api/hubs/roomstatus`, {
          accessTokenFactory: () => token,
          transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling
        })
        .withAutomaticReconnect({
          nextRetryDelayInMilliseconds: (retryContext) => {
            if (retryContext.previousRetryCount < this.maxReconnectAttempts) {
              return this.reconnectDelay * Math.pow(2, retryContext.previousRetryCount);
            }
            return null; // Stop reconnecting
          }
        })
        .configureLogging(signalR.LogLevel.Information)
        .build();

      this.setupEventHandlers();
      
      await this.connection.start();
      
      logger().info('SignalR connection established');
      this.reconnectAttempts = 0;
      this.notifyConnectionStateChanged(this.connection.state);

      // Rejoin branch group if we were previously connected to one
      if (this.currentBranchId) {
        await this.joinBranchGroup(this.currentBranchId);
      }

    } catch (error) {
      logger().error('Failed to connect to SignalR hub', { error });
      this.handleReconnection();
    } finally {
      this.isConnecting = false;
    }
  }

  private setupEventHandlers(): void {
    if (!this.connection) return;

    // Room status changed
    this.connection.on('RoomStatusChanged', (update: RoomStatusUpdate) => {
      logger().info('Room status changed received', { roomId: update.roomId, status: update.newStatus });
      this.onRoomStatusChangedHandlers.forEach(handler => handler(update));
    });

    // Room updated
    this.connection.on('RoomUpdated', (update: RoomUpdate) => {
      logger().info('Room updated received', { roomId: update.roomId });
      this.onRoomUpdatedHandlers.forEach(handler => handler(update));
    });

    // Room created
    this.connection.on('RoomCreated', (data: any) => {
      logger().info('Room created received', { branchId: data.branchId });
      this.onRoomCreatedHandlers.forEach(handler => handler(data));
    });

    // Room deleted
    this.connection.on('RoomDeleted', (roomId: number) => {
      logger().info('Room deleted received', { roomId });
      this.onRoomDeletedHandlers.forEach(handler => handler(roomId));
    });

    // Bulk rooms updated
    this.connection.on('BulkRoomsUpdated', (update: BulkRoomsUpdate) => {
      logger().info('Bulk rooms updated received', { branchId: update.branchId });
      this.onBulkRoomsUpdatedHandlers.forEach(handler => handler(update));
    });

    // Maintenance schedule changed
    this.connection.on('MaintenanceScheduleChanged', (data: any) => {
      logger().info('Maintenance schedule changed received', { branchId: data.branchId });
      this.onMaintenanceScheduleChangedHandlers.forEach(handler => handler(data));
    });

    // System notification
    this.connection.on('SystemNotification', (notification: SystemNotification) => {
      logger().info('System notification received', { message: notification.message });
      this.onSystemNotificationHandlers.forEach(handler => handler(notification));
    });

    // Connection state changes
    this.connection.onreconnecting(() => {
      logger().info('SignalR reconnecting...');
      this.notifyConnectionStateChanged(signalR.HubConnectionState.Reconnecting);
    });

    this.connection.onreconnected(() => {
      logger().info('SignalR reconnected');
      this.notifyConnectionStateChanged(signalR.HubConnectionState.Connected);
      // Rejoin branch group after reconnection
      if (this.currentBranchId) {
        this.joinBranchGroup(this.currentBranchId);
      }
    });

    this.connection.onclose(() => {
      logger().info('SignalR connection closed');
      this.notifyConnectionStateChanged(signalR.HubConnectionState.Disconnected);
      this.handleReconnection();
    });
  }

  private handleReconnection(): void {
    if (this.reconnectAttempts < this.maxReconnectAttempts) {
      this.reconnectAttempts++;
      const delay = this.reconnectDelay * Math.pow(2, this.reconnectAttempts - 1);
      
      logger().info(`Attempting to reconnect in ${delay}ms (attempt ${this.reconnectAttempts}/${this.maxReconnectAttempts})`);
      
      setTimeout(() => {
        this.connect();
      }, delay);
    } else {
      logger().error('Max reconnection attempts reached. Please refresh the application.');
    }
  }

  async disconnect(): Promise<void> {
    if (this.connection) {
      try {
        await this.connection.stop();
        logger().info('SignalR connection stopped');
      } catch (error) {
        logger().error('Error stopping SignalR connection', { error });
      }
      this.connection = null;
      this.currentBranchId = null;
    }
  }

  async joinBranchGroup(branchId: number): Promise<void> {
    if (this.connection?.state === signalR.HubConnectionState.Connected) {
      try {
        await this.connection.invoke('JoinBranchGroup', branchId.toString());
        this.currentBranchId = branchId;
        logger().info('Joined branch group', { branchId });
      } catch (error) {
        logger().error('Failed to join branch group', { branchId, error });
      }
    }
  }

  async leaveBranchGroup(branchId: number): Promise<void> {
    if (this.connection?.state === signalR.HubConnectionState.Connected) {
      try {
        await this.connection.invoke('LeaveBranchGroup', branchId.toString());
        if (this.currentBranchId === branchId) {
          this.currentBranchId = null;
        }
        logger().info('Left branch group', { branchId });
      } catch (error) {
        logger().error('Failed to leave branch group', { branchId, error });
      }
    }
  }

  async joinRoomGroup(roomId: number): Promise<void> {
    if (this.connection?.state === signalR.HubConnectionState.Connected) {
      try {
        await this.connection.invoke('JoinRoomGroup', roomId.toString());
        logger().info('Joined room group', { roomId });
      } catch (error) {
        logger().error('Failed to join room group', { roomId, error });
      }
    }
  }

  async leaveRoomGroup(roomId: number): Promise<void> {
    if (this.connection?.state === signalR.HubConnectionState.Connected) {
      try {
        await this.connection.invoke('LeaveRoomGroup', roomId.toString());
        logger().info('Left room group', { roomId });
      } catch (error) {
        logger().error('Failed to leave room group', { roomId, error });
      }
    }
  }

  // Event subscription methods
  onRoomStatusChanged(handler: (update: RoomStatusUpdate) => void): () => void {
    this.onRoomStatusChangedHandlers.push(handler);
    return () => {
      const index = this.onRoomStatusChangedHandlers.indexOf(handler);
      if (index > -1) {
        this.onRoomStatusChangedHandlers.splice(index, 1);
      }
    };
  }

  onRoomUpdated(handler: (update: RoomUpdate) => void): () => void {
    this.onRoomUpdatedHandlers.push(handler);
    return () => {
      const index = this.onRoomUpdatedHandlers.indexOf(handler);
      if (index > -1) {
        this.onRoomUpdatedHandlers.splice(index, 1);
      }
    };
  }

  onRoomCreated(handler: (data: any) => void): () => void {
    this.onRoomCreatedHandlers.push(handler);
    return () => {
      const index = this.onRoomCreatedHandlers.indexOf(handler);
      if (index > -1) {
        this.onRoomCreatedHandlers.splice(index, 1);
      }
    };
  }

  onRoomDeleted(handler: (roomId: number) => void): () => void {
    this.onRoomDeletedHandlers.push(handler);
    return () => {
      const index = this.onRoomDeletedHandlers.indexOf(handler);
      if (index > -1) {
        this.onRoomDeletedHandlers.splice(index, 1);
      }
    };
  }

  onBulkRoomsUpdated(handler: (update: BulkRoomsUpdate) => void): () => void {
    this.onBulkRoomsUpdatedHandlers.push(handler);
    return () => {
      const index = this.onBulkRoomsUpdatedHandlers.indexOf(handler);
      if (index > -1) {
        this.onBulkRoomsUpdatedHandlers.splice(index, 1);
      }
    };
  }

  onMaintenanceScheduleChanged(handler: (data: any) => void): () => void {
    this.onMaintenanceScheduleChangedHandlers.push(handler);
    return () => {
      const index = this.onMaintenanceScheduleChangedHandlers.indexOf(handler);
      if (index > -1) {
        this.onMaintenanceScheduleChangedHandlers.splice(index, 1);
      }
    };
  }

  onSystemNotification(handler: (notification: SystemNotification) => void): () => void {
    this.onSystemNotificationHandlers.push(handler);
    return () => {
      const index = this.onSystemNotificationHandlers.indexOf(handler);
      if (index > -1) {
        this.onSystemNotificationHandlers.splice(index, 1);
      }
    };
  }

  onConnectionStateChanged(handler: (state: signalR.HubConnectionState) => void): () => void {
    this.onConnectionStateChangedHandlers.push(handler);
    return () => {
      const index = this.onConnectionStateChangedHandlers.indexOf(handler);
      if (index > -1) {
        this.onConnectionStateChangedHandlers.splice(index, 1);
      }
    };
  }

  private notifyConnectionStateChanged(state: signalR.HubConnectionState): void {
    this.onConnectionStateChangedHandlers.forEach(handler => handler(state));
  }

  get connectionState(): signalR.HubConnectionState {
    return this.connection?.state ?? signalR.HubConnectionState.Disconnected;
  }

  get isConnected(): boolean {
    return this.connection?.state === signalR.HubConnectionState.Connected;
  }
}

// Create singleton instance
let signalRServiceInstance: SignalRService | null = null;

export const getSignalRService = (apiBaseUrl?: string, getAuthToken?: () => string | null): SignalRService => {
  if (!signalRServiceInstance && apiBaseUrl && getAuthToken) {
    signalRServiceInstance = new SignalRService(apiBaseUrl, getAuthToken);
  }
  
  if (!signalRServiceInstance) {
    throw new Error('SignalR service not initialized. Call with apiBaseUrl and getAuthToken first.');
  }
  
  return signalRServiceInstance;
};
