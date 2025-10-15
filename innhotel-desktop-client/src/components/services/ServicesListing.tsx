import { ServiceCard } from './ServiceCard';
import type { HotelService } from '@/services/serviceService';

interface ServicesListingProps {
  services: HotelService[];
  onEdit: (service: HotelService) => void;
  onDelete: (id: number) => void;
  onView: (service: HotelService) => void;
}

export function ServicesListing({ services, onEdit, onDelete, onView }: ServicesListingProps) {
  if (services.length === 0) {
    return (
      <div className="text-center py-12">
        <p className="text-muted-foreground">No services found. Create your first service to get started.</p>
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      {services.map((service) => (
        <ServiceCard
          key={service.id}
          service={service}
          onEdit={onEdit}
          onDelete={onDelete}
          onView={onView}
        />
      ))}
    </div>
  );
}