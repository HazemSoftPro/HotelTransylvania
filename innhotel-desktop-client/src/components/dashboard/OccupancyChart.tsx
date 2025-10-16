import React from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { PieChart } from 'lucide-react';

interface OccupancyChartProps {
  totalRooms: number;
  availableRooms: number;
  occupiedRooms: number;
  maintenanceRooms: number;
  occupancyRate: number;
}

/**
 * OccupancyChart Component
 * 
 * Displays room occupancy statistics in a visual format.
 */
export const OccupancyChart: React.FC<OccupancyChartProps> = ({
  totalRooms,
  availableRooms,
  occupiedRooms,
  maintenanceRooms,
  occupancyRate,
}) => {
  const outOfServiceRooms = totalRooms - availableRooms - occupiedRooms - maintenanceRooms;

  const stats = [
    { label: 'Available', value: availableRooms, color: 'bg-green-500', percentage: (availableRooms / totalRooms * 100).toFixed(1) },
    { label: 'Occupied', value: occupiedRooms, color: 'bg-blue-500', percentage: (occupiedRooms / totalRooms * 100).toFixed(1) },
    { label: 'Maintenance', value: maintenanceRooms, color: 'bg-yellow-500', percentage: (maintenanceRooms / totalRooms * 100).toFixed(1) },
    { label: 'Out of Service', value: outOfServiceRooms, color: 'bg-red-500', percentage: (outOfServiceRooms / totalRooms * 100).toFixed(1) },
  ];

  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <PieChart className="h-5 w-5" />
          Room Occupancy
        </CardTitle>
      </CardHeader>
      <CardContent>
        <div className="space-y-6">
          <div className="text-center">
            <div className="text-4xl font-bold">{occupancyRate.toFixed(1)}%</div>
            <p className="text-sm text-muted-foreground mt-1">Occupancy Rate</p>
          </div>

          <div className="space-y-3">
            {stats.map((stat) => (
              <div key={stat.label} className="space-y-2">
                <div className="flex items-center justify-between text-sm">
                  <div className="flex items-center gap-2">
                    <div className={`w-3 h-3 rounded-full ${stat.color}`} />
                    <span>{stat.label}</span>
                  </div>
                  <span className="font-medium">
                    {stat.value} ({stat.percentage}%)
                  </span>
                </div>
                <div className="w-full bg-secondary rounded-full h-2">
                  <div
                    className={`h-2 rounded-full ${stat.color}`}
                    style={{ width: `${stat.percentage}%` }}
                  />
                </div>
              </div>
            ))}
          </div>

          <div className="pt-4 border-t">
            <div className="flex items-center justify-between text-sm">
              <span className="text-muted-foreground">Total Rooms</span>
              <span className="font-medium">{totalRooms}</span>
            </div>
          </div>
        </div>
      </CardContent>
    </Card>
  );
};