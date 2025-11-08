import type { RoomStatus } from './api/room';

export interface RoomFormValues {
  branchId: number;
  roomTypeId: number;
  roomNumber: string;
  status: RoomStatus;
  floor: number;
  manualPrice: number;
}

export const roomStatusOptions = [
  { value: 'Available' as const, label: 'Available' },
  { value: 'Occupied' as const, label: 'Occupied' },
  { value: 'UnderMaintenance' as const, label: 'Under Maintenance' }
] as const;
