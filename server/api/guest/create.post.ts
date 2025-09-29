import { usePrisma } from "~/server/utils/prisma";

export default defineEventHandler(async (event) => {
  const prisma = usePrisma();

  const data = await readBody(event);

  const newGuest = await prisma.guest.create({
    data: data,
  });

  setResponseStatus(event, 201, "Created");
  return {
    message: "Successful add",
    userdata: {
      newGuest,
    },
  };
});
