import { defineStore } from "pinia";

interface GuestData {
  id: number;
  name: string;
  surname: string;
  phone: string;
  email: string;
  address: string;
  city: string;
  country: string;
  zip_code: string;
  description?: string;
  nip?: string;
}

export const useMySlideoutStore = defineStore({
  id: "mySlideoutStore",
  state: () => ({
    isPanelOpen: false,
    activeAction: null as string | null,
    guestdata: {} as GuestData,
  }),
  actions: {
    async openPanel(action: string, guestId?: number) {
      if (action === "edit") {
        try {
          const response = await $fetch<GuestData>(`/api/guest/${guestId}`, {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
              // Authorization: `Bearer ${token}`,
            },
          });

          if (response) {
            this.guestdata = response;
          }
        } catch (error) {
          console.error("Failed to fetch guests:", error);
        }
        this.activeAction = "edit";
      }

      if (action === "add") {
        this.activeAction = "add";
      }
      this.isPanelOpen = true;
    },
    closePanel() {
      this.guestdata = {} as GuestData;
      this.isPanelOpen = false;
    },
    async editGuest(guestId: number) {
      try {
        const response = await $fetch<GuestData>(`/api/guest/${guestId}`, {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
            // Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify(this.guestdata),
        });
        if (!response) {
          throw new Error(`HTTP error! status: ${response}`);
        }
      } catch (error) {
        console.error("Error:", error);
      } finally {
        this.guestdata = {} as GuestData;
        this.closePanel();
      }
    },
    async addGuest() {
      try {
        const response = await $fetch<GuestData>("/api/guest/create", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            // Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify(this.guestdata),
        });
        if (!response) {
          throw new Error(`HTTP error! status: ${response}`);
        }
      } catch (error) {
        console.error("Error:", error);
      } finally {
        this.closePanel();
      }
    },
  },
});
