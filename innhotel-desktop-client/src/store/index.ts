// Export all store hooks for easy importing
export { useAuthStore } from './auth.store';
export { useRoomsStore } from './rooms.store';
export { useRoomTypeStore } from './roomTypes.store';
export { useServiceStore } from './services.store';
export { useGuestsStore } from './guests.store';
export { useReservationsStore } from './reservations.store';
export { useEmployeesStore } from './employees.store';

// Import store types for selectors
import type { AuthStore } from './auth.store';
import type { RoomsStore } from './rooms.store';
import type { GuestsStore } from './guests.store';
import type { ReservationsStore } from './reservations.store';
import type { EmployeesStore } from './employees.store';

// Store selectors for optimized re-renders
export const authSelectors = {
  isAuthenticated: (state: AuthStore) => state.isAuthenticated,
  user: (state: AuthStore) => ({ email: state.email, roles: state.roles }),
  isLoading: (state: AuthStore) => state.isLoading,
};

export const roomsSelectors = {
  rooms: (state: RoomsStore) => state.rooms,
  selectedRoom: (state: RoomsStore) => state.selectedRoom,
  isLoading: (state: RoomsStore) => state.isLoading,
  searchTerm: (state: RoomsStore) => state.searchTerm,
  filters: (state: RoomsStore) => state.filters,
  pagination: (state: RoomsStore) => state.pagination,
};

export const guestsSelectors = {
  guests: (state: GuestsStore) => state.guests,
  selectedGuest: (state: GuestsStore) => state.selectedGuest,
  isLoading: (state: GuestsStore) => state.isLoading,
  searchTerm: (state: GuestsStore) => state.searchTerm,
  filters: (state: GuestsStore) => state.filters,
  pagination: (state: GuestsStore) => state.pagination,
};

export const reservationsSelectors = {
  reservations: (state: ReservationsStore) => state.reservations,
  selectedReservation: (state: ReservationsStore) => state.selectedReservation,
  isLoading: (state: ReservationsStore) => state.isLoading,
  searchTerm: (state: ReservationsStore) => state.searchTerm,
  filters: (state: ReservationsStore) => state.filters,
  pagination: (state: ReservationsStore) => state.pagination,
};

export const employeesSelectors = {
  employees: (state: EmployeesStore) => state.employees,
  selectedEmployee: (state: EmployeesStore) => state.selectedEmployee,
  isLoading: (state: EmployeesStore) => state.isLoading,
  searchTerm: (state: EmployeesStore) => state.searchTerm,
  filters: (state: EmployeesStore) => state.filters,
  pagination: (state: EmployeesStore) => state.pagination,
};
