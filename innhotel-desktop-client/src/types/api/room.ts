export const RoomStatus = {
  Available: 0,
  Occupied: 1,
  UnderMaintenance: 2
} as const;

export type RoomStatus = typeof RoomStatus[keyof typeof RoomStatus];

export interface UpdateRoomRequest {
  roomTypeId: number;
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