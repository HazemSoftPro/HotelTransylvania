export interface RoomFormValues {
  branchId: string;
  roomTypeId: string;
  roomNumber: string;
  status: string;
  floor: number;
  manualPrice: number;
}

export const roomStatusOptions = [
  { id: "0", name: "Available" },
  { id: "1", name: "Occupied" },
  { id: "2", name: "Under Maintenance" }
] as const;
