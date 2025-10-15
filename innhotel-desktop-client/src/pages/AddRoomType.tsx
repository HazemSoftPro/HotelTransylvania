import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { RoomTypeForm } from '@/components/roomTypes/RoomTypeForm';
import { useRoomTypeStore } from '@/store/roomTypes.store';
import { branchService } from '@/services/branchService';
import { ROUTES } from '@/constants/routes';
import { toast } from 'sonner';
import { ArrowLeft, Loader2 } from 'lucide-react';
import { Button } from '@/components/ui/button';
import type { RoomTypeFormData } from '@/schemas/roomTypeSchema';

interface Branch {
  id: number;
  name: string;
}

const AddRoomType = () => {
  const navigate = useNavigate();
  const { createRoomType, isLoading } = useRoomTypeStore();
  const [branches, setBranches] = useState<Branch[]>([]);
  const [loadingBranches, setLoadingBranches] = useState(true);

  useEffect(() => {
    loadBranches();
  }, []);

  const loadBranches = async () => {
    try {
      setLoadingBranches(true);
      const response = await branchService.getAll();
      setBranches(response.data.map(b => ({ id: b.id, name: b.name })));
    } catch (error) {
      toast.error('Failed to load branches', {
        description: error instanceof Error ? error.message : 'An unexpected error occurred',
      });
    } finally {
      setLoadingBranches(false);
    }
  };

  const handleSubmit = async (data: RoomTypeFormData) => {
    try {
      await createRoomType(data);
      toast.success('Room type created successfully', {
        description: `${data.name} has been added to the system.`,
      });
      navigate(ROUTES.ROOM_TYPES);
    } catch (error) {
      toast.error('Failed to create room type', {
        description: error instanceof Error ? error.message : 'An unexpected error occurred',
      });
    }
  };

  if (loadingBranches) {
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
        <h1 className="text-2xl font-bold">Add New Room Type</h1>
        <p className="text-muted-foreground">
          Create a new room type configuration for your hotel.
        </p>
      </div>

      <div className="bg-card rounded-lg border p-6">
        <RoomTypeForm
          onSubmit={handleSubmit}
          branches={branches}
          isLoading={isLoading}
          submitLabel="Create Room Type"
        />
      </div>
    </div>
  );
};

export default AddRoomType;