import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from '@/components/ui/breadcrumb';
import { Home } from 'lucide-react';

interface BreadcrumbItem {
  label: string;
  path?: string;
}

/**
 * Breadcrumbs Component
 * 
 * Automatically generates breadcrumb navigation based on the current route.
 */
export const Breadcrumbs: React.FC = () => {
  const location = useLocation();
  const pathnames = location.pathname.split('/').filter((x) => x);

  // Route name mapping
  const routeNames: Record<string, string> = {
    dashboard: 'Dashboard',
    rooms: 'Rooms',
    'room-types': 'Room Types',
    reservations: 'Reservations',
    guests: 'Guests',
    employees: 'Employees',
    services: 'Services',
    branches: 'Branches',
    add: 'Add New',
    edit: 'Edit',
  };

  // Generate breadcrumb items
  const breadcrumbItems: BreadcrumbItem[] = pathnames.map((name, index) => {
    const path = `/${pathnames.slice(0, index + 1).join('/')}`;
    const label = routeNames[name] || name.charAt(0).toUpperCase() + name.slice(1);
    
    // Don't create link for numeric IDs (detail pages)
    const isNumeric = !isNaN(Number(name));
    
    return {
      label: isNumeric ? 'Details' : label,
      path: isNumeric ? undefined : path,
    };
  });

  // Don't show breadcrumbs on home page
  if (pathnames.length === 0) {
    return null;
  }

  return (
    <Breadcrumb className="mb-4">
      <BreadcrumbList>
        <BreadcrumbItem>
          <BreadcrumbLink asChild>
            <Link to="/" className="flex items-center gap-1">
              <Home className="h-4 w-4" />
              <span>Home</span>
            </Link>
          </BreadcrumbLink>
        </BreadcrumbItem>
        
        {breadcrumbItems.map((item, index) => (
          <React.Fragment key={index}>
            <BreadcrumbSeparator />
            <BreadcrumbItem>
              {index === breadcrumbItems.length - 1 || !item.path ? (
                <BreadcrumbPage>{item.label}</BreadcrumbPage>
              ) : (
                <BreadcrumbLink asChild>
                  <Link to={item.path}>{item.label}</Link>
                </BreadcrumbLink>
              )}
            </BreadcrumbItem>
          </React.Fragment>
        ))}
      </BreadcrumbList>
    </Breadcrumb>
  );
};