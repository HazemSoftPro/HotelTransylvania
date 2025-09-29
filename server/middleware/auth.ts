export default defineEventHandler(async (event) => {
  const authorization = getRequestHeader(event, "Authorization");
  const token =
    authorization && authorization.startsWith("Bearer ")
      ? authorization.slice(7)
      : null;

  const protectedPaths = ["/api/hotel"];

  if (
    protectedPaths.some((path) => getRequestURL(event).pathname.includes(path))
  ) {
    if (
      [
        !token,
        token ? verifyRefreshToken(token).valid : true,
        token ? !getUserIdFromToken(token) : true,
        token ? !verifyAccessToken(token).valid : true,
      ].some((condition) => condition)
    ) {
      throw createError({
        statusCode: 401,
        message: "Unauthorized",
      });
    }
  }
});
