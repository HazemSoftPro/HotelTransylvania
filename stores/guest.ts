import { defineStore } from "pinia";
import type { Guest } from "~/server/models/guest";

interface ResponseType {
  guests: Guest[];
  totalPages: number;
}

export const useMyGuestStore = defineStore({
  id: "myGuestStore",
  state: () => ({
    guests: [] as Guest[],
    loading: false,
    totalPages: 0,
    sortField: "" as string | null,
    sortDirection: "" as "asc" | "desc" | null,
  }),
  actions: {
    async fetchGuests(limit: number, page: number, sq: string) {
      this.loading = true;
      try {
        const response = await $fetch<ResponseType>(`/api/guest/list`, {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            // Authorization: `Bearer ${token}`,
          },
          params: {
            page: page,
            limit: limit,
            query: sq,
            field: this.sortField,
            direction: this.sortDirection,
          },
        });

        if (response) {
          this.guests = response.guests;
          this.totalPages = response.totalPages;
        }
      } catch (error) {
        console.error("Failed to fetch guests:", error);
      } finally {
        this.loading = false;
      }
    },
    sortGuests(field: string, direction: "asc" | "desc") {
      this.sortField = field;
      this.sortDirection = direction;
    },
  },
});
