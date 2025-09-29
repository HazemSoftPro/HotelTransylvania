import { usePrisma } from "~/server/utils/prisma";

export default defineEventHandler(async (event) => {
  const prisma = usePrisma();

  const params = getRouterParams(event);
  const id = Number(params.id);

  const data = await readBody(event);

  const updatedBooking = await prisma.booking.update({
    where: { id: id },
    data: {
      status: data.status,
    },
  });

  const updatedRoom = await prisma.room.updateMany({
    where: { id: updatedBooking.roomId },
    data: {
      status: data.room.status,
    },
  });

  setResponseStatus(event, 201, "Edited");
  return {
    message: "Successful edit",
    bookingdata: {
      updatedBooking,
    },
    roomdata: {
      updatedRoom,
    },
  };
});
