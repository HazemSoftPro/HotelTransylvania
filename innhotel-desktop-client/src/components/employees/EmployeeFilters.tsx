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
import { Position } from "@/types/employee";

export interface EmployeeFilterValues {
  name: string;
  position: string;
  branchId: string;
  hireDateFrom: string;
  hireDateTo: string;
}

interface EmployeeFiltersProps {
  onFilterChange: (filters: EmployeeFilterValues) => void;
  onReset: () => void;
}

const positions = Object.values(Position);

export const EmployeeFilters = ({ onFilterChange, onReset }: EmployeeFiltersProps) => {
  const [filters, setFilters] = useState<EmployeeFilterValues>({
    name: "",
    position: "",
    branchId: "",
    hireDateFrom: "",
    hireDateTo: "",
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

  const handleFilterChange = (key: keyof EmployeeFilterValues, value: string) => {
    const newFilters = { ...filters, [key]: value };
    setFilters(newFilters);
    onFilterChange(newFilters);
  };

  const handleReset = () => {
    const resetFilters: EmployeeFilterValues = {
      name: "",
      position: "",
      branchId: "",
      hireDateFrom: "",
      hireDateTo: "",
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
        {/* Name Search */}
        <div className="space-y-2">
          <Label htmlFor="name">Search Name</Label>
          <div className="relative">
            <Search className="absolute left-2 top-2.5 h-4 w-4 text-muted-foreground" />
            <Input
              id="name"
              placeholder="Search by name..."
              value={filters.name}
              onChange={(e) => handleFilterChange("name", e.target.value)}
              className="pl-8"
            />
          </div>
        </div>

        {/* Position Filter */}
        <div className="space-y-2">
          <Label htmlFor="position">Position</Label>
          <Select
            value={filters.position}
            onValueChange={(value) => handleFilterChange("position", value)}
          >
            <SelectTrigger id="position">
              <SelectValue placeholder="All positions" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="">All positions</SelectItem>
              {positions.map((position) => (
                <SelectItem key={position} value={position}>
                  {position}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
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

        {/* Hire Date Range */}
        <div className="space-y-2">
          <Label>Hire Date Range</Label>
          <div className="grid grid-cols-2 gap-2">
            <Input
              type="date"
              placeholder="From"
              value={filters.hireDateFrom}
              onChange={(e) => handleFilterChange("hireDateFrom", e.target.value)}
            />
            <Input
              type="date"
              placeholder="To"
              value={filters.hireDateTo}
              onChange={(e) => handleFilterChange("hireDateTo", e.target.value)}
            />
          </div>
        </div>
      </CardContent>
    </Card>
  );
};