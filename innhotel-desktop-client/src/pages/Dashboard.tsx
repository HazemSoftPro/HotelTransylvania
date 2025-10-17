import { useEffect, useState } from 'react';
import { 
  Home, 
  DoorOpen, 
  Calendar, 
  Users, 
  DollarSign, 
  TrendingUp,
  RefreshCw,
} from 'lucide-react';
import { MetricCard, RecentActivityList, OccupancyChart } from '@/components/dashboard';
import { Button } from '@/components/ui/button';
import { useDashboardStore } from '@/store/dashboard.store';
import { dashboardService } from '@/services/dashboardService';
import { toast } from 'sonner';
import { ErrorState } from '@/components/search';
import { CardSkeleton } from '@/components/skeletons/CardSkeleton';

const Dashboard = () => {
  const {
    metrics,
    isLoading,
    error,
    lastUpdated,
    autoRefreshEnabled,
    refreshInterval,
    setMetrics,
    setLoading,
    setError,
  } = useDashboardStore();

  const [isRefreshing, setIsRefreshing] = useState(false);

  // Fetch dashboard metrics
  const fetchMetrics = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await dashboardService.getMetrics();
      setMetrics(data);
    } catch (err) {
      const error = err as Error;
      setError(error.message);
      toast.error('Failed to load dashboard', {
        description: error.message,
      });
    } finally {
      setLoading(false);
    }
  };

  // Manual refresh
  const handleRefresh = async () => {
    setIsRefreshing(true);
    await fetchMetrics();
    setIsRefreshing(false);
    toast.success('Dashboard refreshed');
  };

  // Initial load
  useEffect(() => {
    fetchMetrics();
  }, []);

  // Auto-refresh
  useEffect(() => {
    if (!autoRefreshEnabled) return;

    const interval = setInterval(() => {
      fetchMetrics();
    }, refreshInterval * 1000);

    return () => clearInterval(interval);
  }, [autoRefreshEnabled, refreshInterval]);

  // Format currency
  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
    }).format(amount);
  };

  // Error state
  if (error && !metrics) {
    return (
      <div className="container mx-auto py-6">
        <ErrorState
          message={error}
          onRetry={fetchMetrics}
        />
      </div>
    );
  }

  // Loading state
  if (isLoading && !metrics) {
    return (
      <div className="container mx-auto py-6 space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-2xl font-bold">Dashboard</h1>
            <p className="text-muted-foreground">Loading metrics...</p>
          </div>
        </div>
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
          {[...Array(4)].map((_, i) => (
            <CardSkeleton key={i} />
          ))}
        </div>
      </div>
    );
  }

  if (!metrics) return null;

  return (
    <div className="container mx-auto py-6 space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold">Dashboard</h1>
          <p className="text-muted-foreground">
            Welcome back! Here's what's happening today.
          </p>
        </div>
        <div className="flex items-center gap-2">
          {lastUpdated && (
            <span className="text-sm text-muted-foreground">
              Last updated: {new Date(lastUpdated).toLocaleTimeString()}
            </span>
          )}
          <Button
            variant="outline"
            size="sm"
            onClick={handleRefresh}
            disabled={isRefreshing}
          >
            <RefreshCw className={`h-4 w-4 mr-2 ${isRefreshing ? 'animate-spin' : ''}`} />
            Refresh
          </Button>
        </div>
      </div>

      {/* Metric Cards */}
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        <MetricCard
          title="Total Rooms"
          value={metrics.totalRooms}
          icon={Home}
          description={`${metrics.availableRooms} available`}
        />
        <MetricCard
          title="Occupancy Rate"
          value={`${metrics.occupancyRate.toFixed(1)}%`}
          icon={DoorOpen}
          description={`${metrics.occupiedRooms} occupied`}
        />
        <MetricCard
          title="Active Reservations"
          value={metrics.activeReservations}
          icon={Calendar}
          description={`${metrics.pendingReservations} pending`}
        />
        <MetricCard
          title="Total Guests"
          value={metrics.totalGuests}
          icon={Users}
          description={`${metrics.newGuestsThisMonth} new this month`}
        />
      </div>

      {/* Revenue Cards */}
      <div className="grid gap-4 md:grid-cols-2">
        <MetricCard
          title="Total Revenue"
          value={formatCurrency(metrics.totalRevenue)}
          icon={DollarSign}
          description="All-time revenue"
        />
        <MetricCard
          title="Monthly Revenue"
          value={formatCurrency(metrics.monthlyRevenue)}
          icon={TrendingUp}
          description="Last 30 days"
        />
      </div>

      {/* Charts and Activity */}
      <div className="grid gap-4 md:grid-cols-2">
        <OccupancyChart
          totalRooms={metrics.totalRooms}
          availableRooms={metrics.availableRooms}
          occupiedRooms={metrics.occupiedRooms}
          maintenanceRooms={metrics.maintenanceRooms}
          occupancyRate={metrics.occupancyRate}
        />
        <RecentActivityList activities={metrics.recentActivities} />
      </div>

      {/* Reservation Statistics */}
      <div className="grid gap-4 md:grid-cols-3">
        <MetricCard
          title="Confirmed Reservations"
          value={metrics.confirmedReservations}
          icon={Calendar}
          description="Ready for check-in"
        />
        <MetricCard
          title="Checked In"
          value={metrics.checkedInReservations}
          icon={DoorOpen}
          description="Currently staying"
        />
        <MetricCard
          title="Maintenance Rooms"
          value={metrics.maintenanceRooms}
          icon={Home}
          description="Under maintenance"
        />
      </div>
    </div>
  );
};

export default Dashboard;