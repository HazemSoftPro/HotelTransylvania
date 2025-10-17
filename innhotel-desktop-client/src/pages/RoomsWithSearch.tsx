import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import type { RoomResponse } from "@/types/api/room";
import { RoomsListing } from "@/components/rooms/RoomsListing";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import { ROUTES } from "@/constants/routes";
import { ConnectionStatus } from "@/components/ui/connection-status";
import { useRoomsStore } from "@/store/rooms.store";
import { SearchInput, FilterPanel, Pagination, EmptyState, ErrorState } from "@/components/search";
import { useSearch } from "@/hooks/useSearch";
import { searchRooms } from "@/services/searchService";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Input } from "@/components/ui/input";

const RoomsWithSearch = () => {
  const navigate = useNavigate();
  const { joinBranchGroup } = useRoomsStore();
  
  // Search state
  const [searchTerm, setSearchTerm] = useState("");
  const [filters, setFilters] = useState({
    branchId: undefined as number | undefined,
    roomTypeId: undefined as number | undefined,
    status: undefined as string | undefined,
    floor: undefined as number | undefined,
  });

  // Use search hook
  const {
    data: rooms,
    isLoading,
    error,
    pagination,
    search,
    setPage,
    setPageSize,
  } = useSearch<RoomResponse>({
    searchFn: searchRooms,
    entity: 'rooms',
    initialParams: {
      pageNumber: 1,
      pageSize: 10,
    },
  });

  // Join branch groups for real-time updates
  useEffect(() => {
    if (rooms.length > 0) {
      const branchIds = [...new Set(rooms.map(room => room.branchId))];
      branchIds.forEach(branchId => joinBranchGroup(branchId));
    }
  }, [rooms, joinBranchGroup]);

  // Handle search
  const handleSearch = (term: string) => {
    setSearchTerm(term);
    search({
      searchTerm: term,
      ...filters,
      pageNumber: 1,
    });
  };

  // Handle filter apply
  const handleApplyFilters = () => {
    search({
      searchTerm,
      ...filters,
      pageNumber: 1,
    });
  };

  // Handle filter reset
  const handleResetFilters = () => {
    setFilters({
      branchId: undefined,
      roomTypeId: undefined,
      status: undefined,
      floor: undefined,
    });
    search({
      searchTerm,
      pageNumber: 1,
    });
  };

  // Count active filters
  const activeFilterCount = Object.values(filters).filter(v => v !== undefined).length;

  const handleRoomClick = (room: RoomResponse) => {
    navigate(`${ROUTES.ROOMS}/${room.id}`);
  };

  const handleRetry = () => {
    search({ searchTerm, ...filters });
  };

  return (
    <div className="container mx-auto py-6 space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold">Rooms Management</h1>
          <p className="text-muted-foreground">Search and manage your hotel rooms.</p>
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

      {/* Search and Filter Bar */}
      <div className="flex items-center gap-4">
        <SearchInput
          placeholder="Search by room number or type..."
          onSearch={handleSearch}
          defaultValue={searchTerm}
          className="flex-1"
        />
        <FilterPanel
          title="Room Filters"
          description="Filter rooms by various criteria"
          filterCount={activeFilterCount}
          onApply={handleApplyFilters}
          onReset={handleResetFilters}
        >
          <div className="space-y-4">
            <div className="space-y-2">
              <Label>Status</Label>
              <Select
                value={filters.status}
                onValueChange={(value) => setFilters({ ...filters, status: value })}
              >
                <SelectTrigger>
                  <SelectValue placeholder="Select status" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="Available">Available</SelectItem>
                  <SelectItem value="Occupied">Occupied</SelectItem>
                  <SelectItem value="Maintenance">Maintenance</SelectItem>
                  <SelectItem value="OutOfService">Out of Service</SelectItem>
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label>Floor</Label>
              <Input
                type="number"
                placeholder="Enter floor number"
                value={filters.floor || ''}
                onChange={(e) => setFilters({ ...filters, floor: e.target.value ? Number(e.target.value) : undefined })}
              />
            </div>

            <div className="space-y-2">
              <Label>Branch ID</Label>
              <Input
                type="number"
                placeholder="Enter branch ID"
                value={filters.branchId || ''}
                onChange={(e) => setFilters({ ...filters, branchId: e.target.value ? Number(e.target.value) : undefined })}
              />
            </div>

            <div className="space-y-2">
              <Label>Room Type ID</Label>
              <Input
                type="number"
                placeholder="Enter room type ID"
                value={filters.roomTypeId || ''}
                onChange={(e) => setFilters({ ...filters, roomTypeId: e.target.value ? Number(e.target.value) : undefined })}
              />
            </div>
          </div>
        </FilterPanel>
      </div>

      {/* Error State */}
      {error && (
        <ErrorState
          message={error.message}
          onRetry={handleRetry}
        />
      )}

      {/* Empty State */}
      {!isLoading && !error && rooms.length === 0 && (
        <EmptyState
          title="No rooms found"
          description="Try adjusting your search or filters to find rooms."
          actionLabel="Clear Filters"
          onAction={handleResetFilters}
        />
      )}

      {/* Rooms Listing */}
      {!error && rooms.length > 0 && (
        <>
          <RoomsListing 
            rooms={rooms} 
            onRoomClick={handleRoomClick}
            isLoading={isLoading}
          />

          <Pagination
            currentPage={pagination.currentPage}
            totalPages={pagination.totalPages}
            pageSize={pagination.pageSize}
            totalItems={pagination.totalResults}
            onPageChange={setPage}
            onPageSizeChange={setPageSize}
          />
        </>
      )}

      {/* Loading State */}
      {isLoading && (
        <RoomsListing 
          rooms={[]} 
          onRoomClick={handleRoomClick}
          isLoading={true}
        />
      )}
    </div>
  );
};

export default RoomsWithSearch;