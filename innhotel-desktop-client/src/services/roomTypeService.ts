import axiosInstance from '../lib/axios';
import { isAxiosError } from 'axios';
import { logger } from '@/utils/logger';

export interface RoomType {
  id: number;
  branchId: number;
  branchName: string;
  name: string;
  capacity: number;
  description: string | null;
}

interface RoomTypeResponse {
  status: number;
  message: string;
  data: RoomType[];
}

interface SingleRoomTypeResponse {
  status: number;
  message: string;
  data: RoomType;
}

export interface CreateRoomTypeRequest {
  branchId: number;
  name: string;
  capacity: number;
  description?: string | null;
}

export interface UpdateRoomTypeRequest {
  id: number;
  branchId: number;
  name: string;
  capacity: number;
  description?: string | null;
}

export const roomTypeService = {
  /**
   * Get all room types
   */
  getAll: async (): Promise<RoomTypeResponse> => {
    try {
      logger().info('Fetching all room types');
      const response = await axiosInstance.get('/roomtypes');
      logger().info('Successfully fetched all room types');
      return response.data;
    } catch (error) {
      if (isAxiosError(error)) {
        logger().error('Failed to fetch room types', {
          status: error.response?.status,
          message: error.response?.data?.message
        });
      }
      throw error;
    }
  },

  /**
   * Get a single room type by ID
   */
  getById: async (id: number): Promise<SingleRoomTypeResponse> => {
    try {
      logger().info(`Fetching room type with ID: ${id}`);
      const response = await axiosInstance.get(`/roomtypes/${id}`);
      logger().info(`Successfully fetched room type with ID: ${id}`);
      return response.data;
    } catch (error) {
      if (isAxiosError(error)) {
        logger().error(`Failed to fetch room type with ID: ${id}`, {
          status: error.response?.status,
          message: error.response?.data?.message
        });
      }
      throw error;
    }
  },

  /**
   * Create a new room type
   */
  create: async (data: CreateRoomTypeRequest): Promise<SingleRoomTypeResponse> => {
    try {
      logger().info('Creating new room type', { name: data.name });
      const response = await axiosInstance.post('/roomtypes', data);
      logger().info('Successfully created room type', { id: response.data.data.id });
      return response.data;
    } catch (error) {
      if (isAxiosError(error)) {
        logger().error('Failed to create room type', {
          status: error.response?.status,
          message: error.response?.data?.message
        });
      }
      throw error;
    }
  },

  /**
   * Update an existing room type
   */
  update: async (data: UpdateRoomTypeRequest): Promise<SingleRoomTypeResponse> => {
    try {
      logger().info(`Updating room type with ID: ${data.id}`);
      const response = await axiosInstance.put(`/roomtypes/${data.id}`, data);
      logger().info(`Successfully updated room type with ID: ${data.id}`);
      return response.data;
    } catch (error) {
      if (isAxiosError(error)) {
        logger().error(`Failed to update room type with ID: ${data.id}`, {
          status: error.response?.status,
          message: error.response?.data?.message
        });
      }
      throw error;
    }
  },

  /**
   * Delete a room type
   */
  delete: async (id: number): Promise<{ status: number; message: string }> => {
    try {
      logger().info(`Deleting room type with ID: ${id}`);
      const response = await axiosInstance.delete(`/roomtypes/${id}`);
      logger().info(`Successfully deleted room type with ID: ${id}`);
      return response.data;
    } catch (error) {
      if (isAxiosError(error)) {
        logger().error(`Failed to delete room type with ID: ${id}`, {
          status: error.response?.status,
          message: error.response?.data?.message
        });
      }
      throw error;
    }
  }
};
