import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import type { Room, RoomStatus } from '@/types/api/room';
import { getSignalRService, type RoomStatusUpdate, type RoomUpdate, type RoomCreatedData } from '@/services/signalRService';
import * as signalR from '@microsoft/signalr';
import { toast } from 'sonner';

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
  // Real-time connection state
  isConnected: boolean;
  connectionError: string | null;
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
  
  // Real-time connection management
  initializeRealTimeConnection: (apiBaseUrl: string, getAuthToken: () => string | null) => void;
  setConnectionState: (isConnected: boolean, error?: string | null) => void;
  joinBranchGroup: (branchId: number) => void;
  leaveBranchGroup: (branchId: number) => void;
  
  // Reset state
  reset: () => void;
}

export type RoomsStore = RoomsState & RoomsActions;

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
  isConnected: false,
  connectionError: null,
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
      
      // Real-time connection management
      initializeRealTimeConnection: (apiBaseUrl: string, getAuthToken: () => string | null) => {
        try {
          const signalRService = getSignalRService(apiBaseUrl, getAuthToken);
          
          // Set up event handlers
          signalRService.onRoomStatusChanged((update: RoomStatusUpdate) => {
            const { updateRoom } = get();
            const room = get().rooms.find(r => r.id === update.roomId);
            if (room) {
              updateRoom({ ...room, status: update.newStatus });
              toast.success(`Room ${room.roomNumber} status updated to ${update.newStatus}`);
            }
          });

          signalRService.onRoomUpdated((update: RoomUpdate) => {
            const { updateRoom } = get();
            if (update.data) {
              updateRoom(update.data);
              toast.info(`Room ${update.data.roomNumber} has been updated`);
            }
          });

          signalRService.onRoomCreated((data: RoomCreatedData) => {
            const { addRoom } = get();
            if (data.data) {
              addRoom(data.data);
              toast.success(`New room ${data.data.roomNumber} has been created`);
            }
          });

          signalRService.onRoomDeleted((roomId: number) => {
            const { removeRoom } = get();
            const room = get().rooms.find(r => r.id === roomId);
            removeRoom(roomId);
            toast.info(`Room ${room?.roomNumber || roomId} has been deleted`);
          });

          signalRService.onSystemNotification((notification) => {
            switch (notification.type) {
              case 'error':
                toast.error(notification.message);
                break;
              case 'warning':
                toast.warning(notification.message);
                break;
              case 'success':
                toast.success(notification.message);
                break;
              default:
                toast.info(notification.message);
            }
          });

          signalRService.onConnectionStateChanged((state) => {
            const isConnected = state === signalR.HubConnectionState.Connected;
            set({ isConnected, connectionError: null }, false, 'connectionStateChanged');
          });

          // Connect to SignalR
          signalRService.connect().catch((error) => {
            set({ 
              isConnected: false, 
              connectionError: error.message 
            }, false, 'connectionError');
          });

        } catch (error) {
          set({ 
            isConnected: false, 
            connectionError: error instanceof Error ? error.message : 'Unknown connection error' 
          }, false, 'initializationError');
        }
      },

      setConnectionState: (isConnected: boolean, error?: string | null) => set({
        isConnected,
        connectionError: error || null
      }, false, 'setConnectionState'),

      joinBranchGroup: (branchId: number) => {
        try {
          const signalRService = getSignalRService();
          signalRService.joinBranchGroup(branchId);
        } catch (error) {
          console.error('Failed to join branch group:', error);
        }
      },

      leaveBranchGroup: (branchId: number) => {
        try {
          const signalRService = getSignalRService();
          signalRService.leaveBranchGroup(branchId);
        } catch (error) {
          console.error('Failed to leave branch group:', error);
        }
      },
      
      // Reset state
      reset: () => set(initialState, false, 'reset'),
    }),
    { name: 'rooms-store' }
  )
);
