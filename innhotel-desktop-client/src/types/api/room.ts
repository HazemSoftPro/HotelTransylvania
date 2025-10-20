export const RoomStatus = {
  Available: 0,
  Occupied: 1,
  UnderMaintenance: 2
} as const;

export type RoomStatus = typeof RoomStatus[keyof typeof RoomStatus];

export interface CreateRoomRequest {
  branchId: number;
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
  status: number;
  message: string;
  data: Room[];
  items: Room[];
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface CreateRoomResponse {
  status: number;
  message: string;
  data: Room;
}

export interface UpdateRoomResponse {
  status: number;
  message: string;
  data: Room;
}