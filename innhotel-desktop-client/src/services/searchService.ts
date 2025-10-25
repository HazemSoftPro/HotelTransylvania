import api from '@/lib/axios';
import type { RoomResponse } from '@/types/api/room';
import type { ReservationResponse } from '@/types/api/reservation';
import type { GuestResponse } from '@/types/api/guest';
import type { EmployeeResponse } from '@/types/api/employee';
import type { RoomType } from '@/services/roomTypeService';
import type { HotelService } from '@/services/serviceService';

/**
 * Search Service
 * 
 * Provides search functionality for all entities with caching and error handling.
 */

export interface SearchParams {
  searchTerm?: string;
  pageNumber?: number;
  pageSize?: number;
  [key: string]: unknown;
}

export interface SearchResponse<T> {
  status: number;
  message: string;
  data: T[];
  pagination: {
    pageNumber: number;
    pageSize: number;
    totalResults: number;
  };
}

/**
 * Search rooms with filters
 */
export const searchRooms = async (params: SearchParams & {
  branchId?: number;
  roomTypeId?: number;
  status?: string;
  floor?: number;
}): Promise<SearchResponse<RoomResponse>> => {
  const response = await api.get('/rooms/search', { params });
  return response.data;
};

/**
 * Search reservations with filters
 */
export const searchReservations = async (params: SearchParams & {
  guestId?: number;
  status?: string;
  checkInDateFrom?: string;
  checkInDateTo?: string;
  checkOutDateFrom?: string;
  checkOutDateTo?: string;
}): Promise<SearchResponse<ReservationResponse>> => {
  const response = await api.get('/reservations/search', { params });
  return response.data;
};

/**
 * Search guests with filters
 */
export const searchGuests = async (params: SearchParams): Promise<SearchResponse<GuestResponse>> => {
  const response = await api.get('/guests/search', { params });
  return response.data;
};

/**
 * Search employees with filters
 */
export const searchEmployees = async (params: SearchParams & {
  branchId?: number;
  position?: string;
  hireDateFrom?: string;
  hireDateTo?: string;
}): Promise<SearchResponse<EmployeeResponse>> => {
  const response = await api.get('/employees/search', { params });
  return response.data;
};

/**
 * Search room types with filters
 */
export const searchRoomTypes = async (params: SearchParams & {
  branchId?: number;
  minCapacity?: number;
  maxCapacity?: number;
}): Promise<SearchResponse<RoomType>> => {
  const response = await api.get('/roomtypes/search', { params });
  return response.data;
};

/**
 * Search services with filters
 */
export const searchServices = async (params: SearchParams & {
  branchId?: number;
  minPrice?: number;
  maxPrice?: number;
}): Promise<SearchResponse<HotelService>> => {
  const response = await api.get('/services/search', { params });
  return response.data;
};

/**
 * Generic search function
 */
export const search = async <T>(
  endpoint: string,
  params: SearchParams
): Promise<SearchResponse<T>> => {
  const response = await api.get(endpoint, { params });
  return response.data;
};

export default {
  searchRooms,
  searchReservations,
  searchGuests,
  searchEmployees,
  searchRoomTypes,
  searchServices,
  search,
};