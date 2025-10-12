import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import type { Guest } from '@/types/api/guest';

interface GuestsState {
  guests: Guest[];
  selectedGuest: Guest | null;
  isLoading: boolean;
  error: string | null;
  searchTerm: string;
  filters: {
    gender?: string;
    idProofType?: string;
  };
  pagination: {
    pageNumber: number;
    pageSize: number;
    totalResults: number;
  };
}

interface GuestsActions {
  // Guest management
  setGuests: (guests: Guest[]) => void;
  addGuest: (guest: Guest) => void;
  updateGuest: (guest: Guest) => void;
  removeGuest: (guestId: number) => void;
  setSelectedGuest: (guest: Guest | null) => void;
  
  // Loading and error states
  setLoading: (loading: boolean) => void;
  setError: (error: string | null) => void;
  
  // Search and filtering
  setSearchTerm: (term: string) => void;
  setFilters: (filters: Partial<GuestsState['filters']>) => void;
  clearFilters: () => void;
  
  // Pagination
  setPagination: (pagination: Partial<GuestsState['pagination']>) => void;
  
  // Reset state
  reset: () => void;
}

type GuestsStore = GuestsState & GuestsActions;

const initialState: GuestsState = {
  guests: [],
  selectedGuest: null,
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

export const useGuestsStore = create<GuestsStore>()(
  devtools(
    (set, get) => ({
      ...initialState,
      
      // Guest management
      setGuests: (guests) => set({ guests }, false, 'setGuests'),
      
      addGuest: (guest) => set(
        (state) => ({ guests: [...state.guests, guest] }),
        false,
        'addGuest'
      ),
      
      updateGuest: (updatedGuest) => set(
        (state) => ({
          guests: state.guests.map(guest => 
            guest.id === updatedGuest.id ? updatedGuest : guest
          ),
          selectedGuest: state.selectedGuest?.id === updatedGuest.id 
            ? updatedGuest 
            : state.selectedGuest,
        }),
        false,
        'updateGuest'
      ),
      
      removeGuest: (guestId) => set(
        (state) => ({
          guests: state.guests.filter(guest => guest.id !== guestId),
          selectedGuest: state.selectedGuest?.id === guestId 
            ? null 
            : state.selectedGuest,
        }),
        false,
        'removeGuest'
      ),
      
      setSelectedGuest: (guest) => set({ selectedGuest: guest }, false, 'setSelectedGuest'),
      
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
    { name: 'guests-store' }
  )
);
