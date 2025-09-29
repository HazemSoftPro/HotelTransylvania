import { usePrisma } from "~/server/utils/prisma";

export default defineEventHandler(async (event) => {
  const prisma = usePrisma();

  let additions;

  try {
    additions = await prisma.addition.findMany({});

    if (additions.length === 0) {
      setResponseStatus(event, 201, "No Content");
    }

    return additions;
  } catch (error) {
    console.error("Failed to fetch additions:", error);
  }
});
