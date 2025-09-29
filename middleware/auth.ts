export default defineNuxtRouteMiddleware(async (to, from) => {
  const authStore = useAuthStore();
  const token = useCookie("token").value || "";
  await authStore.checkToken(token);
  const data = authStore.data;

  if (data.authed == false) {
    return navigateTo("/login");
  }
});
