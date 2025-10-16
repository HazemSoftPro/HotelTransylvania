import api from '@/lib/axios';

export interface DashboardMetrics {
  totalRooms: number;
  availableRooms: number;
  occupiedRooms: number;
  maintenanceRooms: number;
  occupancyRate: number;
  totalReservations: number;
  activeReservations: number;
  pendingReservations: number;
  confirmedReservations: number;
  checkedInReservations: number;
  totalRevenue: number;
  monthlyRevenue: number;
  totalGuests: number;
  newGuestsThisMonth: number;
  recentActivities: RecentActivity[];
}

export interface RecentActivity {
  type: string;
  description: string;
  timestamp: string;
  entityId?: string;
}

export interface DashboardMetricsParams {
  branchId?: number;
  startDate?: string;
  endDate?: string;
}

/**
 * Dashboard Service
 * 
 * Provides methods for fetching dashboard metrics and analytics.
 */
class DashboardService {
  /**
   * Get dashboard metrics
   */
  async getMetrics(params?: DashboardMetricsParams): Promise<DashboardMetrics> {
    const response = await api.get('/dashboard/metrics', { params });
    return response.data.data;
  }
}

export const dashboardService = new DashboardService();