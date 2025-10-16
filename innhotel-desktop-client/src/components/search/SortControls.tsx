import React from 'react';
import { ArrowUpDown, ArrowUp, ArrowDown } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';

export type SortDirection = 'asc' | 'desc' | null;

export interface SortOption {
  value: string;
  label: string;
}

interface SortControlsProps {
  sortBy: string;
  sortDirection: SortDirection;
  sortOptions: SortOption[];
  onSortChange: (sortBy: string, direction: SortDirection) => void;
}

/**
 * SortControls Component
 * 
 * A reusable component for sorting data with field selection and direction toggle.
 * 
 * @param sortBy - Current sort field
 * @param sortDirection - Current sort direction ('asc', 'desc', or null)
 * @param sortOptions - Available sort field options
 * @param onSortChange - Callback when sort changes
 */
export const SortControls: React.FC<SortControlsProps> = ({
  sortBy,
  sortDirection,
  sortOptions,
  onSortChange,
}) => {
  const handleSortFieldChange = (value: string) => {
    onSortChange(value, sortDirection || 'asc');
  };

  const handleSortDirectionToggle = () => {
    if (!sortDirection) {
      onSortChange(sortBy, 'asc');
    } else if (sortDirection === 'asc') {
      onSortChange(sortBy, 'desc');
    } else {
      onSortChange(sortBy, null);
    }
  };

  const getSortIcon = () => {
    if (!sortDirection) return <ArrowUpDown className="h-4 w-4" />;
    if (sortDirection === 'asc') return <ArrowUp className="h-4 w-4" />;
    return <ArrowDown className="h-4 w-4" />;
  };

  return (
    <div className="flex items-center gap-2">
      <span className="text-sm text-muted-foreground">Sort by:</span>
      <Select value={sortBy} onValueChange={handleSortFieldChange}>
        <SelectTrigger className="h-9 w-[180px]">
          <SelectValue placeholder="Select field" />
        </SelectTrigger>
        <SelectContent>
          {sortOptions.map((option) => (
            <SelectItem key={option.value} value={option.value}>
              {option.label}
            </SelectItem>
          ))}
        </SelectContent>
      </Select>
      <Button
        variant="outline"
        size="sm"
        onClick={handleSortDirectionToggle}
        className="h-9 w-9 p-0"
      >
        {getSortIcon()}
      </Button>
    </div>
  );
};