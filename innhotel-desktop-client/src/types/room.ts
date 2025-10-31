export interface RoomFormValues {
  branchId: number;
  roomTypeId: number;
  roomNumber: string;
  status: number;
  floor: number;
  manualPrice: number;
}

export const roomStatusOptions = [
  { id: 0, name: "Available" },
  { id: 1, name: "Occupied" },
  { id: 2, name: "Under Maintenance" }
] as const;
