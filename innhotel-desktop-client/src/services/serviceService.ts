import axiosInstance from '../lib/axios';
import { isAxiosError } from 'axios';
import { logger } from '@/utils/logger';

export interface HotelService {
  id: number;
  branchId: number;
  branchName: string;
  name: string;
  price: number;
  description: string | null;
}

interface ServiceResponse {
  status: number;
  message: string;
  data: HotelService[];
}

interface SingleServiceResponse {
  status: number;
  message: string;
  data: HotelService;
}

export interface CreateServiceRequest {
  branchId: number;
  name: string;
  price: number;
  description?: string;
}

export interface UpdateServiceRequest {
  id: number;
  branchId: number;
  name: string;
  price: number;
  description?: string;
}

export const serviceService = {
  /**
   * Get all services with pagination
   */
  getAll: async (pageNumber: number = 1, pageSize: number = 100): Promise<ServiceResponse> => {
    try {
      logger().info('Fetching all services', { pageNumber, pageSize });
      const response = await axiosInstance.get('/services', {
        params: { pageNumber, pageSize }
      });
      logger().info('Successfully fetched all services');
      return response.data;
    } catch (error) {
      if (isAxiosError(error)) {
        logger().error('Failed to fetch services', {
          status: error.response?.status,
          message: error.response?.data?.message
        });
      }
      throw error;
    }
  },

  /**
   * Get a single service by ID
   */
  getById: async (id: number): Promise<SingleServiceResponse> => {
    try {
      logger().info(`Fetching service with ID: ${id}`);
      const response = await axiosInstance.get(`/services/${id}`);
      logger().info(`Successfully fetched service with ID: ${id}`);
      return response.data;
    } catch (error) {
      if (isAxiosError(error)) {
        logger().error(`Failed to fetch service with ID: ${id}`, {
          status: error.response?.status,
          message: error.response?.data?.message
        });
      }
      throw error;
    }
  },

  /**
   * Create a new service
   */
  create: async (data: CreateServiceRequest): Promise<SingleServiceResponse> => {
    try {
      logger().info('Creating new service', { name: data.name });
      const response = await axiosInstance.post('/services', data);
      logger().info('Successfully created service', { id: response.data.data.id });
      return response.data;
    } catch (error) {
      if (isAxiosError(error)) {
        logger().error('Failed to create service', {
          status: error.response?.status,
          message: error.response?.data?.message
        });
      }
      throw error;
    }
  },

  /**
   * Update an existing service
   */
  update: async (data: UpdateServiceRequest): Promise<SingleServiceResponse> => {
    try {
      logger().info(`Updating service with ID: ${data.id}`);
      const response = await axiosInstance.put(`/services/${data.id}`, data);
      logger().info(`Successfully updated service with ID: ${data.id}`);
      return response.data;
    } catch (error) {
      if (isAxiosError(error)) {
        logger().error(`Failed to update service with ID: ${data.id}`, {
          status: error.response?.status,
          message: error.response?.data?.message
        });
      }
      throw error;
    }
  },

  /**
   * Delete a service
   */
  delete: async (id: number): Promise<{ status: number; message: string }> => {
    try {
      logger().info(`Deleting service with ID: ${id}`);
      const response = await axiosInstance.delete(`/services/${id}`);
      logger().info(`Successfully deleted service with ID: ${id}`);
      return response.data;
    } catch (error) {
      if (isAxiosError(error)) {
        logger().error(`Failed to delete service with ID: ${id}`, {
          status: error.response?.status,
          message: error.response?.data?.message
        });
      }
      throw error;
    }
  }
};