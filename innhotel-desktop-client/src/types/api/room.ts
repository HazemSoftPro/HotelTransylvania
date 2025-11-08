// RoomStatus as string enum to match API
export type RoomStatus = 'Available' | 'Occupied' | 'UnderMaintenance';

// Request type for creating a new room
export interface CreateRoomRequest {
  branchId: number;
  roomTypeId: number;
  roomNumber: string;
  status: RoomStatus;
  floor: number;
  manualPrice: number;
}

// Request type for updating a room
export interface UpdateRoomRequest {
  roomTypeId: number;
  roomNumber: string;
  status: RoomStatus;
  floor: number;
  manualPrice: number;
}

// Single room response
export interface RoomResponse {
  id: number;
  branchId: number;
  branchName: string;
  roomTypeId: number;
  roomTypeName: string;
  manualPrice: number;
  capacity: number;
  roomNumber: string;
  status: RoomStatus;
  floor: number;
}

// List of rooms response
export interface RoomsResponse {
  items: RoomResponse[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

// Room type definition (same as RoomResponse for store compatibility)
export type Room = RoomResponse;

// Create room response
export type CreateRoomResponse = RoomResponse;

// Update room response
export type UpdateRoomResponse = RoomResponse;
