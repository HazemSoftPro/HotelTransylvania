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
import type { BranchResponse } from "@/types/api/branch";

export interface RoomTypeFilterValues {
  search: string;
  branchId: string;
  capacity: string;
}

interface RoomTypeFiltersProps {
  onFilterChange: (filters: RoomTypeFilterValues) => void;
  onReset: () => void;
}

export const RoomTypeFilters = ({ onFilterChange, onReset }: RoomTypeFiltersProps) => {
  const [filters, setFilters] = useState<RoomTypeFilterValues>({
    search: "",
    branchId: "",
    capacity: "",
  });

  const [branches, setBranches] = useState<BranchResponse[]>([]);
  const [isLoadingBranches, setIsLoadingBranches] = useState(true);

  useEffect(() => {
    const fetchBranches = async () => {
      try {
        const response = await branchService.getAll();
        setBranches(response.items);
      } catch (error) {
        console.error("Failed to fetch branches:", error);
      } finally {
        setIsLoadingBranches(false);
      }
    };

    fetchBranches();
  }, []);

  const handleFilterChange = (key: keyof RoomTypeFilterValues, value: string) => {
    const newFilters = { ...filters, [key]: value };
    setFilters(newFilters);
    onFilterChange(newFilters);
  };

  const handleReset = () => {
    const resetFilters: RoomTypeFilterValues = {
      search: "",
      branchId: "",
      capacity: "",
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
          <Label htmlFor="search">Search Name</Label>
          <div className="relative">
            <Search className="absolute left-2 top-2.5 h-4 w-4 text-muted-foreground" />
            <Input
              id="search"
              placeholder="Search by name..."
              value={filters.search}
              onChange={(e) => handleFilterChange("search", e.target.value)}
              className="pl-8"
            />
          </div>
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
              <SelectItem value="all">All branches</SelectItem>
              {branches.map((branch) => (
                <SelectItem key={branch.id} value={branch.id.toString()}>
                  {branch.name}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        {/* Capacity Filter */}
        <div className="space-y-2">
          <Label htmlFor="capacity">Capacity</Label>
          <Input
            id="capacity"
            type="number"
            placeholder="Filter by capacity..."
            value={filters.capacity}
            onChange={(e) => handleFilterChange("capacity", e.target.value)}
          />
        </div>
      </CardContent>
    </Card>
  );
};