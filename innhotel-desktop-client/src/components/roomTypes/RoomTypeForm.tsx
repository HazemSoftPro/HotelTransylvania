import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { roomTypeSchema, type RoomTypeFormData } from '@/schemas/roomTypeSchema';
import { Button } from '@/components/ui/button';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
  FormDescription,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Loader2 } from 'lucide-react';

interface Branch {
  id: number;
  name: string;
}

interface RoomTypeFormProps {
  defaultValues?: Partial<RoomTypeFormData>;
  onSubmit: (data: RoomTypeFormData) => Promise<void>;
  branches: Branch[];
  isLoading?: boolean;
  submitLabel?: string;
}

export function RoomTypeForm({
  defaultValues,
  onSubmit,
  branches,
  isLoading = false,
  submitLabel = 'Submit',
}: RoomTypeFormProps) {
  const form = useForm<RoomTypeFormData>({
    resolver: zodResolver(roomTypeSchema),
    defaultValues: {
      branchId: defaultValues?.branchId || 0,
      name: defaultValues?.name || '',
      capacity: defaultValues?.capacity || 1,
      description: defaultValues?.description || '',
    },
  });

  useEffect(() => {
    if (defaultValues) {
      form.reset({
        branchId: defaultValues.branchId || 0,
        name: defaultValues.name || '',
        capacity: defaultValues.capacity || 1,
        description: defaultValues.description || '',
      });
    }
  }, [defaultValues, form]);

  const handleSubmit = async (data: RoomTypeFormData) => {
    try {
      await onSubmit(data);
      if (!defaultValues) {
        form.reset();
      }
    } catch (error) {
      console.error('Form submission error:', error);
    }
  };

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(handleSubmit)} className="space-y-6">
        <FormField
          control={form.control}
          name="branchId"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Branch *</FormLabel>
              <Select
                onValueChange={(value) => field.onChange(parseInt(value))}
                value={field.value?.toString()}
                disabled={isLoading}
              >
                <FormControl>
                  <SelectTrigger>
                    <SelectValue placeholder="Select a branch" />
                  </SelectTrigger>
                </FormControl>
                <SelectContent>
                  {branches.map((branch) => (
                    <SelectItem key={branch.id} value={branch.id.toString()}>
                      {branch.name}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
              <FormDescription>
                Select the branch where this room type will be available
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="name"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Room Type Name *</FormLabel>
              <FormControl>
                <Input
                  placeholder="e.g., Deluxe Suite, Standard Room"
                  {...field}
                  disabled={isLoading}
                />
              </FormControl>
              <FormDescription>
                Enter a descriptive name for this room type
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="capacity"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Capacity *</FormLabel>
              <FormControl>
                <Input
                  type="number"
                  placeholder="e.g., 2"
                  {...field}
                  onChange={(e) => field.onChange(parseInt(e.target.value) || 0)}
                  disabled={isLoading}
                  min={1}
                  max={20}
                />
              </FormControl>
              <FormDescription>
                Maximum number of guests this room type can accommodate
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="description"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Description</FormLabel>
              <FormControl>
                <Textarea
                  placeholder="Enter a detailed description of this room type..."
                  className="resize-none"
                  rows={4}
                  {...field}
                  disabled={isLoading}
                />
              </FormControl>
              <FormDescription>
                Optional description of amenities, features, and characteristics
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />

        <div className="flex justify-end gap-4">
          <Button
            type="button"
            variant="outline"
            onClick={() => form.reset()}
            disabled={isLoading}
          >
            Reset
          </Button>
          <Button type="submit" disabled={isLoading}>
            {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
            {submitLabel}
          </Button>
        </div>
      </form>
    </Form>
  );
}