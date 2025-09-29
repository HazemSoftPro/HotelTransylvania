import { usePrisma } from "~/server/utils/prisma";

export default defineEventHandler(async (event) => {
  const prisma = usePrisma();

  const { limit, page, query, field, direction } = getQuery(event);

  if (typeof field !== "string") {
    throw new Error("Field must be a string");
  }

  let guests;
  let totalPages;
  try {
    const orderBy = field && direction ? { [field]: direction } : undefined;
    guests = await prisma.guest.findMany({
      take: Number(limit),
      skip: (Number(page) - 1) * Number(limit),
      where: {
        OR: [
          {
            name: {
              contains: String(query),
            },
          },
          {
            surname: {
              contains: String(query),
            },
          },
          {
            phone: {
              contains: String(query),
            },
          },
          {
            email: {
              contains: String(query),
            },
          },
        ],
      },
      orderBy,
    });

    const totalGuests = await prisma.guest.count({
      where: {
        OR: [
          {
            name: {
              contains: String(query),
            },
          },
          {
            surname: {
              contains: String(query),
            },
          },
          {
            phone: {
              contains: String(query),
            },
          },
          {
            email: {
              contains: String(query),
            },
          },
        ],
      },
    });
    totalPages = Math.ceil(totalGuests / Number(limit));

    if (guests.length === 0) {
      setResponseStatus(event, 201, "No Content");
    }

    return { guests, totalPages };
  } catch (error) {
    if (error instanceof Error) {
      throw new Error(`Error finding guests: ${error.message}`);
    }
    throw error;
  }
});
