import {
  verifyAccessToken,
  verifyRefreshToken,
  createAccessToken,
  getUserIdFromToken,
  getStoreRefreshToken,
  getUserData,
} from "~/server/utils/session";

export default defineEventHandler(async (event) => {
  const authHeader = event.req.headers.authorization;
  const token =
    authHeader && authHeader.startsWith("Bearer ") ? authHeader.slice(7) : null;

  if (!token) {
    setResponseStatus(event, 401, "Unauthorized: No token provided");
    return {
      message: "No token access",
    };
  }

  if (verifyRefreshToken(token).valid) {
    setResponseStatus(
      event,
      401,
      "Unauthorized: Provided token is a refresh token",
    );
    return {
      message: "Refresh token cannot be used for authentication",
    };
  }

  const userId = await getUserIdFromToken(token);

  if (!userId) {
    setResponseStatus(event, 401, "Unauthorized: Invalid access token");
    return {
      message: "Invalid access token",
    };
  }

  const isValid = await verifyAccessToken(token);

  if (!isValid.valid) {
    const storedRefreshToken = await getStoreRefreshToken(userId);
    const RefreshToken: any = getCookie(event, "refreshToken");

    if (
      verifyRefreshToken(RefreshToken).valid &&
      storedRefreshToken === RefreshToken
    ) {
      const newToken = await createAccessToken(userId);
      setCookie(event, "token", newToken);
      setResponseStatus(event, 200, "OK");
      return {
        message: "A new access token has been generated",
      };
    } else {
      deleteCookie(event, "refreshToken");
      deleteCookie(event, "token");
      setResponseStatus(event, 401, "Unauthorized");
      return {
        message:
          "Invalid refresh token, please log in again to get a new refresh token",
      };
    }
  } else {
    const userdata: any = await getUserData(token);
    setResponseStatus(event, 200, "Ok");
    return {
      message: "Access token is valid",
      userdata: {
        id: userdata.id,
        username: userdata.username,
        email: userdata.email,
        role: userdata.role,
      },
    };
  }
});
