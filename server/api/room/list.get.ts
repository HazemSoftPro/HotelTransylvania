import { usePrisma } from "~/server/utils/prisma";

type RoomStandard = "ECONOMY" | "STANDARD" | "LUXURY" | "APARTMENT";

export default defineEventHandler(async (event) => {
  const prisma = usePrisma();

  const { number, type } = getQuery(event);

  let standards: RoomStandard[];

  if (typeof type === "string") {
    standards = type.split(",") as RoomStandard[];
  } else {
    standards = ["ECONOMY", "STANDARD", "LUXURY", "APARTMENT"];
  }

  let whereClause = {};

  if (number) {
    whereClause = {
      ...whereClause,
      number: {
        contains: String(number),
      },
    };
  }

  if (standards) {
    whereClause = {
      ...whereClause,
      type: {
        in: standards,
      },
    };
  }

  let rooms;
  try {
    rooms = await prisma.room.findMany({
      where: whereClause,
      include: {
        equipment: true,
      },
    });

    if (rooms.length === 0) {
      setResponseStatus(event, 201, "No Content");
    }

    rooms.sort((a, b) => Number(a.number) - Number(b.number));

    return { rooms: rooms };
  } catch (error) {
    console.error("Failed to fetch rooms:", error);
  }
});
