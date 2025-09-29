import { defineStore } from "pinia";

interface UserData {
  authed: boolean;
  userdata?: {
    id: string;
    username: string;
    email: string;
    role: string;
  };
}

export const useAuthStore = defineStore({
  id: "auth",
  state: () => ({
    data: {
      authed: true,
      userdata: { id: "", username: "", email: "", role: "" },
    } as UserData,
  }),
  actions: {
    async checkToken(token: string) {
      if (!token) {
        this.data.authed = false;
      }

      try {
        const response = await $fetch<UserData>(`/api/auth/token`, {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        });
        if (response) {
          this.data.authed = true;
          this.data.userdata = response.userdata;
        }
      } catch (error: unknown) {
        this.data.authed = false;
        if (error instanceof Error) {
          console.log(error.message);
        }
      }
    },
    async logout() {
      try {
        const response = await fetch("/api/auth/logout", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
        });

        if (response.status === 200) {
          this.data.authed = false;
          if (process.client) {
            window.location.reload();
          }
        }
      } catch (error: unknown) {
        if (error instanceof Error) {
          console.log(error.message);
        }
      }
    },
    async login(username: string, password: string) {
      try {
        const response = await $fetch<UserData>("/api/auth/login", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ username, password }),
        });

        if (response) {
          this.data.authed = true;
          this.data.userdata = response.userdata;
          navigateTo("/");
        } else {
          this.data.authed = false;
        }
      } catch (error: unknown) {
        if (error instanceof Error) {
          console.log(error.message);
        }
      }
    },
  },
});
