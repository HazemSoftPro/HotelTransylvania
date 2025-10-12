import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import type { Room } from '@/types/api/room';

interface RoomsState {
  rooms: Room[];
  selectedRoom: Room | null;
  isLoading: boolean;
  error: string | null;
  searchTerm: string;
  filters: {
    branchId?: number;
    roomTypeId?: number;
    status?: string;
    floor?: number;
  };
  pagination: {
    pageNumber: number;
    pageSize: number;
    totalResults: number;
  };
}

interface RoomsActions {
  // Room management
  setRooms: (rooms: Room[]) => void;
  addRoom: (room: Room) => void;
  updateRoom: (room: Room) => void;
  removeRoom: (roomId: number) => void;
  setSelectedRoom: (room: Room | null) => void;
  
  // Loading and error states
  setLoading: (loading: boolean) => void;
  setError: (error: string | null) => void;
  
  // Search and filtering
  setSearchTerm: (term: string) => void;
  setFilters: (filters: Partial<RoomsState['filters']>) => void;
  clearFilters: () => void;
  
  // Pagination
  setPagination: (pagination: Partial<RoomsState['pagination']>) => void;
  
  // Reset state
  reset: () => void;
}

type RoomsStore = RoomsState & RoomsActions;

const initialState: RoomsState = {
  rooms: [],
  selectedRoom: null,
  isLoading: false,
  error: null,
  searchTerm: '',
  filters: {},
  pagination: {
    pageNumber: 1,
    pageSize: 10,
    totalResults: 0,
  },
};

export const useRoomsStore = create<RoomsStore>()(
  devtools(
    (set, get) => ({
      ...initialState,
      
      // Room management
      setRooms: (rooms) => set({ rooms }, false, 'setRooms'),
      
      addRoom: (room) => set(
        (state) => ({ rooms: [...state.rooms, room] }),
        false,
        'addRoom'
      ),
      
      updateRoom: (updatedRoom) => set(
        (state) => ({
          rooms: state.rooms.map(room => 
            room.id === updatedRoom.id ? updatedRoom : room
          ),
          selectedRoom: state.selectedRoom?.id === updatedRoom.id 
            ? updatedRoom 
            : state.selectedRoom,
        }),
        false,
        'updateRoom'
      ),
      
      removeRoom: (roomId) => set(
        (state) => ({
          rooms: state.rooms.filter(room => room.id !== roomId),
          selectedRoom: state.selectedRoom?.id === roomId 
            ? null 
            : state.selectedRoom,
        }),
        false,
        'removeRoom'
      ),
      
      setSelectedRoom: (room) => set({ selectedRoom: room }, false, 'setSelectedRoom'),
      
      // Loading and error states
      setLoading: (loading) => set({ isLoading: loading }, false, 'setLoading'),
      setError: (error) => set({ error }, false, 'setError'),
      
      // Search and filtering
      setSearchTerm: (searchTerm) => set({ searchTerm }, false, 'setSearchTerm'),
      
      setFilters: (newFilters) => set(
        (state) => ({ filters: { ...state.filters, ...newFilters } }),
        false,
        'setFilters'
      ),
      
      clearFilters: () => set({ filters: {} }, false, 'clearFilters'),
      
      // Pagination
      setPagination: (newPagination) => set(
        (state) => ({ 
          pagination: { ...state.pagination, ...newPagination } 
        }),
        false,
        'setPagination'
      ),
      
      // Reset state
      reset: () => set(initialState, false, 'reset'),
    }),
    { name: 'rooms-store' }
  )
);
