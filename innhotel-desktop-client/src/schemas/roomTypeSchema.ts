import { z } from 'zod';

export const roomTypeSchema = z.object({
  branchId: z.number({
    required_error: 'Branch is required',
    invalid_type_error: 'Branch must be a number'
  }).positive('Branch ID must be positive'),
  
  name: z.string({
    required_error: 'Room type name is required',
    invalid_type_error: 'Room type name must be a string'
  })
    .min(2, 'Room type name must be at least 2 characters')
    .max(100, 'Room type name must not exceed 100 characters')
    .trim(),
  
  capacity: z.number({
    required_error: 'Capacity is required',
    invalid_type_error: 'Capacity must be a number'
  })
    .int('Capacity must be a whole number')
    .positive('Capacity must be at least 1')
    .max(20, 'Capacity cannot exceed 20 people'),
  
  basePrice: z.number({
    required_error: 'Base price is required',
    invalid_type_error: 'Base price must be a number'
  })
    .positive('Base price must be greater than 0'),
  
  description: z.string()
    .max(500, 'Description must not exceed 500 characters')
    .trim()
    .optional()
    .or(z.literal(''))
});

export const updateRoomTypeSchema = roomTypeSchema.extend({
  id: z.number({
    required_error: 'Room type ID is required',
    invalid_type_error: 'Room type ID must be a number'
  }).positive('Room type ID must be positive')
});

export type RoomTypeFormData = z.infer<typeof roomTypeSchema>;
export type UpdateRoomTypeFormData = z.infer<typeof updateRoomTypeSchema>;