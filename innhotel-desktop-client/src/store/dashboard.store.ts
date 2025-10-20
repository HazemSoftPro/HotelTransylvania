import { create } from 'zustand';
import type { DashboardMetrics } from '@/services/dashboardService';

interface DashboardState {
  metrics: DashboardMetrics | null;
  isLoading: boolean;
  error: string | null;
  lastUpdated: Date | null;
  autoRefreshEnabled: boolean;
  refreshInterval: number; // in seconds
  
  // Actions
  setMetrics: (metrics: DashboardMetrics) => void;
  setLoading: (isLoading: boolean) => void;
  setError: (error: string | null) => void;
  setAutoRefresh: (enabled: boolean) => void;
  setRefreshInterval: (interval: number) => void;
  clearMetrics: () => void;
}

/**
 * Dashboard Store
 * 
 * Global state management for dashboard metrics and settings.
 */
export const useDashboardStore = create<DashboardState>((set) => ({
  metrics: null,
  isLoading: false,
  error: null,
  lastUpdated: null,
  autoRefreshEnabled: true,
  refreshInterval: 30, // 30 seconds default

  setMetrics: (metrics: DashboardMetrics) => {
    set({
      metrics,
      lastUpdated: new Date(),
      error: null,
    });
  },

  setLoading: (isLoading: boolean) => {
    set({ isLoading });
  },

  setError: (error: string | null) => {
    set({ error, isLoading: false });
  },

  setAutoRefresh: (enabled: boolean) => {
    set({ autoRefreshEnabled: enabled });
  },

  setRefreshInterval: (interval: number) => {
    set({ refreshInterval: interval });
  },

  clearMetrics: () => {
    set({
      metrics: null,
      error: null,
      lastUpdated: null,
    });
  },
}));