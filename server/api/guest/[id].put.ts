import { usePrisma } from "~/server/utils/prisma";

export default defineEventHandler(async (event) => {
  const prisma = usePrisma();

  const params = getRouterParams(event);
  const id = Number(params.id);

  const data = await readBody(event);

  const updatedGuest = await prisma.guest.update({
    where: { id: id },
    data: data,
  });

  setResponseStatus(event, 201, "Edited");
  return {
    message: "Successful edit",
    userdata: {
      updatedGuest,
    },
  };
});
