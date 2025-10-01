import pkg from "@prisma/client";
const { PrismaClient } = pkg;

let _prisma: any;

export const usePrisma = () => {
  if (!_prisma) {
    _prisma = new PrismaClient();
  }
  return _prisma;
};
