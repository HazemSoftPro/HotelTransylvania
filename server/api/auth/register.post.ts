import { usePrisma } from "~/server/utils/prisma";
import { hashPassword } from "~~/server/utils/password";

export default defineEventHandler(async (event) => {
  try {
    const prisma = usePrisma();
    const { email, username, password } = await readBody(event);

    const user = await prisma.user.findFirst({
      where: { OR: [{ username }, { email }] },
    });

    if (user != null) {
      prisma.$disconnect();
      setResponseStatus(event, 409, "Conflict");
      return {
        message: "The user already exists",
      };
    } else {
      const { salt, hash } = await hashPassword(password);

      const createUser: any = await prisma.user.create({
        data: {
          email: email,
          username: username,
          password: hash,
          salt: salt,
        },
      });

      delete createUser.password;
      delete createUser.salt;

      await prisma.$disconnect();

      createUser.success = true;

      setResponseStatus(event, 201, "Created");
      return {
        message: "The user was created",
      };
    }
  } catch (error) {
    if (error instanceof Error) {
      throw new Error(`Error: ${error.message}`);
    }
    throw error;
  }
});
