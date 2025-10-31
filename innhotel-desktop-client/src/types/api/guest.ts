import type { Pagination, UpdateResponse } from './global';

// Gender and IdProofType as strings to match API expectations
export type Gender = 'Male' | 'Female';
export type IdProofType = 'Passport' | 'DriverLicense' | 'NationalId';

// Guest interface for creation req and update
export interface Guest {
  id: number;
  firstName: string;
  lastName: string;
  gender: Gender;
  idProofType: IdProofType;
  idProofNumber: string;
  email?: string;
  phone?: string;
  address?: string;
}

// Guest creation data (without id)
export type GuestCreateData = Omit<Guest, 'id'>;

// res of successful Guest creation and getById
export interface GuestResponse {
  id: number;
  firstName: string;
  lastName: string;
  gender: Gender;
  idProofType: IdProofType;
  idProofNumber: string;
  email?: string;
  phone?: string;
  address?: string;
}

// res of successful get all guests
export interface GuestsResponse extends Pagination {
  items: GuestResponse[];
}

// res of successful guest update
export type UpdateGuestResponse = UpdateResponse<GuestResponse>;

// Request interface for creating/updating guests (same as Guest without id)
export interface GuestReq {
  firstName: string;
  lastName: string;
  gender: Gender;
  idProofType: IdProofType;
  idProofNumber: string;
  email?: string;
  phone?: string;
  address?: string;
}