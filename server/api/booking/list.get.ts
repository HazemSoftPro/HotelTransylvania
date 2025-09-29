import { usePrisma } from "~/server/utils/prisma";

type BookingStatus =
  | "PENDING"
  | "CONFIRMED"
  | "GUARANTEED"
  | "CANCELED"
  | "CHECKED_IN"
  | "CHECKED_OUT";

interface SearchCondition {
  guest?: {
    name?: { contains: string; mode: string };
    surname?: { contains: string; mode: string };
    email?: { contains: string; mode: string };
    phone?: { contains: string; mode: string };
  };
  id?: number;
}

export default defineEventHandler(async (event) => {
  const prisma = usePrisma();
  const { limit, page, query, startDate, endDate, status } = getQuery(event);

  let statuses: BookingStatus[];

  if (typeof status === "string") {
    statuses = status.split(",") as BookingStatus[];
  } else {
    statuses = [
      "PENDING",
      "CONFIRMED",
      "GUARANTEED",
      "CANCELED",
      "CHECKED_IN",
      "CHECKED_OUT",
    ];
  }

  const where: any = {
    status: {
      in: statuses,
    },
  };

  if (typeof query === "string" && query.trim() !== "") {
    const searchConditions: SearchCondition[] = [
      { guest: { name: { contains: query, mode: "insensitive" } } },
      { guest: { surname: { contains: query, mode: "insensitive" } } },
      { guest: { email: { contains: query, mode: "insensitive" } } },
      { guest: { phone: { contains: query, mode: "insensitive" } } },
    ];

    // Sprawdź, czy zapytanie może być przekształcone na liczbę
    const queryAsNumber = Number(query);
    if (!isNaN(queryAsNumber)) {
      // Dodaj warunek wyszukiwania po id
      searchConditions.push({ id: queryAsNumber });
    }
    where.OR = searchConditions;
  }

  // Warunki dla dat startu i końca rezerwacji
  if (typeof startDate === "string" && startDate.trim() !== "") {
    where.startDate = {
      ...where.startDate,
      gte: new Date(startDate),
    };
  }

  if (typeof endDate === "string" && endDate.trim() !== "") {
    where.startDate = {
      ...where.startDate,
      lte: new Date(endDate),
    };
  }

  interface FindManyOptions {
    include?: {
      guest?: boolean;
      room?: boolean;
      additions?: {
        include?: {
          addition?: boolean;
        };
      };
    };
    where?: any;
    take?: number;
    skip?: number;
    orderBy?: {
      startDate: "asc" | "desc";
    };
  }

  let findManyArgs: FindManyOptions = {
    include: {
      guest: true,
      room: true,
      additions: {
        include: {
          addition: true,
        },
      },
    },
    where,
    orderBy: {
      startDate: "asc", // Sortuj od najwcześniejszej do najpóźniejszej daty
    },
  };

  if (limit) {
    findManyArgs = {
      ...findManyArgs,
      take: Number(limit),
      skip: page ? (Number(page) - 1) * Number(limit) : 0,
    };
  }

  try {
    const bookings = await prisma.booking.findMany(findManyArgs);

    const totalBookings = await prisma.booking.count({
      where,
    });
    const totalPages = Math.ceil(totalBookings / Number(limit));

    if (bookings.length === 0) {
      setResponseStatus(event, 204);
    }

    return { bookings, totalPages, count: bookings.length };
  } catch (error) {
    if (error instanceof Error) {
      throw new Error(`Error finding bookings: ${error.message}`);
    }
    throw error;
  }
});
