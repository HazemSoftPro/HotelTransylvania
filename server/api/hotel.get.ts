import { PrismaClient } from "@prisma/client";
const prisma = new PrismaClient();

export default defineEventHandler(async (event) => {
  const hotel = await prisma.hotel.findFirst({
    where: {
      id: 1,
    },
  });

  if (!hotel) {
    setResponseStatus(event, 201, "No Content");
  } else {
    return {
      body: hotel,
    };
  }
});
