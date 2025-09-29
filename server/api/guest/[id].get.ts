import { usePrisma } from "~/server/utils/prisma";

export default defineEventHandler(async (event) => {
  const prisma = usePrisma();

  const params = getRouterParams(event);
  const id = Number(params.id);

  const guest = await prisma.guest.findUnique({
    where: {
      id: id,
    },
  });

  return guest;
});
