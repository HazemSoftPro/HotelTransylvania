import { create } from 'zustand';
import type { HotelService } from '@/services/serviceService';
import { serviceService } from '@/services/serviceService';

interface ServiceState {
  services: HotelService[];
  selectedService: HotelService | null;
  isLoading: boolean;
  error: string | null;
  
  // Actions
  fetchServices: (pageNumber?: number, pageSize?: number) => Promise<void>;
  fetchServiceById: (id: number) => Promise<void>;
  createService: (data: Omit<HotelService, 'id' | 'branchName'>) => Promise<HotelService>;
  updateService: (data: HotelService) => Promise<HotelService>;
  deleteService: (id: number) => Promise<void>;
  setSelectedService: (service: HotelService | null) => void;
  clearError: () => void;
}

export const useServiceStore = create<ServiceState>((set, get) => ({
  services: [],
  selectedService: null,
  isLoading: false,
  error: null,

  fetchServices: async (pageNumber = 1, pageSize = 100) => {
    set({ isLoading: true, error: null });
    try {
      const response = await serviceService.getAll(pageNumber, pageSize);
      set({ services: response.data, isLoading: false });
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to fetch services',
        isLoading: false 
      });
      throw error;
    }
  },

  fetchServiceById: async (id: number) => {
    set({ isLoading: true, error: null });
    try {
      const response = await serviceService.getById(id);
      set({ selectedService: response.data, isLoading: false });
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to fetch service',
        isLoading: false 
      });
      throw error;
    }
  },

  createService: async (data) => {
    set({ isLoading: true, error: null });
    try {
      const response = await serviceService.create(data);
      const newService = response.data;
      
      // Optimistic update
      set(state => ({ 
        services: [...state.services, newService],
        isLoading: false 
      }));
      
      return newService;
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to create service',
        isLoading: false 
      });
      throw error;
    }
  },

  updateService: async (data) => {
    set({ isLoading: true, error: null });
    
    // Store original state for rollback
    const originalServices = get().services;
    
    // Optimistic update
    set(state => ({
      services: state.services.map(s => 
        s.id === data.id ? data : s
      )
    }));
    
    try {
      const response = await serviceService.update(data);
      const updatedService = response.data;
      
      set(state => ({ 
        services: state.services.map(s => 
          s.id === updatedService.id ? updatedService : s
        ),
        selectedService: state.selectedService?.id === updatedService.id 
          ? updatedService 
          : state.selectedService,
        isLoading: false 
      }));
      
      return updatedService;
    } catch (error) {
      // Rollback on error
      set({ 
        services: originalServices,
        error: error instanceof Error ? error.message : 'Failed to update service',
        isLoading: false 
      });
      throw error;
    }
  },

  deleteService: async (id: number) => {
    set({ isLoading: true, error: null });
    
    // Store original state for rollback
    const originalServices = get().services;
    
    // Optimistic update
    set(state => ({
      services: state.services.filter(s => s.id !== id)
    }));
    
    try {
      await serviceService.delete(id);
      set({ isLoading: false });
    } catch (error) {
      // Rollback on error
      set({ 
        services: originalServices,
        error: error instanceof Error ? error.message : 'Failed to delete service',
        isLoading: false 
      });
      throw error;
    }
  },

  setSelectedService: (service) => {
    set({ selectedService: service });
  },

  clearError: () => {
    set({ error: null });
  }
}));