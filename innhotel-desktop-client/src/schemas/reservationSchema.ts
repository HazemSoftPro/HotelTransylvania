import { z } from 'zod';

const baseReservationSchema = z.object({
  guestId: z.number({
    required_error: 'Guest is required',
    invalid_type_error: 'Guest must be selected'
  }).positive('Please select a valid guest'),
  
  branchId: z.number({
    required_error: 'Branch is required',
    invalid_type_error: 'Branch must be selected'
  }).positive('Please select a valid branch'),
  
  checkInDate: z.string({
    required_error: 'Check-in date is required'
  }).refine((date) => {
    const checkIn = new Date(date);
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    return checkIn >= today;
  }, 'Check-in date cannot be in the past'),
  
  checkOutDate: z.string({
    required_error: 'Check-out date is required'
  }),
  
  status: z.enum(['Pending', 'Confirmed', 'Checked In', 'Checked Out'], {
    required_error: 'Status is required',
    invalid_type_error: 'Invalid status selected'
  }),
  
  roomIds: z.array(z.number()).min(1, 'At least one room must be selected'),
  
  serviceIds: z.array(z.number()).optional().default([]),
});

export const reservationSchema = baseReservationSchema.refine((data) => {
  const checkIn = new Date(data.checkInDate);
  const checkOut = new Date(data.checkOutDate);
  return checkOut > checkIn;
}, {
  message: 'Check-out date must be after check-in date',
  path: ['checkOutDate']
}).refine((data) => {
  const checkIn = new Date(data.checkInDate);
  const checkOut = new Date(data.checkOutDate);
  const diffTime = Math.abs(checkOut.getTime() - checkIn.getTime());
  const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  return diffDays <= 365;
}, {
  message: 'Reservation cannot exceed 365 days',
  path: ['checkOutDate']
});

export const updateReservationSchema = baseReservationSchema.extend({
  id: z.number({
    required_error: 'Reservation ID is required',
    invalid_type_error: 'Reservation ID must be a number'
  }).positive('Reservation ID must be positive')
}).refine((data) => {
  const checkIn = new Date(data.checkInDate);
  const checkOut = new Date(data.checkOutDate);
  return checkOut > checkIn;
}, {
  message: 'Check-out date must be after check-in date',
  path: ['checkOutDate']
}).refine((data) => {
  const checkIn = new Date(data.checkInDate);
  const checkOut = new Date(data.checkOutDate);
  const diffTime = Math.abs(checkOut.getTime() - checkIn.getTime());
  const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  return diffDays <= 365;
}, {
  message: 'Reservation cannot exceed 365 days',
  path: ['checkOutDate']
});

export type ReservationFormData = z.infer<typeof reservationSchema>;
export type UpdateReservationFormData = z.infer<typeof updateReservationSchema>;