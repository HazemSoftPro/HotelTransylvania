export interface Room {
  id: number;
  number: string;
  type: RoomStandard;
  maxGuests: number;
  pricePerNight: number;
  status: RoomStatus;
  equipment: Equipment;
  equipmentId: number;
}

export interface Equipment {
  id: number;
  singleBeds: number;
  doubleBeds: number;
  hasTV: boolean;
  hasWifi: boolean;
  hasBathroom: boolean;
  bathroomType: BathroomType | null;
  hasKitchen: boolean;
  hasFridge: boolean;
  hasBalcony: boolean;
  hasAirConditioning: boolean;
  hasWardrobe: boolean;
  hasHairDryer: boolean;
  hasCoffeeAndTeaSet: boolean;
  hasCosmetics: boolean;
  hasTowels: boolean;
  rooms: { [key: string]: any }[];
}

export enum RoomStatus {
  CLEAN = "CLEAN",
  DIRTY = "DIRTY",
  SERVICE = "SERVICE",
  OCCUPIED = "OCCUPIED",
  ARRIVAL = "ARRIVAL",
}

export enum RoomStandard {
  ECONOMY = "ECONOMY",
  STANDARD = "STANDARD",
  LUXURY = "LUXURY",
  APARTMENT = "APARTMENT",
}

export enum BathroomType {
  SHOWER = "SHOWER",
  BATHTUB = "BATHTUB",
  BOTH = "BOTH",
}
