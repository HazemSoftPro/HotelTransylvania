import type { Pagination, UpdateResponse } from './global';

// Room in a reservation (for creation)
export interface ReservationRoom {
  roomId: number;
  pricePerNight: number;
}

// Room in a reservation response
export interface ReservationRoomResponse {
  roomId: number;
  roomNumber: string;
  roomTypeName: string;
  pricePerNight: number;
}

// Service in a reservation (for creation)
export interface ReservationService {
  serviceId: number;
  quantity: number;
  unitPrice: number;
}

// Service in a reservation response
export interface ReservationServiceResponse {
  serviceId: number;
  serviceName: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

// Reservation interface for creation
export interface Reservation {
  id: number;
  guestId: number;
  checkInDate: string; // DateOnly format: YYYY-MM-DD
  checkOutDate: string; // DateOnly format: YYYY-MM-DD
  rooms: ReservationRoom[];
  services: ReservationService[];
  status?: 'Pending' | 'Confirmed' | 'Cancelled' | 'Completed';
}

// Response of successful reservation creation and getById
export interface ReservationResponse {
  id: number;
  guestId: number;
  guestName: string;
  branchId?: number;
  branchName: string;
  checkInDate: string;
  checkOutDate: string;
  reservationDate: string;
  status: 'Pending' | 'Confirmed' | 'CheckedIn' | 'CheckedOut' | 'Cancelled' | number;
  totalCost: number;
  rooms: ReservationRoomResponse[];
  services: ReservationServiceResponse[];
}

// Response of successful get all reservations
export interface ReservationsResponse extends Pagination {
  items: ReservationResponse[];
}

// Response of successful reservation update
export type UpdateReservationResponse = UpdateResponse<ReservationResponse>;