import { z } from "zod";

const roomSchema = z.object({
  branchId: z.number({
    required_error: "Please select a branch",
    invalid_type_error: "Branch must be a number"
  }).positive("Invalid branch selection. Please choose a valid branch"),
  
  roomTypeId: z.number({
    required_error: "Please select a room type",
    invalid_type_error: "Room type must be a number"
  }).positive("Invalid room type selection. Please choose a valid room type"),
  
  roomNumber: z.string()
    .min(1, "Room number is a required field")
    .max(20, "Room number must be less than 20 digits")
    .refine((val) => /^\d+$/.test(val), {
      message: "Room number can only contain numeric digits (0-9)"
    })
    .refine((val) => parseInt(val) > 0, {
      message: "Room number must be a positive number greater than zero"
    }),
  
  status: z.enum(['Available', 'Occupied', 'UnderMaintenance'], {
    required_error: "Please select a room status",
    invalid_type_error: "Status must be a valid room status"
  }),
  
  floor: z.number({
    required_error: "Floor number is required",
    invalid_type_error: "Floor must be a number"
  })
    .min(0, "Floor number cannot be negative")
    .max(100, "Floor number cannot exceed 100"),
  
  manualPrice: z.number({
    required_error: "Manual price is required",
    invalid_type_error: "Price must be a number"
  })
    .min(0.01, "Manual price must be greater than 0")
    .max(10000, "Manual price cannot exceed 10,000")
});

export type RoomFormValues = z.infer<typeof roomSchema>;

export { roomSchema };
