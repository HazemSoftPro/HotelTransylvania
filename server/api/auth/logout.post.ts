export default defineEventHandler(async (event) => {
  setCookie(event, "token", "");
  setResponseStatus(event, 200, "OK");
  return {
    message: "Successfully logged off",
  };
});
