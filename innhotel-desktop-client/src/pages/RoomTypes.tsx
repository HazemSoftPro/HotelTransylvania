import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { RoomTypesListing } from '@/components/roomTypes/RoomTypesListing';
import { useRoomTypeStore } from '@/store/roomTypes.store';
import { Button } from '@/components/ui/button';
import { Plus, Loader2 } from 'lucide-react';
import { ROUTES } from '@/constants/routes';
import { toast } from 'sonner';
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from '@/components/ui/alert-dialog';
import type { RoomType } from '@/services/roomTypeService';

const RoomTypes = () => {
  const navigate = useNavigate();
  const { roomTypes, isLoading, fetchRoomTypes, deleteRoomType } = useRoomTypeStore();
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [roomTypeToDelete, setRoomTypeToDelete] = useState<number | null>(null);
  const [isDeleting, setIsDeleting] = useState(false);

  useEffect(() => {
    loadRoomTypes();
  }, []);

  const loadRoomTypes = async () => {
    try {
      await fetchRoomTypes();
    } catch (error) {
      toast.error('Failed to load room types', {
        description: error instanceof Error ? error.message : 'An unexpected error occurred',
      });
    }
  };

  const handleRoomTypeClick = (roomType: RoomType) => {
    navigate(`${ROUTES.ROOM_TYPES}/${roomType.id}`);
  };

  const handleEdit = (roomType: RoomType) => {
    navigate(`${ROUTES.ROOM_TYPES}/${roomType.id}/edit`);
  };

  const handleDeleteClick = (id: number) => {
    setRoomTypeToDelete(id);
    setDeleteDialogOpen(true);
  };

  const handleDeleteConfirm = async () => {
    if (!roomTypeToDelete) return;

    setIsDeleting(true);
    try {
      await deleteRoomType(roomTypeToDelete);
      toast.success('Room type deleted successfully');
      setDeleteDialogOpen(false);
      setRoomTypeToDelete(null);
    } catch (error) {
      toast.error('Failed to delete room type', {
        description: error instanceof Error ? error.message : 'An unexpected error occurred',
      });
    } finally {
      setIsDeleting(false);
    }
  };

  if (isLoading) {
    return (
      <div className="container mx-auto py-6">
        <div className="flex items-center justify-center h-64">
          <Loader2 className="h-8 w-8 animate-spin text-primary" />
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto py-6 space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold">Room Types Management</h1>
          <p className="text-muted-foreground">
            Manage your hotel room types and their configurations.
          </p>
        </div>
        <Button onClick={() => navigate(ROUTES.ADD_ROOM_TYPE)} className="gap-2">
          <Plus className="h-4 w-4" />
          Add Room Type
        </Button>
      </div>

      <RoomTypesListing
        roomTypes={roomTypes}
        onEdit={handleEdit}
        onDelete={handleDeleteClick}
        onView={handleRoomTypeClick}
      />

      <AlertDialog open={deleteDialogOpen} onOpenChange={setDeleteDialogOpen}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Are you sure?</AlertDialogTitle>
            <AlertDialogDescription>
              This action cannot be undone. This will permanently delete the room type
              and may affect existing rooms using this type.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel disabled={isDeleting}>Cancel</AlertDialogCancel>
            <AlertDialogAction
              onClick={handleDeleteConfirm}
              disabled={isDeleting}
              className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
            >
              {isDeleting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
              Delete
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </div>
  );
};

export default RoomTypes;