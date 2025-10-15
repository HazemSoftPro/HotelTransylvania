import { RoomTypeCard } from './RoomTypeCard';
import type { RoomType } from '@/services/roomTypeService';

interface RoomTypesListingProps {
  roomTypes: RoomType[];
  onEdit: (roomType: RoomType) => void;
  onDelete: (id: number) => void;
  onView: (roomType: RoomType) => void;
}

export function RoomTypesListing({ roomTypes, onEdit, onDelete, onView }: RoomTypesListingProps) {
  if (roomTypes.length === 0) {
    return (
      <div className="text-center py-12">
        <p className="text-muted-foreground">No room types found. Create your first room type to get started.</p>
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      {roomTypes.map((roomType) => (
        <RoomTypeCard
          key={roomType.id}
          roomType={roomType}
          onEdit={onEdit}
          onDelete={onDelete}
          onView={onView}
        />
      ))}
    </div>
  );
}