import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Search, X } from "lucide-react";

export interface BranchFilterValues {
  name: string;
  location: string;
}

interface BranchFiltersProps {
  onFilterChange: (filters: BranchFilterValues) => void;
  onReset: () => void;
}

export const BranchFilters = ({ onFilterChange, onReset }: BranchFiltersProps) => {
  const [filters, setFilters] = useState<BranchFilterValues>({
    name: "",
    location: "",
  });

  const handleFilterChange = (key: keyof BranchFilterValues, value: string) => {
    const newFilters = { ...filters, [key]: value };
    setFilters(newFilters);
    onFilterChange(newFilters);
  };

  const handleReset = () => {
    const resetFilters: BranchFilterValues = {
      name: "",
      location: "",
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

        {/* Location Search */}
        <div className="space-y-2">
          <Label htmlFor="location">Location</Label>
          <Input
            id="location"
            placeholder="Search by location..."
            value={filters.location}
            onChange={(e) => handleFilterChange("location", e.target.value)}
          />
        </div>
      </CardContent>
    </Card>
  );
};