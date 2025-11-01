import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "@/components/ui/button";
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
import type { GuestReq } from "@/types/api/guest";
import { guestSchema, type GuestFormValues } from "@/schemas/guestSchema";

interface GuestFormProps {
  onSubmit: (data: GuestReq) => void;
  defaultValues?: Partial<GuestFormValues>;
  isLoading?: boolean;
  mode?: 'create' | 'update';
}

const ID_PROOF_TYPES = [
  { value: 'Passport', label: 'Passport' },
  { value: 'DriverLicense', label: "Driver's License" },
  { value: 'NationalId', label: 'National ID' }
] as const;

const GENDER_OPTIONS = [
  { value: 'Male', label: 'Male' },
  { value: 'Female', label: 'Female' }
] as const;

export const GuestForm = ({
  onSubmit,
  defaultValues,
  isLoading = false,
  mode = 'create'
}: GuestFormProps) => {
  const form = useForm<GuestFormValues>({
    resolver: zodResolver(guestSchema),
    defaultValues: {
      firstName: "",
      lastName: "",
      gender: "Male",
      idProofType: "Passport",
      idProofNumber: "",
      email: "",
      phone: "",
      address: "",
      ...defaultValues
    },
  });

  const handleSubmit = async (data: GuestFormValues) => {
    try {
      const guestData: GuestReq = {
        firstName: data.firstName,
        lastName: data.lastName,
        gender: data.gender,
        idProofType: data.idProofType,
        idProofNumber: data.idProofNumber,
        email: data.email || "",
        phone: data.phone || "",
        address: data.address || ""
      };

      await onSubmit(guestData);
      if (mode === 'create') {
        form.reset();
      }
    } catch (error) {
      console.error("GuestForm submission failed:", error);
    }
  };

  const isUpdate = mode === 'update';

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(handleSubmit)} className="space-y-4">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <FormField
            control={form.control}
            name="firstName"
            render={({ field }) => (
              <FormItem>
                <FormLabel>First Name <span className="text-destructive">*</span></FormLabel>
                <FormControl>
                  <Input placeholder="Enter first name" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="lastName"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Last Name <span className="text-destructive">*</span></FormLabel>
                <FormControl>
                  <Input placeholder="Enter last name" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <FormField
            control={form.control}
            name="gender"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Gender <span className="text-destructive">*</span></FormLabel>
                <Select onValueChange={field.onChange} value={field.value}>
                  <FormControl>
                    <SelectTrigger className="w-full">
                      <SelectValue placeholder="Select gender" />
                    </SelectTrigger>
                  </FormControl>
                  <SelectContent>
                    {GENDER_OPTIONS.map((option) => (
                      <SelectItem key={option.value} value={option.value}>
                        {option.label}
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
            name="idProofType"
            render={({ field }) => (
              <FormItem>
                <FormLabel>ID Proof Type <span className="text-destructive">*</span></FormLabel>
                <Select onValueChange={field.onChange} value={field.value}>
                  <FormControl>
                    <SelectTrigger className="w-full">
                      <SelectValue placeholder="Select ID proof type" />
                    </SelectTrigger>
                  </FormControl>
                  <SelectContent>
                    {ID_PROOF_TYPES.map((option) => (
                      <SelectItem key={option.value} value={option.value}>
                        {option.label}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
                <FormMessage />
              </FormItem>
            )}
          />
        </div>

        <FormField
          control={form.control}
          name="idProofNumber"
          render={({ field }) => (
            <FormItem>
              <FormLabel>ID Proof Number <span className="text-destructive">*</span></FormLabel>
              <FormControl>
                <Input placeholder="Enter ID proof number" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="email"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Email</FormLabel>
              <FormControl>
                <Input type="email" placeholder="Enter email address" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="phone"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Phone</FormLabel>
              <FormControl>
                <Input placeholder="Enter phone number" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="address"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Address</FormLabel>
              <FormControl>
                <Input placeholder="Enter address" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <Button
          type="submit"
          className="w-full"
          disabled={isLoading}
        >
          {isLoading
            ? (isUpdate ? "Updating Guest..." : "Creating Guest...")
            : (isUpdate ? "Update Guest" : "Create Guest")
          }
        </Button>
      </form>
    </Form>
  );
};