import bcrypt from "bcryptjs";

export async function comparePasswords(
  inputPassword: string,
  savedPassword: string
): Promise<boolean> {
  const match = await bcrypt.compare(inputPassword, savedPassword);
  return match;
}

export async function hashPassword(
  password: string
): Promise<{ salt: string; hash: string }> {
  const salt = await bcrypt.genSalt();
  const hash = await bcrypt.hash(password, salt);
  return { salt, hash };
}
