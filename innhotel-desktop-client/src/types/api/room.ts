export const RoomStatus = {
  Available: 0,
  Occupied: 1,
  UnderMaintenance: 2
} as const;

export type RoomStatus = typeof RoomStatus[keyof typeof RoomStatus];

export interface CreateRoomRequest {
  roomTypeId: number;
  roomNumber: string;
  status: RoomStatus;
  floor: number;
  priceOverride?: number;
}

export interface UpdateRoomRequest {
  roomTypeId: number;
  roomNumber: string;
  status: RoomStatus;
  floor: number;
  priceOverride?: number;
}

export interface Room {
  id: number;
  branchId: number;
  branchName: string;
  roomTypeId: number;
  roomTypeName: string;
  basePrice: number;
  capacity: number;
  roomNumber: string;
  status: RoomStatus;
  floor: number;
  priceOverride?: number;
}

export interface RoomResponse {
  id: number;
  branchId: number;
  branchName: string;
  roomTypeId: number;
  roomTypeName: string;
  basePrice: number;
  capacity: number;
  roomNumber: string;
  status: RoomStatus;
  floor: number;
  priceOverride?: number;
}

export interface RoomsResponse {
  data: RoomResponse[];
  items: RoomResponse[];
  total: number;
  totalCount: number;
  page: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface CreateRoomResponse {
  id: number;
  message: string;
}

export interface UpdateRoomResponse {
  id: number;
  message: string;
  data: RoomResponse;
}
