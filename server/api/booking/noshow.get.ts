import { usePrisma } from "~/server/utils/prisma";

export default defineEventHandler(async (event) => {
  const prisma = usePrisma();

  const yesterday = new Date();
  yesterday.setDate(yesterday.getDate() - 1);

  const bookings = await prisma.booking.findMany({
    where: {
      startDate: {
        lte: yesterday,
      },
      status: {
        in: ["PENDING", "CONFIRMED", "GUARANTEED"],
      },
    },
    include: {
      room: true,
    },
  });

  if (bookings.length === 0) {
    setResponseStatus(event, 304, "Not Modified");
    return {
      message: "No bookings found",
    };
  }

  for (const booking of bookings) {
    await prisma.booking.update({
      where: { id: booking.id },
      data: { status: "CANCELED" },
    });

    if (booking.room.status === "ARRIVAL") {
      await prisma.room.update({
        where: { id: booking.room.id },
        data: { status: "CLEAN" },
      });
    }
  }

  setResponseStatus(event, 200, "OK");
  return {
    message: "Successful update",
    bookings: bookings,
  };
});
