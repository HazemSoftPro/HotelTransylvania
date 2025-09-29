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

export const useMyModalStore = defineStore({
  id: "myModalStore",
  state: () => ({
    isModalOpen: false,
    guestdata: {} as GuestData,
  }),
  actions: {
    async openModal(guestId: number) {
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

      this.isModalOpen = true;
    },
    closeModal() {
      this.isModalOpen = false;
    },
    toggleModal() {
      this.isModalOpen = !this.isModalOpen;
    },
    async deleteGuest(guestId: number) {
      try {
        await $fetch(`/api/guest/${guestId}`, {
          method: "DELETE",
          headers: {
            "Content-Type": "application/json",
            // Authorization: `Bearer ${token}`,
          },
        });
      } catch (error) {
        console.error("Failed to delete guest:", error);
      }

      this.isModalOpen = false;
    },
  },
});
