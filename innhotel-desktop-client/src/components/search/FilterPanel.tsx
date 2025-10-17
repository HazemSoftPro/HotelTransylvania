import React, { useState } from 'react';
import { Filter, X } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Sheet, SheetContent, SheetDescription, SheetHeader, SheetTitle, SheetTrigger } from '@/components/ui/sheet';

interface FilterPanelProps {
  children: React.ReactNode;
  onApply: () => void;
  onReset: () => void;
  title?: string;
  description?: string;
  filterCount?: number;
}

/**
 * FilterPanel Component
 * 
 * A reusable filter panel component that displays in a side sheet.
 * Provides a consistent UI for applying and resetting filters.
 * 
 * @param children - Filter form elements to display
 * @param onApply - Callback when filters are applied
 * @param onReset - Callback when filters are reset
 * @param title - Panel title
 * @param description - Panel description
 * @param filterCount - Number of active filters (displayed as badge)
 */
export const FilterPanel: React.FC<FilterPanelProps> = ({
  children,
  onApply,
  onReset,
  title = 'Filters',
  description = 'Apply filters to refine your search results',
  filterCount = 0,
}) => {
  const [open, setOpen] = useState(false);

  const handleApply = () => {
    onApply();
    setOpen(false);
  };

  const handleReset = () => {
    onReset();
  };

  return (
    <Sheet open={open} onOpenChange={setOpen}>
      <SheetTrigger asChild>
        <Button variant="outline" className="relative">
          <Filter className="mr-2 h-4 w-4" />
          Filters
          {filterCount > 0 && (
            <span className="ml-2 flex h-5 w-5 items-center justify-center rounded-full bg-primary text-xs text-primary-foreground">
              {filterCount}
            </span>
          )}
        </Button>
      </SheetTrigger>
      <SheetContent className="w-[400px] sm:w-[540px] overflow-y-auto">
        <SheetHeader>
          <SheetTitle>{title}</SheetTitle>
          <SheetDescription>{description}</SheetDescription>
        </SheetHeader>
        <div className="mt-6 space-y-4">
          {children}
        </div>
        <div className="mt-6 flex gap-2">
          <Button onClick={handleApply} className="flex-1">
            Apply Filters
          </Button>
          <Button onClick={handleReset} variant="outline" className="flex-1">
            <X className="mr-2 h-4 w-4" />
            Reset
          </Button>
        </div>
      </SheetContent>
    </Sheet>
  );
};