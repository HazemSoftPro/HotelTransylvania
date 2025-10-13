// Export all store hooks for easy importing
export { useAuthStore } from './auth.store';
export { useRoomsStore } from './rooms.store';
export { useGuestsStore } from './guests.store';
export { useReservationsStore } from './reservations.store';
export { useEmployeesStore } from './employees.store';

// Store selectors for optimized re-renders
export const authSelectors = {
  isAuthenticated: (state: any) => state.isAuthenticated,
  user: (state: any) => ({ email: state.email, roles: state.roles }),
  isLoading: (state: any) => state.isLoading,
};

export const roomsSelectors = {
  rooms: (state: any) => state.rooms,
  selectedRoom: (state: any) => state.selectedRoom,
  isLoading: (state: any) => state.isLoading,
  searchTerm: (state: any) => state.searchTerm,
  filters: (state: any) => state.filters,
  pagination: (state: any) => state.pagination,
};

export const guestsSelectors = {
  guests: (state: any) => state.guests,
  selectedGuest: (state: any) => state.selectedGuest,
  isLoading: (state: any) => state.isLoading,
  searchTerm: (state: any) => state.searchTerm,
  filters: (state: any) => state.filters,
  pagination: (state: any) => state.pagination,
};

export const reservationsSelectors = {
  reservations: (state: any) => state.reservations,
  selectedReservation: (state: any) => state.selectedReservation,
  isLoading: (state: any) => state.isLoading,
  searchTerm: (state: any) => state.searchTerm,
  filters: (state: any) => state.filters,
  pagination: (state: any) => state.pagination,
};

export const employeesSelectors = {
  employees: (state: any) => state.employees,
  selectedEmployee: (state: any) => state.selectedEmployee,
  isLoading: (state: any) => state.isLoading,
  searchTerm: (state: any) => state.searchTerm,
  filters: (state: any) => state.filters,
  pagination: (state: any) => state.pagination,
};
