import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { ServiceForm } from '@/components/services/ServiceForm';
import { useServiceStore } from '@/store/services.store';
import { branchService } from '@/services/branchService';
import { ROUTES } from '@/constants/routes';
import { toast } from 'sonner';
import { ArrowLeft, Loader2, Edit } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import type { UpdateServiceFormData } from '@/schemas/serviceSchema';

interface Branch {
  id: number;
  name: string;
}

const ServiceDetails = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { selectedService, isLoading, fetchServiceById, updateService } = useServiceStore();
  const [branches, setBranches] = useState<Branch[]>([]);
  const [loadingBranches, setLoadingBranches] = useState(true);
  const [isEditMode, setIsEditMode] = useState(false);

  useEffect(() => {
    if (id) {
      loadService(parseInt(id));
      loadBranches();
    }
  }, [id]);

  const loadService = async (serviceId: number) => {
    try {
      await fetchServiceById(serviceId);
    } catch (error) {
      toast.error('Failed to load service', {
        description: error instanceof Error ? error.message : 'An unexpected error occurred',
      });
      navigate(ROUTES.SERVICES);
    }
  };

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

  const handleSubmit = async (data: UpdateServiceFormData) => {
    if (!selectedService) return;

    try {
      await updateService({
        ...data,
        id: selectedService.id,
        branchName: selectedService.branchName,
      });
      toast.success('Service updated successfully', {
        description: `${data.name} has been updated.`,
      });
      setIsEditMode(false);
      await loadService(selectedService.id);
    } catch (error) {
      toast.error('Failed to update service', {
        description: error instanceof Error ? error.message : 'An unexpected error occurred',
      });
    }
  };

  if (isLoading || loadingBranches || !selectedService) {
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
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-2xl font-bold">{selectedService.name}</h1>
            <p className="text-muted-foreground">Service details and configuration</p>
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
            <h2 className="text-lg font-semibold">Edit Service</h2>
            <Button variant="outline" onClick={() => setIsEditMode(false)}>
              Cancel
            </Button>
          </div>
          <ServiceForm
            defaultValues={{
              branchId: selectedService.branchId,
              name: selectedService.name,
              price: selectedService.price,
              description: selectedService.description || '',
            }}
            onSubmit={handleSubmit}
            branches={branches}
            isLoading={isLoading}
            submitLabel="Update Service"
          />
        </div>
      ) : (
        <Card>
          <CardHeader>
            <CardTitle>Service Information</CardTitle>
            <CardDescription>View detailed information about this service</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div>
              <label className="text-sm font-medium text-muted-foreground">Branch</label>
              <p className="text-base">{selectedService.branchName}</p>
            </div>
            <div>
              <label className="text-sm font-medium text-muted-foreground">Name</label>
              <p className="text-base">{selectedService.name}</p>
            </div>
            <div>
              <label className="text-sm font-medium text-muted-foreground">Price</label>
              <div className="flex items-center gap-2">
                <Badge variant="secondary" className="text-base">
                  ${selectedService.price.toFixed(2)}
                </Badge>
              </div>
            </div>
            {selectedService.description && (
              <div>
                <label className="text-sm font-medium text-muted-foreground">Description</label>
                <p className="text-base whitespace-pre-wrap">{selectedService.description}</p>
              </div>
            )}
          </CardContent>
        </Card>
      )}
    </div>
  );
};

export default ServiceDetails;