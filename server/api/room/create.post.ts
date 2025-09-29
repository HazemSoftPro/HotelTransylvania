import { usePrisma } from "~/server/utils/prisma";

export default defineEventHandler(async (event) => {
  const prisma = usePrisma();

  const data = await readBody(event);

  const { equipment, room } = data;

  try {
    // {{ $t('first_create_room') }}
    const newRoom = await prisma.room.create({
      data: room,
    });

    try {
      // {{ $t('then_create_equipment') }}
      const newEquipment = await prisma.equipment.create({
        data: equipment,
      });

      // {{ $t('finally_update_room_with_equipment') }}
      await prisma.room.update({
        where: { id: newRoom.id },
        data: { equipmentId: newEquipment.id
         },
      });

      setResponseStatus(event, 201, "Created");
      return {
        message: "Successful add",
        roomData: {
          newRoom,
        },
      };
    } catch (equipmentError) {
      console.error("Failed to create equipment:", equipmentError);
      // {{ $t('if_equipment_creation_fails_remove_room') }}
      await prisma.room.delete({
        where: { id: newRoom.id },
      });
      setResponseStatus(event, 500, "Internal Server Error");
      return {
        message: "Failed to create equipment",
      };
    }
  } catch (roomError) {
    console.error("Failed to create room:", roomError);
    setResponseStatus(event, 500, "Internal Server Error");
    return {
      message: "Failed to create room",
    };
  }
});
