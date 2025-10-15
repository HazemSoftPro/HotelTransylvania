import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Edit, Trash2, Users, Building2 } from 'lucide-react';
import type { RoomType } from '@/services/roomTypeService';

interface RoomTypeCardProps {
  roomType: RoomType;
  onEdit: (roomType: RoomType) => void;
  onDelete: (id: number) => void;
  onView: (roomType: RoomType) => void;
}

export function RoomTypeCard({ roomType, onEdit, onDelete, onView }: RoomTypeCardProps) {
  return (
    <Card className="hover:shadow-lg transition-shadow cursor-pointer">
      <CardHeader onClick={() => onView(roomType)}>
        <div className="flex justify-between items-start">
          <div className="flex-1">
            <CardTitle className="text-xl mb-2">{roomType.name}</CardTitle>
            <CardDescription className="flex items-center gap-2">
              <Building2 className="h-4 w-4" />
              {roomType.branchName}
            </CardDescription>
          </div>
          <Badge variant="secondary" className="flex items-center gap-1">
            <Users className="h-3 w-3" />
            {roomType.capacity} {roomType.capacity === 1 ? 'Guest' : 'Guests'}
          </Badge>
        </div>
      </CardHeader>
      <CardContent>
        {roomType.description && (
          <p className="text-sm text-muted-foreground mb-4 line-clamp-2">
            {roomType.description}
          </p>
        )}
        <div className="flex justify-end gap-2">
          <Button
            variant="outline"
            size="sm"
            onClick={(e) => {
              e.stopPropagation();
              onEdit(roomType);
            }}
          >
            <Edit className="h-4 w-4 mr-1" />
            Edit
          </Button>
          <Button
            variant="destructive"
            size="sm"
            onClick={(e) => {
              e.stopPropagation();
              onDelete(roomType.id);
            }}
          >
            <Trash2 className="h-4 w-4 mr-1" />
            Delete
          </Button>
        </div>
      </CardContent>
    </Card>
  );
}