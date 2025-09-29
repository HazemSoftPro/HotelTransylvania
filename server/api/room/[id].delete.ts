import { usePrisma } from "~/server/utils/prisma";

export default defineEventHandler(async (event) => {
  const prisma = usePrisma();

  const params = getRouterParams(event);
  const id = Number(params.id);

  const room = await prisma.room.findUnique({
    where: {
      id: id,
    },
  });

  if (room) {
    // {{ $t('first_remove_room') }}
    await prisma.room.delete({
      where: {
        id: id,
      },
    });

    // {{ $t('then_remove_equipment') }}
    if (room.equipmentId !== null) {
      await prisma.equipment.delete({
        where: {
          id: room.equipmentId,
        },
      });
    }
  }

  setResponseStatus(event, 202, "Accepted");
  return {
    message: "room and associated equipment have been removed",
  };
});
