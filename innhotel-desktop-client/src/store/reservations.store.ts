import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import type { ReservationResponse } from '@/types/api/reservation';

interface ReservationsState {
  reservations: ReservationResponse[];
  selectedReservation: ReservationResponse | null;
  isLoading: boolean;
  error: string | null;
  searchTerm: string;
  filters: {
    guestName?: string;
    roomNumbers?: string[];
    status?: string;
    checkInDateFrom?: string;
    checkInDateTo?: string;
    checkOutDateFrom?: string;
    checkOutDateTo?: string;
  };
  pagination: {
    pageNumber: number;
    pageSize: number;
    totalResults: number;
  };
}

interface ReservationsActions {
  // Reservation management
  setReservations: (reservations: ReservationResponse[]) => void;
  addReservation: (reservation: ReservationResponse) => void;
  updateReservation: (reservation: ReservationResponse) => void;
  removeReservation: (reservationId: number) => void;
  setSelectedReservation: (reservation: ReservationResponse | null) => void;
  
  // Loading and error states
  setLoading: (loading: boolean) => void;
  setError: (error: string | null) => void;
  
  // Search and filtering
  setSearchTerm: (term: string) => void;
  setFilters: (filters: Partial<ReservationsState['filters']>) => void;
  clearFilters: () => void;
  
  // Pagination
  setPagination: (pagination: Partial<ReservationsState['pagination']>) => void;
  
  // Reset state
  reset: () => void;
}

type ReservationsStore = ReservationsState & ReservationsActions;

const initialState: ReservationsState = {
  reservations: [],
  selectedReservation: null,
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

export const useReservationsStore = create<ReservationsStore>()(
  devtools(
    (set, get) => ({
      ...initialState,
      
      // Reservation management
      setReservations: (reservations) => set({ reservations }, false, 'setReservations'),
      
      addReservation: (reservation) => set(
        (state) => ({ reservations: [...state.reservations, reservation] }),
        false,
        'addReservation'
      ),
      
      updateReservation: (updatedReservation) => set(
        (state) => ({
          reservations: state.reservations.map(reservation => 
            reservation.id === updatedReservation.id ? updatedReservation : reservation
          ),
          selectedReservation: state.selectedReservation?.id === updatedReservation.id 
            ? updatedReservation 
            : state.selectedReservation,
        }),
        false,
        'updateReservation'
      ),
      
      removeReservation: (reservationId) => set(
        (state) => ({
          reservations: state.reservations.filter(reservation => reservation.id !== reservationId),
          selectedReservation: state.selectedReservation?.id === reservationId 
            ? null 
            : state.selectedReservation,
        }),
        false,
        'removeReservation'
      ),
      
      setSelectedReservation: (reservation) => set({ selectedReservation: reservation }, false, 'setSelectedReservation'),
      
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
    { name: 'reservations-store' }
  )
);
