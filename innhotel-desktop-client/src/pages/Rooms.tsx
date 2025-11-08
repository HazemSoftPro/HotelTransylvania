import { useEffect, useState, useMemo } from "react";
import { useNavigate } from "react-router-dom";
import type { RoomResponse } from "@/types/api/room";
import { RoomsListing } from "@/components/rooms/RoomsListing";
import { RoomFilters, type RoomFilterValues } from "@/components/rooms/RoomFilters";
import { roomService } from "@/services/roomService";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import { ROUTES } from "@/constants/routes";
import { Pagination } from "@/components/pagination/Pagination";
import { PAGE_SIZE_OPTIONS } from "@/constants/pagination";
import { ConnectionStatus } from "@/components/ui/connection-status";
import { useRoomsStore } from "@/store/rooms.store";

const Rooms = () => {
  const navigate = useNavigate();
  const { joinBranchGroup } = useRoomsStore();
  const [rooms, setRooms] = useState<RoomResponse[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(8);
  const [totalPages, setTotalPages] = useState(0);
  const [hasPreviousPage, setHasPreviousPage] = useState(false);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [filters, setFilters] = useState<RoomFilterValues>({
    search: "",
    status: "",
    floor: "",
    branchId: "",
    roomTypeId: "",
  });

  useEffect(() => {
    const fetchRooms = async () => {
      try {
        setIsLoading(true);
        const response = await roomService.getAll(currentPage, pageSize);
        setRooms(response.items);
        setTotalPages(response.totalPages);
        setHasPreviousPage(response.hasPreviousPage);
        setHasNextPage(response.hasNextPage);

        // Join branch groups for real-time updates
        const branchIds = [...new Set(response.items.map(room => room.branchId))];
        branchIds.forEach(branchId => joinBranchGroup(branchId));
      } catch (error) {
        console.error('Failed to fetch rooms:', error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchRooms();
  }, [currentPage, pageSize, joinBranchGroup]);

  const handleRoomClick = (room: RoomResponse) => {
    navigate(`${ROUTES.ROOMS}/${room.id}`);
  };

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
  };

  const handlePageSizeChange = (size: number) => {
    setPageSize(size);
    setCurrentPage(1);
  };

  const handleFilterChange = (newFilters: RoomFilterValues) => {
    setFilters(newFilters);
    setCurrentPage(1);
  };

  const handleResetFilters = () => {
    setFilters({
      search: "",
      status: "",
      floor: "",
      branchId: "",
      roomTypeId: "",
    });
    setCurrentPage(1);
  };

  // Apply client-side filtering
  const filteredRooms = useMemo(() => {
    return rooms.filter((room) => {
      // Search filter
      if (filters.search && !room.roomNumber.toLowerCase().includes(filters.search.toLowerCase())) {
        return false;
      }

      // Status filter
      if (filters.status && filters.status !== "all" && room.status.toString() !== filters.status) {
        return false;
      }

      // Floor filter
      if (filters.floor && room.floor.toString() !== filters.floor) {
        return false;
      }

      // Branch filter
      if (filters.branchId && filters.branchId !== "all-branches" && room.branchId.toString() !== filters.branchId) {
        return false;
      }

      // Room Type filter
      if (filters.roomTypeId && filters.roomTypeId !== "all-types" && room.roomTypeId.toString() !== filters.roomTypeId) {
        return false;
      }

      return true;
    });
  }, [rooms, filters]);

  return (
    <div className="container mx-auto py-6 space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold">Rooms Management</h1>
          <p className="text-muted-foreground">Manage your hotel rooms and their status.</p>
        </div>
        <div className="flex items-center gap-4">
          <ConnectionStatus />
          <Button 
            onClick={() => navigate(ROUTES.ADD_ROOM)} 
            className="gap-2"
          >
            <Plus className="h-4 w-4" />
            Add Room
          </Button>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">
        <div className="lg:col-span-1">
          <RoomFilters 
            onFilterChange={handleFilterChange}
            onReset={handleResetFilters}
          />
        </div>
        
        <div className="lg:col-span-3 space-y-6">
          <RoomsListing 
            rooms={filteredRooms} 
            onRoomClick={handleRoomClick}
            isLoading={isLoading}
          />

          {!isLoading && (
            <Pagination
              currentPage={currentPage}
              pageSize={pageSize}
              totalPages={totalPages}
              totalCount={filteredRooms.length}
              hasPreviousPage={hasPreviousPage}
              hasNextPage={hasNextPage}
              pageSizeOptions={PAGE_SIZE_OPTIONS}
              onPageChange={handlePageChange}
              onPageSizeChange={handlePageSizeChange}
              itemName="rooms"
            />
          )}
        </div>
      </div>
    </div>
  );
};

export default Rooms;
