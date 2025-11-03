import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { branchService } from '@/services/branchService';
import { roomTypeService } from '@/services/roomTypeService';
import { roomSchema } from '@/schemas/roomSchema';
import { roomStatusOptions } from '@/types/room';
import type { RoomStatus } from '@/types/api/room';
import type { RoomFormValues } from '@/schemas/roomSchema';
import { Button } from '@/components/ui/button';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import type { BranchResponse } from '@/types/api/branch';

import type { RoomType } from '@/services/roomTypeService';

interface CreateRoomRequest {
  branchId: number;
  roomTypeId: number;
  roomNumber: string;
  status: RoomStatus;
  floor: number;
  manualPrice: number;
}

export interface UpdateRoomRequest {
  roomTypeId: number;
  roomNumber: string;
  status: RoomStatus;
  floor: number;
  manualPrice: number;
}

interface BaseRoomFormProps {
  defaultValues?: Partial<RoomFormValues>;
  isLoading?: boolean;
}

interface CreateRoomFormProps extends BaseRoomFormProps {
  mode: 'create';
  onSubmit: (data: CreateRoomRequest) => void;
}

interface UpdateRoomFormProps extends BaseRoomFormProps {
  mode: 'update';
  onSubmit: (data: UpdateRoomRequest) => void;
}

type RoomFormProps = CreateRoomFormProps | UpdateRoomFormProps;

export const RoomForm = ({ mode, onSubmit, defaultValues, isLoading }: RoomFormProps) => {
  const [branches, setBranches] = useState<BranchResponse[]>([]);
  const [roomTypes, setRoomTypes] = useState<RoomType[]>([]);
  const [isLoadingBranches, setIsLoadingBranches] = useState(true);
  const [isLoadingRoomTypes, setIsLoadingRoomTypes] = useState(true);
  const [selectedBranchId, setSelectedBranchId] = useState<number | null>(null);

  useEffect(() => {
    const fetchBranches = async () => {
      try {
        const response = await branchService.getAll();
        setBranches(response.items);
      } catch (error) {
        console.error('Failed to fetch branches:', error);
      } finally {
        setIsLoadingBranches(false);
      }
    };

    const fetchRoomTypes = async () => {
      try {
        const response = await roomTypeService.getAll();
        setRoomTypes(response.data);
      } catch (error) {
        console.error('Failed to fetch room types:', error);
      } finally {
        setIsLoadingRoomTypes(false);
      }
    };

    fetchBranches();
    fetchRoomTypes();
  }, []);

  // Set initial selected branch from default values
  useEffect(() => {
    if (defaultValues?.branchId) {
      setSelectedBranchId(defaultValues.branchId);
    }
  }, [defaultValues?.branchId]);

  // Filter room types based on selected branch
  const filteredRoomTypes = selectedBranchId
    ? roomTypes.filter(rt => rt.branchId === selectedBranchId)
    : roomTypes;

  const form = useForm<RoomFormValues>({
    resolver: zodResolver(roomSchema),
    defaultValues: {
      branchId: 0,
      roomTypeId: 0,
      roomNumber: '',
      status: 0,
      floor: 0,
      manualPrice: 0,
      ...defaultValues,
    },
  });

  const handleSubmit = async (data: RoomFormValues) => {
    try {
      if (mode === 'create') {
        const request: CreateRoomRequest = {
          branchId: data.branchId,
          roomTypeId: data.roomTypeId,
          roomNumber: data.roomNumber,
          status: data.status as RoomStatus,
          floor: data.floor,
          manualPrice: data.manualPrice,
        };
        await onSubmit(request);
        form.reset();
      } else {
        const request: UpdateRoomRequest = {
          roomTypeId: data.roomTypeId,
          roomNumber: data.roomNumber,
          status: data.status as RoomStatus,
          floor: data.floor,
          manualPrice: data.manualPrice,
        };
        await onSubmit(request);
      }
    } catch (error) {
      console.error('Form submission failed:', error);
    }
  };

  const isUpdate = mode === 'update';

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(handleSubmit)} className="space-y-4">
        {mode === 'create' && (
          <FormField
            control={form.control}
            name="branchId"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Branch <span className="text-destructive">*</span></FormLabel>
                <Select
                  onValueChange={(value) => {
                    const branchId = parseInt(value);
                    field.onChange(branchId);
                    setSelectedBranchId(branchId);
                    // Reset room type when branch changes
                    form.setValue('roomTypeId', 0);
                  }}
                  value={field.value ? String(field.value) : ''}
                  disabled={isLoadingBranches}
                >
                  <FormControl>
                    <SelectTrigger className="w-full">
                      <SelectValue placeholder={isLoadingBranches ? "Loading branches..." : "Select a branch"} />
                    </SelectTrigger>
                  </FormControl>
                  <SelectContent>
                    {branches.map((branch) => (
                      <SelectItem key={branch.id} value={String(branch.id)}>
                        {branch.name} - {branch.location}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
                <FormMessage />
              </FormItem>
            )}
          />
        )}

        <FormField
          control={form.control}
          name="roomTypeId"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Room Type <span className="text-destructive">*</span></FormLabel>
              <Select
                onValueChange={(value) => field.onChange(parseInt(value))}
                value={field.value ? String(field.value) : ''}
                disabled={isLoadingRoomTypes || (mode === 'create' && !selectedBranchId)}
              >
                <FormControl>
                  <SelectTrigger className="w-full">
                    <SelectValue 
                      placeholder={
                        isLoadingRoomTypes 
                          ? "Loading room types..." 
                          : mode === 'create' && !selectedBranchId
                          ? "Select a branch first"
                          : filteredRoomTypes.length === 0
                          ? "No room types available for this branch"
                          : "Select a room type"
                      } 
                    />
                  </SelectTrigger>
                </FormControl>
                <SelectContent>
                  {filteredRoomTypes.map((roomType) => (
                    <SelectItem key={roomType.id} value={String(roomType.id)}>
                      {roomType.name} - Capacity: {roomType.capacity}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
              <FormMessage />
            </FormItem>
          )}
        />

        <div className="grid grid-cols-2 gap-4">
          <FormField
            control={form.control}
            name="roomNumber"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Room Number <span className="text-destructive">*</span></FormLabel>
                <FormControl>
                  <Input placeholder="Enter room number" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="floor"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Floor <span className="text-destructive">*</span></FormLabel>
                <FormControl>
                  <Input
                    type="number"
                    placeholder="Enter floor number"
                    {...field}
                    onChange={(e) => field.onChange(parseInt(e.target.value) || 0)}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
        </div>

        <FormField
          control={form.control}
          name="status"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Status <span className="text-destructive">*</span></FormLabel>
              <Select
                onValueChange={(value) => field.onChange(parseInt(value))}
                value={String(field.value)}
              >
                <FormControl>
                  <SelectTrigger className="w-full">
                    <SelectValue placeholder="Select room status" />
                  </SelectTrigger>
                </FormControl>
                <SelectContent>
                  {roomStatusOptions.map((status) => (
                    <SelectItem key={status.id} value={String(status.id)}>
                      {status.name}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="manualPrice"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Manual Price <span className="text-destructive">*</span></FormLabel>
              <FormControl>
                <Input
                  type="number"
                  step="0.01"
                  placeholder="Enter manual price"
                  {...field}
                  onChange={(e) => field.onChange(e.target.value ? parseFloat(e.target.value) : 0)}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <Button
          type="submit"
          className="w-full"
          disabled={isLoading || isLoadingBranches || isLoadingRoomTypes}
        >
          {isLoading
            ? (isUpdate ? "Saving Changes..." : "Creating Room...")
            : (isUpdate ? "Save Changes" : "Create Room")
          }
        </Button>
      </form>
    </Form>
  );
};