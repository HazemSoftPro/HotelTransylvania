import { useEffect, useState, useCallback, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { RoomTypesListing } from '@/components/roomTypes/RoomTypesListing';
import { RoomTypeFilters, type RoomTypeFilterValues } from '@/components/roomTypes/RoomTypeFilters';
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
  const [filters, setFilters] = useState<RoomTypeFilterValues>({
    search: "",
    branchId: "",
    capacity: "",
  });

  const loadRoomTypes = useCallback(async () => {
    try {
      await fetchRoomTypes();
    } catch (error) {
      toast.error('Failed to load room types', {
        description: error instanceof Error ? error.message : 'An unexpected error occurred',
      });
    }
  }, [fetchRoomTypes]);

  useEffect(() => {
    loadRoomTypes();
  }, [loadRoomTypes]);

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

  const handleFilterChange = (newFilters: RoomTypeFilterValues) => {
    setFilters(newFilters);
  };

  const handleResetFilters = () => {
    setFilters({
      search: "",
      branchId: "",
      capacity: "",
    });
  };

  // Apply client-side filtering
  const filteredRoomTypes = useMemo(() => {
    return roomTypes.filter((roomType) => {
      // Search filter
      if (filters.search && !roomType.name.toLowerCase().includes(filters.search.toLowerCase())) {
        return false;
      }

      // Branch filter
      if (filters.branchId && roomType.branchId.toString() !== filters.branchId) {
        return false;
      }

      // Capacity filter
      if (filters.capacity && roomType.capacity.toString() !== filters.capacity) {
        return false;
      }

      return true;
    });
  }, [roomTypes, filters]);

  if (isLoading) {
    return (
      <div className="container mx-auto py-6 space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-2xl font-bold">Room Types Management</h1>
            <p className="text-muted-foreground">
              Manage your hotel room types and their configurations.
            </p>
          </div>
          <Button disabled className="gap-2">
            <Plus className="h-4 w-4" />
            Add Room Type
          </Button>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {Array.from({ length: 6 }).map((_, i) => (
            <div key={i} className="animate-pulse">
              <div className="bg-card rounded-lg border p-6 space-y-4">
                <div className="h-6 bg-muted rounded w-3/4"></div>
                <div className="h-4 bg-muted rounded w-1/2"></div>
                <div className="h-4 bg-muted rounded w-full"></div>
                <div className="flex justify-end gap-2">
                  <div className="h-9 bg-muted rounded w-20"></div>
                  <div className="h-9 bg-muted rounded w-20"></div>
                </div>
              </div>
            </div>
          ))}
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

      <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">
        <div className="lg:col-span-1">
          <RoomTypeFilters 
            onFilterChange={handleFilterChange}
            onReset={handleResetFilters}
          />
        </div>
        
        <div className="lg:col-span-3">
          <RoomTypesListing
            roomTypes={filteredRoomTypes}
            onEdit={handleEdit}
            onDelete={handleDeleteClick}
            onView={handleRoomTypeClick}
          />
        </div>
      </div>

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