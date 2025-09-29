import { usePrisma } from "~/server/utils/prisma";

export default defineEventHandler(async (event) => {
  const prisma = usePrisma();

  const params = getRouterParams(event);
  const id = Number(params.id);

  await prisma.guest.delete({
    where: {
      id: id,
    },
  });

  setResponseStatus(event, 202, "Accepted");
  return {
    message: "guest has been removed",
  };
});
