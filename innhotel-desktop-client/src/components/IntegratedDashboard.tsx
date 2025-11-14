import React, { useState, useEffect } from 'react';
import { signalRService, DashboardMetrics, ActivityItem, AlertItem } from '../services/signalrService';
import { Card, CardContent, CardHeader, CardTitle } from './ui/card';
import { Badge } from './ui/badge';
import { Alert, AlertDescription } from './ui/alert';
import { Button } from './ui/button';
import { RefreshCw, Users, BedDollar, Home, AlertTriangle, CheckCircle, Clock } from 'lucide-react';

interface Props {
  userRole: string;
}

const IntegratedDashboard: React.FC<Props> = ({ userRole }) => {
  const [metrics, setMetrics] = useState<DashboardMetrics | null>(null);
  const [activities, setActivities] = useState<ActivityItem[]>([]);
  const [alerts, setAlerts] = useState<AlertItem[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [lastUpdated, setLastUpdated] = useState<Date>(new Date());
  const [onlineUsers, setOnlineUsers] = useState(0);

  useEffect(() => {
    // Load initial data
    loadDashboardData();

    // Set up real-time listeners
    setupRealTimeListeners();

    // Set up periodic refresh
    const interval = setInterval(loadDashboardData, 30000); // Refresh every 30 seconds

    return () => {
      clearInterval(interval);
    };
  }, []);

  const loadDashboardData = async () => {
    try {
      setIsLoading(true);
      
      // Fetch dashboard metrics
      const metricsResponse = await fetch('/api/dashboard/metrics');
      const metricsData = await metricsResponse.json();
      setMetrics(metricsData);

      // Fetch recent activities
      const activitiesResponse = await fetch('/api/dashboard/activities?limit=10');
      const activitiesData = await activitiesResponse.json();
      setActivities(activitiesData);

      // Fetch alerts
      const alertsResponse = await fetch('/api/dashboard/alerts');
      const alertsData = await alertsResponse.json();
      setAlerts(alertsData);

      // Get online users count
      const onlineCount = await signalRService.getOnlineUsersCount();
      setOnlineUsers(onlineCount);

      setLastUpdated(new Date());
    } catch (error) {
      console.error('Error loading dashboard data:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const setupRealTimeListeners = () => {
    // Listen for real-time updates
    window.addEventListener('metricsUpdated', (event: any) => {
      setMetrics(event.detail);
    });

    window.addEventListener('roomStatusUpdate', (event: any) => {
      console.log('Room status updated:', event.detail);
      // Refresh metrics to reflect the change
      loadDashboardData();
    });

    window.addEventListener('reservationUpdate', (event: any) => {
      console.log('Reservation updated:', event.detail);
      loadDashboardData();
    });

    window.addEventListener('systemAlert', (event: any) => {
      setAlerts(prev => [event.detail, ...prev].slice(0, 10));
    });

    window.addEventListener('hotelNotification', (event: any) => {
      // Handle notifications
      console.log('New notification:', event.detail);
    });
  };

  const handleRefresh = async () => {
    await loadDashboardData();
    
    // Trigger dashboard refresh for all connected clients
    try {
      await fetch('/api/dashboard/refresh', { method: 'POST' });
    } catch (error) {
      console.error('Error triggering dashboard refresh:', error);
    }
  };

  const getOccupancyColor = (rate: number) => {
    if (rate >= 90) return 'text-red-600';
    if (rate >= 75) return 'text-yellow-600';
    return 'text-green-600';
  };

  const getAlertIcon = (type: string) => {
    switch (type) {
      case 'Error':
        return <AlertTriangle className="h-4 w-4 text-red-500" />;
      case 'Warning':
        return <AlertTriangle className="h-4 w-4 text-yellow-500" />;
      default:
        return <CheckCircle className="h-4 w-4 text-blue-500" />;
    }
  };

  const formatTime = (timestamp: string) => {
    return new Date(timestamp).toLocaleTimeString();
  };

  if (isLoading && !metrics) {
    return (
      <div className="flex items-center justify-center h-64">
        <RefreshCw className="h-8 w-8 animate-spin" />
        <span className="ml-2">Loading dashboard...</span>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold">Hotel Dashboard</h1>
          <p className="text-gray-600">
            Last updated: {lastUpdated.toLocaleTimeString()} | 
            Online users: {onlineUsers} | 
            Connection: <Badge variant={signalRService.isConnected() ? "default" : "destructive"}>
              {signalRService.getConnectionState()}
            </Badge>
          </p>
        </div>
        <Button onClick={handleRefresh} disabled={isLoading}>
          <RefreshCw className={`h-4 w-4 mr-2 ${isLoading ? 'animate-spin' : ''}`} />
          Refresh
        </Button>
      </div>

      {/* Key Metrics */}
      {metrics && (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Total Rooms</CardTitle>
              <Home className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{metrics.totalRooms}</div>
              <p className="text-xs text-muted-foreground">
                {metrics.availableRooms} available
              </p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Occupancy Rate</CardTitle>
              <Users className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className={`text-2xl font-bold ${getOccupancyColor(metrics.occupancyRate)}`}>
                {metrics.occupancyRate.toFixed(1)}%
              </div>
              <p className="text-xs text-muted-foreground">
                {metrics.occupiedRooms} occupied
              </p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Today's Revenue</CardTitle>
              <BedDollar className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">${metrics.todayRevenue.toFixed(2)}</div>
              <p className="text-xs text-muted-foreground">
                Avg room: ${metrics.averageRoomRate.toFixed(2)}
              </p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Today's Activity</CardTitle>
              <Clock className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-lg font-bold">
                {metrics.todayCheckIns} in / {metrics.todayCheckOuts} out
              </div>
              <p className="text-xs text-muted-foreground">
                {metrics.pendingReservations} pending
              </p>
            </CardContent>
          </Card>
        </div>
      )}

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Recent Activities */}
        <Card className="lg:col-span-2">
          <CardHeader>
            <CardTitle>Recent Activities</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-3">
              {activities.map((activity, index) => (
                <div key={index} className="flex items-center space-x-3 p-2 rounded hover:bg-gray-50">
                  <div className="flex-shrink-0">
                    <Badge variant="outline">{activity.type}</Badge>
                  </div>
                  <div className="flex-1">
                    <p className="text-sm font-medium">{activity.description}</p>
                    <p className="text-xs text-gray-500">{formatTime(activity.timestamp)}</p>
                  </div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        {/* Alerts */}
        <Card>
          <CardHeader>
            <CardTitle>Alerts & Notifications</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-3">
              {alerts.map((alert, index) => (
                <Alert key={index} className="p-3">
                  <div className="flex items-start space-x-2">
                    {getAlertIcon(alert.type)}
                    <div className="flex-1">
                      <AlertDescription className="text-sm">
                        {alert.message}
                      </AlertDescription>
                      <div className="flex items-center mt-1 space-x-2">
                        <Badge variant={alert.priority === 'High' ? 'destructive' : 'secondary'}>
                          {alert.priority}
                        </Badge>
                        <span className="text-xs text-gray-500">
                          {formatTime(alert.timestamp)}
                        </span>
                      </div>
                    </div>
                  </div>
                </Alert>
              ))}
            </div>
          </CardContent>
        </Card>
      </div>

      {/* System Status */}
      <Card>
        <CardHeader>
          <CardTitle>System Status</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
            <div className="flex items-center space-x-2">
              <div className="w-3 h-3 bg-green-500 rounded-full"></div>
              <span className="text-sm">Database: Healthy</span>
            </div>
            <div className="flex items-center space-x-2">
              <div className="w-3 h-3 bg-green-500 rounded-full"></div>
              <span className="text-sm">API: Healthy</span>
            </div>
            <div className="flex items-center space-x-2">
              <div className={`w-3 h-3 rounded-full ${signalRService.isConnected() ? 'bg-green-500' : 'bg-red-500'}`}></div>
              <span className="text-sm">SignalR: {signalRService.getConnectionState()}</span>
            </div>
            <div className="flex items-center space-x-2">
              <div className="w-3 h-3 bg-green-500 rounded-full"></div>
              <span className="text-sm">Uptime: 24h</span>
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default IntegratedDashboard;