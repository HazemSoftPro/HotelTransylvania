import { create } from 'zustand';
import { type RoomType, roomTypeService } from '@/services/roomTypeService';

interface RoomTypeState {
  roomTypes: RoomType[];
  selectedRoomType: RoomType | null;
  isLoading: boolean;
  error: string | null;
  
  // Actions
  fetchRoomTypes: () => Promise<void>;
  fetchRoomTypeById: (id: number) => Promise<void>;
  createRoomType: (data: Omit<RoomType, 'id' | 'branchName'>) => Promise<RoomType>;
  updateRoomType: (data: RoomType) => Promise<RoomType>;
  deleteRoomType: (id: number) => Promise<void>;
  setSelectedRoomType: (roomType: RoomType | null) => void;
  clearError: () => void;
}

export const useRoomTypeStore = create<RoomTypeState>((set, get) => ({
  roomTypes: [],
  selectedRoomType: null,
  isLoading: false,
  error: null,

  fetchRoomTypes: async () => {
    set({ isLoading: true, error: null });
    try {
      const response = await roomTypeService.getAll();
      set({ roomTypes: response.data, isLoading: false });
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to fetch room types',
        isLoading: false 
      });
      throw error;
    }
  },

  fetchRoomTypeById: async (id: number) => {
    set({ isLoading: true, error: null });
    try {
      const response = await roomTypeService.getById(id);
      set({ selectedRoomType: response.data, isLoading: false });
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to fetch room type',
        isLoading: false 
      });
      throw error;
    }
  },

  createRoomType: async (data) => {
    set({ isLoading: true, error: null });
    try {
      const response = await roomTypeService.create(data);
      const newRoomType = response.data;
      
      // Optimistic update
      set(state => ({ 
        roomTypes: [...state.roomTypes, newRoomType],
        isLoading: false 
      }));
      
      return newRoomType;
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to create room type',
        isLoading: false 
      });
      throw error;
    }
  },

  updateRoomType: async (data) => {
    set({ isLoading: true, error: null });
    
    // Store original state for rollback
    const originalRoomTypes = get().roomTypes;
    
    // Optimistic update
    set(state => ({
      roomTypes: state.roomTypes.map(rt => 
        rt.id === data.id ? data : rt
      )
    }));
    
    try {
      const response = await roomTypeService.update(data);
      const updatedRoomType = response.data;
      
      set(state => ({ 
        roomTypes: state.roomTypes.map(rt => 
          rt.id === updatedRoomType.id ? updatedRoomType : rt
        ),
        selectedRoomType: state.selectedRoomType?.id === updatedRoomType.id 
          ? updatedRoomType 
          : state.selectedRoomType,
        isLoading: false 
      }));
      
      return updatedRoomType;
    } catch (error) {
      // Rollback on error
      set({ 
        roomTypes: originalRoomTypes,
        error: error instanceof Error ? error.message : 'Failed to update room type',
        isLoading: false 
      });
      throw error;
    }
  },

  deleteRoomType: async (id: number) => {
    set({ isLoading: true, error: null });
    
    // Store original state for rollback
    const originalRoomTypes = get().roomTypes;
    
    // Optimistic update
    set(state => ({
      roomTypes: state.roomTypes.filter(rt => rt.id !== id)
    }));
    
    try {
      await roomTypeService.delete(id);
      set({ isLoading: false });
    } catch (error) {
      // Rollback on error
      set({ 
        roomTypes: originalRoomTypes,
        error: error instanceof Error ? error.message : 'Failed to delete room type',
        isLoading: false 
      });
      throw error;
    }
  },

  setSelectedRoomType: (roomType) => {
    set({ selectedRoomType: roomType });
  },

  clearError: () => {
    set({ error: null });
  }
}));
