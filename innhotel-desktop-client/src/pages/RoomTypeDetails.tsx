import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { RoomTypeForm } from '@/components/roomTypes/RoomTypeForm';
import { useRoomTypeStore } from '@/store/roomTypes.store';
import { branchService } from '@/services/branchService';
import { ROUTES } from '@/constants/routes';
import { toast } from 'sonner';
import { ArrowLeft, Loader2, Edit } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import type { RoomTypeFormData } from '@/schemas/roomTypeSchema';

interface Branch {
  id: number;
  name: string;
}

const RoomTypeDetails = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { selectedRoomType, isLoading, fetchRoomTypeById, updateRoomType } = useRoomTypeStore();
  const [branches, setBranches] = useState<Branch[]>([]);
  const [loadingBranches, setLoadingBranches] = useState(true);
  const [isEditMode, setIsEditMode] = useState(false);

  useEffect(() => {
    if (id) {
      loadRoomType(parseInt(id));
      loadBranches();
    }
  }, [id]);

  const loadRoomType = async (roomTypeId: number) => {
    try {
      await fetchRoomTypeById(roomTypeId);
    } catch (error) {
      toast.error('Failed to load room type', {
        description: error instanceof Error ? error.message : 'An unexpected error occurred',
      });
      navigate(ROUTES.ROOM_TYPES);
    }
  };

  const loadBranches = async () => {
    try {
      setLoadingBranches(true);
      const response = await branchService.getAll();
      setBranches(response.items.map((b: { id: number; name: string }) => ({ id: b.id, name: b.name })));
    } catch (error) {
      toast.error('Failed to load branches', {
        description: error instanceof Error ? error.message : 'An unexpected error occurred',
      });
    } finally {
      setLoadingBranches(false);
    }
  };

  const handleSubmit = async (data: RoomTypeFormData) => {
    if (!selectedRoomType) return;

    try {
      await updateRoomType({
        ...data,
        id: selectedRoomType.id,
        branchName: selectedRoomType.branchName,
        description: data.description || null
      });
      toast.success('Room type updated successfully', {
        description: `${data.name} has been updated.`,
      });
      setIsEditMode(false);
      await loadRoomType(selectedRoomType.id);
    } catch (error) {
      toast.error('Failed to update room type', {
        description: error instanceof Error ? error.message : 'An unexpected error occurred',
      });
    }
  };

  if (isLoading || loadingBranches || !selectedRoomType) {
    return (
      <div className="container mx-auto py-6">
        <div className="flex items-center justify-center h-64">
          <Loader2 className="h-8 w-8 animate-spin text-primary" />
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto py-6 max-w-2xl">
      <div className="mb-6">
        <Button
          variant="ghost"
          onClick={() => navigate(ROUTES.ROOM_TYPES)}
          className="mb-4"
        >
          <ArrowLeft className="mr-2 h-4 w-4" />
          Back to Room Types
        </Button>
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-2xl font-bold">{selectedRoomType.name}</h1>
            <p className="text-muted-foreground">Room type details and configuration</p>
          </div>
          {!isEditMode && (
            <Button onClick={() => setIsEditMode(true)}>
              <Edit className="mr-2 h-4 w-4" />
              Edit
            </Button>
          )}
        </div>
      </div>

      {isEditMode ? (
        <div className="bg-card rounded-lg border p-6">
          <div className="mb-4 flex justify-between items-center">
            <h2 className="text-lg font-semibold">Edit Room Type</h2>
            <Button variant="outline" onClick={() => setIsEditMode(false)}>
              Cancel
            </Button>
          </div>
          <RoomTypeForm
            defaultValues={{
              branchId: selectedRoomType.branchId,
              name: selectedRoomType.name,
              capacity: selectedRoomType.capacity,
              description: selectedRoomType.description || '',
            }}
            onSubmit={handleSubmit}
            branches={branches}
            isLoading={isLoading}
            submitLabel="Update Room Type"
          />
        </div>
      ) : (
        <Card>
          <CardHeader>
            <CardTitle>Room Type Information</CardTitle>
            <CardDescription>View detailed information about this room type</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div>
              <label className="text-sm font-medium text-muted-foreground">Branch</label>
              <p className="text-base">{selectedRoomType.branchName}</p>
            </div>
            <div>
              <label className="text-sm font-medium text-muted-foreground">Name</label>
              <p className="text-base">{selectedRoomType.name}</p>
            </div>
            <div>
              <label className="text-sm font-medium text-muted-foreground">Capacity</label>
              <div className="flex items-center gap-2">
                <Badge variant="secondary">
                  {selectedRoomType.capacity} {selectedRoomType.capacity === 1 ? 'Guest' : 'Guests'}
                </Badge>
              </div>
            </div>
            {selectedRoomType.description && (
              <div>
                <label className="text-sm font-medium text-muted-foreground">Description</label>
                <p className="text-base whitespace-pre-wrap">{selectedRoomType.description}</p>
              </div>
            )}
          </CardContent>
        </Card>
      )}
    </div>
  );
};

export default RoomTypeDetails;