import { usePrisma } from "~/server/utils/prisma";

export default defineEventHandler(async (event) => {
  const prisma = usePrisma();

  const params = getRouterParams(event);
  const id = Number(params.id);

  const roomData = await prisma.room.findUnique({
    where: {
      id: id,
    },
    include: {
      equipment: true,
    },
  });

  if (!roomData) {
    setResponseStatus(event, 404, "Not Found");
    return {
      message: "Room not found",
    };
  }

  const { equipment, ...room } = roomData;

  return {
    room,
    equipment,
  };
});
