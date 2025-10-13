import axiosInstance from '../lib/axios';
import { isAxiosError } from 'axios';
import { logger } from '@/utils/logger';

interface RoomTypeResponse {
  status: number;
  message: string;
  data: Array<{
    id: number;
    branchId: number;
    branchName: string;
    name: string;
    basePrice: number;
    capacity: number;
    description: string | null;
  }>;
}

export const roomTypeService = {
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
  }
};
