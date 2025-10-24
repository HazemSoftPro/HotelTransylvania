import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { ServiceForm } from '@/components/services/ServiceForm';
import { useServiceStore } from '@/store/services.store';
import { branchService } from '@/services/branchService';
import { ROUTES } from '@/constants/routes';
import { toast } from 'sonner';
import { ArrowLeft, Loader2 } from 'lucide-react';
import { Button } from '@/components/ui/button';
import type { ServiceFormData } from '@/schemas/serviceSchema';

interface Branch {
  id: number;
  name: string;
}

const AddService = () => {
  const navigate = useNavigate();
  const { createService, isLoading } = useServiceStore();
  const [branches, setBranches] = useState<Branch[]>([]);
  const [loadingBranches, setLoadingBranches] = useState(true);

  useEffect(() => {
    loadBranches();
  }, []);

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

  const handleSubmit = async (data: ServiceFormData) => {
    try {
      await createService({
        ...data,
        description: data.description || null
      });
      toast.success('Service created successfully', {
        description: `${data.name} has been added to the system.`,
      });
      navigate(ROUTES.SERVICES);
    } catch (error) {
      toast.error('Failed to create service', {
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
          onClick={() => navigate(ROUTES.SERVICES)}
          className="mb-4"
        >
          <ArrowLeft className="mr-2 h-4 w-4" />
          Back to Services
        </Button>
        <h1 className="text-2xl font-bold">Add New Service</h1>
        <p className="text-muted-foreground">
          Create a new service or amenity for your hotel.
        </p>
      </div>

      <div className="bg-card rounded-lg border p-6">
        <ServiceForm
          onSubmit={handleSubmit}
          branches={branches}
          isLoading={isLoading}
          submitLabel="Create Service"
        />
      </div>
    </div>
  );
};

export default AddService;