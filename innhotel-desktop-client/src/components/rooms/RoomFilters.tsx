import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Search, X } from "lucide-react";
import { branchService } from "@/services/branchService";
import { roomTypeService } from "@/services/roomTypeService";
import type { BranchResponse } from "@/types/api/branch";
import type { RoomType } from "@/services/roomTypeService";

export interface RoomFilterValues {
  search: string;
  status: string;
  floor: string;
  branchId: string;
  roomTypeId: string;
}

interface RoomFiltersProps {
  onFilterChange: (filters: RoomFilterValues) => void;
  onReset: () => void;
}

export const RoomFilters = ({ onFilterChange, onReset }: RoomFiltersProps) => {
  const [filters, setFilters] = useState<RoomFilterValues>({
    search: "",
    status: "",
    floor: "",
    branchId: "",
    roomTypeId: "",
  });

  const [branches, setBranches] = useState<BranchResponse[]>([]);
  const [roomTypes, setRoomTypes] = useState<RoomType[]>([]);
  const [isLoadingBranches, setIsLoadingBranches] = useState(true);
  const [isLoadingRoomTypes, setIsLoadingRoomTypes] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [branchesResponse, roomTypesResponse] = await Promise.all([
          branchService.getAll(),
          roomTypeService.getAll(),
        ]);
        setBranches(branchesResponse.items);
        setRoomTypes(roomTypesResponse.data);
      } catch (error) {
        console.error("Failed to fetch filter data:", error);
      } finally {
        setIsLoadingBranches(false);
        setIsLoadingRoomTypes(false);
      }
    };

    fetchData();
  }, []);

  const handleFilterChange = (key: keyof RoomFilterValues, value: string) => {
    const newFilters = { ...filters, [key]: value };
    setFilters(newFilters);
    onFilterChange(newFilters);
  };

  const handleReset = () => {
    const resetFilters: RoomFilterValues = {
      search: "",
      status: "",
      floor: "",
      branchId: "",
      roomTypeId: "",
    };
    setFilters(resetFilters);
    onReset();
  };

  const hasActiveFilters = Object.values(filters).some((value) => value !== "");

  return (
    <Card>
      <CardHeader>
        <div className="flex items-center justify-between">
          <CardTitle className="text-lg">Filters</CardTitle>
          {hasActiveFilters && (
            <Button
              variant="ghost"
              size="sm"
              onClick={handleReset}
              className="h-8 px-2 lg:px-3"
            >
              <X className="mr-2 h-4 w-4" />
              Clear Filters
            </Button>
          )}
        </div>
      </CardHeader>
      <CardContent className="space-y-4">
        {/* Search */}
        <div className="space-y-2">
          <Label htmlFor="search">Search Room Number</Label>
          <div className="relative">
            <Search className="absolute left-2 top-2.5 h-4 w-4 text-muted-foreground" />
            <Input
              id="search"
              placeholder="Search by room number..."
              value={filters.search}
              onChange={(e) => handleFilterChange("search", e.target.value)}
              className="pl-8"
            />
          </div>
        </div>

        {/* Status Filter */}
        <div className="space-y-2">
          <Label htmlFor="status">Status</Label>
          <Select
            value={filters.status}
            onValueChange={(value) => handleFilterChange("status", value)}
          >
            <SelectTrigger id="status">
              <SelectValue placeholder="All statuses" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="">All statuses</SelectItem>
              <SelectItem value="0">Available</SelectItem>
              <SelectItem value="1">Occupied</SelectItem>
              <SelectItem value="2">Under Maintenance</SelectItem>
            </SelectContent>
          </Select>
        </div>

        {/* Floor Filter */}
        <div className="space-y-2">
          <Label htmlFor="floor">Floor</Label>
          <Input
            id="floor"
            type="number"
            placeholder="Filter by floor..."
            value={filters.floor}
            onChange={(e) => handleFilterChange("floor", e.target.value)}
          />
        </div>

        {/* Branch Filter */}
        <div className="space-y-2">
          <Label htmlFor="branch">Branch</Label>
          <Select
            value={filters.branchId}
            onValueChange={(value) => handleFilterChange("branchId", value)}
            disabled={isLoadingBranches}
          >
            <SelectTrigger id="branch">
              <SelectValue
                placeholder={
                  isLoadingBranches ? "Loading branches..." : "All branches"
                }
              />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="">All branches</SelectItem>
              {branches.map((branch) => (
                <SelectItem key={branch.id} value={branch.id.toString()}>
                  {branch.name}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        {/* Room Type Filter */}
        <div className="space-y-2">
          <Label htmlFor="roomType">Room Type</Label>
          <Select
            value={filters.roomTypeId}
            onValueChange={(value) => handleFilterChange("roomTypeId", value)}
            disabled={isLoadingRoomTypes}
          >
            <SelectTrigger id="roomType">
              <SelectValue
                placeholder={
                  isLoadingRoomTypes ? "Loading room types..." : "All room types"
                }
              />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="">All room types</SelectItem>
              {roomTypes.map((roomType) => (
                <SelectItem key={roomType.id} value={roomType.id.toString()}>
                  {roomType.name}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>
      </CardContent>
    </Card>
  );
};