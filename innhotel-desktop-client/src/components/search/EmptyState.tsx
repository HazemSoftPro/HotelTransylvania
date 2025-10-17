import React from 'react';
import { Search, FileX } from 'lucide-react';
import { Button } from '@/components/ui/button';

interface EmptyStateProps {
  title?: string;
  description?: string;
  icon?: 'search' | 'empty';
  actionLabel?: string;
  onAction?: () => void;
}

/**
 * EmptyState Component
 * 
 * A reusable component to display when no data is available.
 * 
 * @param title - Main message to display
 * @param description - Additional description text
 * @param icon - Icon to display ('search' for no results, 'empty' for no data)
 * @param actionLabel - Label for action button
 * @param onAction - Callback when action button is clicked
 */
export const EmptyState: React.FC<EmptyStateProps> = ({
  title = 'No results found',
  description = 'Try adjusting your search or filters to find what you\'re looking for.',
  icon = 'search',
  actionLabel,
  onAction,
}) => {
  const Icon = icon === 'search' ? Search : FileX;

  return (
    <div className="flex flex-col items-center justify-center py-12 px-4 text-center">
      <div className="rounded-full bg-muted p-6 mb-4">
        <Icon className="h-12 w-12 text-muted-foreground" />
      </div>
      <h3 className="text-lg font-semibold mb-2">{title}</h3>
      <p className="text-sm text-muted-foreground max-w-md mb-6">{description}</p>
      {actionLabel && onAction && (
        <Button onClick={onAction} variant="outline">
          {actionLabel}
        </Button>
      )}
    </div>
  );
};