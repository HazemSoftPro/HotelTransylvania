import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import type { Employee } from '@/types/api/employee';

interface EmployeesState {
  employees: Employee[];
  selectedEmployee: Employee | null;
  isLoading: boolean;
  error: string | null;
  searchTerm: string;
  filters: {
    branchId?: number;
    position?: string;
    hireDateFrom?: string;
    hireDateTo?: string;
  };
  pagination: {
    pageNumber: number;
    pageSize: number;
    totalResults: number;
  };
}

interface EmployeesActions {
  // Employee management
  setEmployees: (employees: Employee[]) => void;
  addEmployee: (employee: Employee) => void;
  updateEmployee: (employee: Employee) => void;
  removeEmployee: (employeeId: number) => void;
  setSelectedEmployee: (employee: Employee | null) => void;
  
  // Loading and error states
  setLoading: (loading: boolean) => void;
  setError: (error: string | null) => void;
  
  // Search and filtering
  setSearchTerm: (term: string) => void;
  setFilters: (filters: Partial<EmployeesState['filters']>) => void;
  clearFilters: () => void;
  
  // Pagination
  setPagination: (pagination: Partial<EmployeesState['pagination']>) => void;
  
  // Reset state
  reset: () => void;
}

type EmployeesStore = EmployeesState & EmployeesActions;

const initialState: EmployeesState = {
  employees: [],
  selectedEmployee: null,
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

export const useEmployeesStore = create<EmployeesStore>()(
  devtools(
    (set, _get) => ({
      ...initialState,
      
      // Employee management
      setEmployees: (employees) => set({ employees }, false, 'setEmployees'),
      
      addEmployee: (employee) => set(
        (state) => ({ employees: [...state.employees, employee] }),
        false,
        'addEmployee'
      ),
      
      updateEmployee: (updatedEmployee) => set(
        (state) => ({
          employees: state.employees.map(employee => 
            employee.id === updatedEmployee.id ? updatedEmployee : employee
          ),
          selectedEmployee: state.selectedEmployee?.id === updatedEmployee.id 
            ? updatedEmployee 
            : state.selectedEmployee,
        }),
        false,
        'updateEmployee'
      ),
      
      removeEmployee: (employeeId) => set(
        (state) => ({
          employees: state.employees.filter(employee => employee.id !== employeeId),
          selectedEmployee: state.selectedEmployee?.id === employeeId 
            ? null 
            : state.selectedEmployee,
        }),
        false,
        'removeEmployee'
      ),
      
      setSelectedEmployee: (employee) => set({ selectedEmployee: employee }, false, 'setSelectedEmployee'),
      
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
    { name: 'employees-store' }
  )
);
