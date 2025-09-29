import { usePrisma } from "~/server/utils/prisma";

export default defineEventHandler(async (event) => {
  const prisma = usePrisma();

  const data = await readBody(event);
  const { roomId, status } = data;

  const room = await prisma.room.findUnique({
    where: { id: roomId },
  });

  let updatedRoom;
  if (room) {
    updatedRoom = await prisma.room.update({
      where: { id: roomId },
      data: {
        status: status,
      },
    });
  }

  setResponseStatus(event, 201, "Edited");
  return {
    message: "Successful edit",
    roomdata: {
      updatedRoom,
    },
  };
});
