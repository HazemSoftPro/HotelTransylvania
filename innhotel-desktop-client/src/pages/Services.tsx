import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { ServicesListing } from '@/components/services/ServicesListing';
import { useServiceStore } from '@/store/services.store';
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
import type { HotelService } from '@/services/serviceService';

const Services = () => {
  const navigate = useNavigate();
  const { services, isLoading, fetchServices, deleteService } = useServiceStore();
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [serviceToDelete, setServiceToDelete] = useState<number | null>(null);
  const [isDeleting, setIsDeleting] = useState(false);

  useEffect(() => {
    loadServices();
  }, []);

  const loadServices = async () => {
    try {
      await fetchServices();
    } catch (error) {
      toast.error('Failed to load services', {
        description: error instanceof Error ? error.message : 'An unexpected error occurred',
      });
    }
  };

  const handleServiceClick = (service: HotelService) => {
    navigate(`${ROUTES.SERVICES}/${service.id}`);
  };

  const handleEdit = (service: HotelService) => {
    navigate(`${ROUTES.SERVICES}/${service.id}/edit`);
  };

  const handleDeleteClick = (id: number) => {
    setServiceToDelete(id);
    setDeleteDialogOpen(true);
  };

  const handleDeleteConfirm = async () => {
    if (!serviceToDelete) return;

    setIsDeleting(true);
    try {
      await deleteService(serviceToDelete);
      toast.success('Service deleted successfully');
      setDeleteDialogOpen(false);
      setServiceToDelete(null);
    } catch (error) {
      toast.error('Failed to delete service', {
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
          <h1 className="text-2xl font-bold">Services Management</h1>
          <p className="text-muted-foreground">
            Manage your hotel services and amenities.
          </p>
        </div>
        <Button onClick={() => navigate(ROUTES.ADD_SERVICE)} className="gap-2">
          <Plus className="h-4 w-4" />
          Add Service
        </Button>
      </div>

      <ServicesListing
        services={services}
        onEdit={handleEdit}
        onDelete={handleDeleteClick}
        onView={handleServiceClick}
      />

      <AlertDialog open={deleteDialogOpen} onOpenChange={setDeleteDialogOpen}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Are you sure?</AlertDialogTitle>
            <AlertDialogDescription>
              This action cannot be undone. This will permanently delete the service
              and may affect existing reservations using this service.
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

export default Services;