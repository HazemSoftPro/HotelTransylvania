import { usePrisma } from "~/server/utils/prisma";

export default defineEventHandler(async (event) => {
  const prisma = usePrisma();

  const data = await readBody(event);

  const {
    startDate,
    endDate,
    roomId,
    guestId,
    additions,
    status,
    source,
    price,
  } = data;

  try {
    // {{ $t('first_create_booking') }}
    const newBooking = await prisma.booking.create({
      data: {
        startDate,
        endDate,
        roomId,
        guestId,
        status,
        source,
        price,
      },
    });

    try {
      // {{ $t('then_create_additions') }}
      for (const addition of additions) {
        await prisma.bookingAdditions.create({
          data: {
            additionId: addition.id,
            bookingId: newBooking.id,
            quantity: addition.quantity,
          },
        });
      }

      setResponseStatus(event, 201, "Created");
      return {
        message: "Successful add",
        bookingData: {
          newBooking,
        },
      };
    } catch (additionError) {
      console.error("Failed to create addition:", additionError);
      // {{ $t('if_additions_fail_remove_booking') }}
      await prisma.booking.delete({
        where: { id: newBooking.id },
      });
      setResponseStatus(event, 500, "Internal Server Error");
      return {
        message: "Failed to create addition",
      };
    }
  } catch (bookingError) {
    console.error("Failed to create booking:", bookingError);
    setResponseStatus(event, 500, "Internal Server Error");
    return {
      message: "Failed to create booking",
    };
  }
});
