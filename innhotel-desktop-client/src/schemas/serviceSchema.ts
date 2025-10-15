import { z } from 'zod';

export const serviceSchema = z.object({
  branchId: z.number({
    required_error: 'Branch is required',
    invalid_type_error: 'Branch must be a number'
  }).positive('Branch ID must be positive'),
  
  name: z.string({
    required_error: 'Service name is required',
    invalid_type_error: 'Service name must be a string'
  })
    .min(2, 'Service name must be at least 2 characters')
    .max(100, 'Service name must not exceed 100 characters')
    .trim(),
  
  price: z.number({
    required_error: 'Price is required',
    invalid_type_error: 'Price must be a number'
  })
    .positive('Price must be greater than 0')
    .max(10000, 'Price cannot exceed 10,000')
    .multipleOf(0.01, 'Price must have at most 2 decimal places'),
  
  description: z.string()
    .max(500, 'Description must not exceed 500 characters')
    .trim()
    .optional()
    .or(z.literal(''))
});

export const updateServiceSchema = serviceSchema.extend({
  id: z.number({
    required_error: 'Service ID is required',
    invalid_type_error: 'Service ID must be a number'
  }).positive('Service ID must be positive')
});

export type ServiceFormData = z.infer<typeof serviceSchema>;
export type UpdateServiceFormData = z.infer<typeof updateServiceSchema>;