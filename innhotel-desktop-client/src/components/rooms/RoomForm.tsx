import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import type { CreateRoomRequest, UpdateRoomRequest, RoomStatus } from "@/types/api/room";
import { useState, useEffect } from "react";
import { branchService } from "@/services/branchService";
import { roomTypeService } from "@/services/roomTypeService";
import { roomSchema } from "@/schemas/roomSchema";
import type { RoomFormValues } from "@/types/room";
import { roomStatusOptions } from "@/types/room";

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

interface Branch {
  id: number;
  name: string;
}

interface RoomType {
  id: number;
  name: string;
  branchId: number;
}

export const RoomForm = ({ 
  onSubmit, 
  defaultValues,
  isLoading = false,
  mode 
}: RoomFormProps) => {
  const [branches, setBranches] = useState<Branch[]>([]);
  const [roomTypes, setRoomTypes] = useState<RoomType[]>([]);
  const [isLoadingBranches, setIsLoadingBranches] = useState(false);
  const [isLoadingRoomTypes, setIsLoadingRoomTypes] = useState(false);

  const form = useForm<RoomFormValues>({
    resolver: zodResolver(roomSchema),
    defaultValues: {
      room_number: "",
      room_type_id: "",
      status: "0",
      floor: 1,
      branch_id: "",
      ...defaultValues
    }
  });

  const selectedBranchId = form.watch("branch_id");

  useEffect(() => {
    const fetchBranches = async () => {
      try {
        setIsLoadingBranches(true);
        const response = await branchService.getAll();
        setBranches(response.items.map(branch => ({
          id: branch.id,
          name: branch.name
        })));
      } catch (error) {
        console.error('Failed to fetch branches:', error);
      } finally {
        setIsLoadingBranches(false);
      }
    };

    if (mode === 'create') {
      fetchBranches();
    }
  }, [mode]);

  useEffect(() => {
    const fetchRoomTypes = async () => {
      if (!selectedBranchId) {
        setRoomTypes([]);
        return;
      }

      try {
        setIsLoadingRoomTypes(true);
        const response = await roomTypeService.getAll();
        const filteredRoomTypes = response.data
          .filter((rt: any) => rt.branchId === parseInt(selectedBranchId))
          .map((rt: any) => ({
            id: rt.id,
            name: rt.name,
            branchId: rt.branchId
          }));
        setRoomTypes(filteredRoomTypes);
        
        // Reset room type selection when branch changes
        form.setValue("room_type_id", "");
      } catch (error) {
        console.error('Failed to fetch room types:', error);
        setRoomTypes([]);
      } finally {
        setIsLoadingRoomTypes(false);
      }
    };

    if (mode === 'create') {
      fetchRoomTypes();
    }
  }, [selectedBranchId, mode, form]);

  const handleSubmit = (values: RoomFormValues) => {
    if (mode === 'create') {
      const createRequest: CreateRoomRequest = {
        branchId: parseInt(values.branch_id),
        roomTypeId: parseInt(values.room_type_id),
        roomNumber: values.room_number,
        status: parseInt(values.status) as RoomStatus,
        floor: values.floor
      };
      onSubmit(createRequest);
    } else {
      const updateRequest: UpdateRoomRequest = {
        roomTypeId: parseInt(values.room_type_id),
        roomNumber: values.room_number,
        status: parseInt(values.status) as RoomStatus,
        floor: values.floor
      };
      onSubmit(updateRequest);
    }
  };

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(handleSubmit)} className="space-y-6">
        {mode === 'create' && (
          <FormField
            control={form.control}
            name="branch_id"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Branch <span className="text-destructive">*</span></FormLabel>
                <Select onValueChange={field.onChange} value={field.value}>
                  <FormControl>
                    <SelectTrigger className="w-full">
                      <SelectValue placeholder="Select branch" />
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
                <FormMessage />
              </FormItem>
            )}
          />
        )}

        <FormField
          control={form.control}
          name="room_number"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Room Number <span className="text-destructive">*</span></FormLabel>
              <FormControl>
                <Input 
                  type="text"
                  placeholder="e.g. 101" 
                  {...field} 
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="room_type_id"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Room Type <span className="text-destructive">*</span></FormLabel>
              <Select 
                onValueChange={field.onChange} 
                value={field.value}
                disabled={mode === 'create' && (!selectedBranchId || isLoadingRoomTypes)}
              >
                <FormControl>
                  <SelectTrigger className="w-full">
                    <SelectValue placeholder={
                      mode === 'create' && !selectedBranchId 
                        ? "Select branch first" 
                        : isLoadingRoomTypes 
                        ? "Loading room types..." 
                        : roomTypes.length === 0 
                        ? "No room types available for this branch"
                        : "Select room type"
                    } />
                  </SelectTrigger>
                </FormControl>
                <SelectContent>
                  {roomTypes.map((type) => (
                    <SelectItem key={type.id} value={type.id.toString()}>
                      {type.name}
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
          name="status"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Status <span className="text-destructive">*</span></FormLabel>
              <Select onValueChange={field.onChange} value={field.value}>
                <FormControl>
                  <SelectTrigger className="w-full">
                    <SelectValue placeholder="Select status" />
                  </SelectTrigger>
                </FormControl>
                <SelectContent>
                  {roomStatusOptions.map((status) => (
                    <SelectItem key={status.id} value={status.id}>
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
          name="floor"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Floor <span className="text-destructive">*</span></FormLabel>
              <FormControl>
                <Input 
                  type="number" 
                  min="0"
                  max="100"
                  placeholder="Floor number" 
                  {...field}
                  onChange={(e) => field.onChange(parseInt(e.target.value))}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <Button type="submit" disabled={isLoading || isLoadingBranches || isLoadingRoomTypes}>
          {isLoading 
            ? (mode === 'update' ? "Saving Changes..." : "Creating Room...") 
            : (mode === 'update' ? "Save Changes" : "Create Room")
          }
        </Button>
      </form>
    </Form>
  );
};
