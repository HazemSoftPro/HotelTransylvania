import { defineStore } from "pinia";
import type { Room, Equipment } from "~/server/models/room";

type Response = {
  room: Room;
  equipment: Equipment;
  vacant: number;
  rooms: Room[];
};

enum RoomStatus {
  CLEAN = "CLEAN",
  DIRTY = "DIRTY",
  SERVICE = "SERVICE",
  OCCUPIED = "OCCUPIED",
  ARRIVAL = "ARRIVAL",
}

export const useMyRoomStore = defineStore({
  id: "myRoomStore",
  state: () => ({
    rooms: [] as Room[],
    isLoading: false,
    isModalOpen: false,
    activeAction: null as string | null,
    room: {} as Room,
    availableRoom: [] as Room[],
    availableVacant: 0,
    equipment: {} as Equipment,
  }),
  actions: {
    async fetchAvailableRooms(dateStart: string, dateEnd: string) {
      this.isLoading = true;
      try {
        const response = await $fetch<Response>(`/api/room/available`, {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            // Authorization: `Bearer ${token}`,
          },
          params: {
            dateStart: dateStart,
            dateEnd: dateEnd,
          },
        });

        if (response) {
          this.availableRoom = response.rooms;
          this.availableVacant = response.vacant;
        }
      } catch (error) {
        console.error("Failed to fetch available rooms:", error);
      } finally {
        this.isLoading = false;
      }
    },
    async fetchRooms(number?: string) {
      this.isLoading = true;
      try {
        const response = await $fetch<Response>(`/api/room/list`, {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            // Authorization: `Bearer ${token}`,
          },
          params: {
            number: number,
          },
        });

        if (response) {
          this.rooms = response.rooms;
        }
      } catch (error) {
        console.error("Failed to fetch rooms:", error);
      } finally {
        this.isLoading = false;
      }
    },
    async openModal(action: string, roomId?: number) {
      if (action === "info") {
        try {
          const response = await $fetch<Response>(`/api/room/${roomId}`, {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
              // Authorization: `Bearer ${token}`,
            },
          });

          if (response) {
            this.room = response.room;
            this.equipment = response.equipment;
          }
        } catch (error) {
          console.error("Failed to fetch room:", error);
        }
        this.activeAction = "info";
      }

      if (action === "add") {
        this.activeAction = "add";
      }

      if (action === "clean") {
        try {
          const response = await $fetch<Response>(`/api/room/${roomId}`, {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
              // Authorization: `Bearer ${token}`,
            },
          });

          if (response) {
            this.room = response.room;
            this.equipment = response.equipment;
          }
        } catch (error) {
          console.error("Failed to fetch room:", error);
        }
        this.activeAction = "clean";
      }

      if (action === "delete") {
        try {
          const response = await $fetch<Response>(`/api/room/${roomId}`, {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
              // Authorization: `Bearer ${token}`,
            },
          });

          if (response) {
            this.room = response.room;
            this.equipment = response.equipment;
          }
        } catch (error) {
          console.error("Failed to fetch room:", error);
        }
        this.activeAction = "delete";
      }

      if (action === "edit") {
        try {
          const response = await $fetch<Response>(`/api/room/${roomId}`, {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
              // Authorization: `Bearer ${token}`,
            },
          });

          if (response) {
            this.room = response.room;
            this.equipment = response.equipment;
          }
        } catch (error) {
          console.error("Failed to fetch room:", error);
        }
        this.activeAction = "edit";
      }

      this.isModalOpen = true;
    },
    closeModal() {
      this.room = {} as Room;
      this.equipment = {} as Equipment;
      this.isModalOpen = false;
    },
    async deleteRoom(roomId: number) {
      try {
        await $fetch(`/api/room/${roomId}`, {
          method: "DELETE",
          headers: {
            "Content-Type": "application/json",
            // Authorization: `Bearer ${token}`,
          },
        });
      } catch (error) {
        push.error($t('error_while_deleting_room'));
      } finally {
        push.success($t('room_deleted_successfully'));
        this.closeModal();
      }
    },
    async editRoom(roomId: number, status?: RoomStatus) {
      try {
        if (status) {
          this.room.status = status;
        }
        const body = {
          room: this.room,
          equipment: this.equipment,
        };
        const response = await $fetch<Room>(`/api/room/${roomId}`, {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
            // Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify(body),
        });
        if (!response) {
          throw new Error(`HTTP error! status: ${response}`);
        }
      } catch (error) {
        push.error($t('error_while_updating_room'));
      } finally {
        push.success($t('room_updated_successfully'));
        this.closeModal();
        this.room = {} as Room;
      }
    },
    async addRoom() {
      try {
        const body = {
          room: this.room,
          equipment: this.equipment,
        };

        const response = await $fetch<Room>("/api/room/create", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            // Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify(body),
        });

        if (response) {
          push.success($t('room_added_successfully'));
          this.closeModal();
        }
      } catch (error) {
        push.error($t('error_while_adding_room'));
      }
    },
  },
});