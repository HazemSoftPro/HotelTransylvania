import { useState } from "react";
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

export interface GuestFilterValues {
  name: string;
  phone: string;
  email: string;
  gender: string;
  idProofType: string;
}

interface GuestFiltersProps {
  onFilterChange: (filters: GuestFilterValues) => void;
  onReset: () => void;
}

export const GuestFilters = ({ onFilterChange, onReset }: GuestFiltersProps) => {
  const [filters, setFilters] = useState<GuestFilterValues>({
    name: "",
    phone: "",
    email: "",
    gender: "",
    idProofType: "",
  });

  const handleFilterChange = (key: keyof GuestFilterValues, value: string) => {
    const newFilters = { ...filters, [key]: value };
    setFilters(newFilters);
    onFilterChange(newFilters);
  };

  const handleReset = () => {
    const resetFilters: GuestFilterValues = {
      name: "",
      phone: "",
      email: "",
      gender: "",
      idProofType: "",
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

        {/* Phone Search */}
        <div className="space-y-2">
          <Label htmlFor="phone">Phone</Label>
          <Input
            id="phone"
            placeholder="Search by phone..."
            value={filters.phone}
            onChange={(e) => handleFilterChange("phone", e.target.value)}
          />
        </div>

        {/* Email Search */}
        <div className="space-y-2">
          <Label htmlFor="email">Email</Label>
          <Input
            id="email"
            placeholder="Search by email..."
            value={filters.email}
            onChange={(e) => handleFilterChange("email", e.target.value)}
          />
        </div>

        {/* Gender Filter */}
        <div className="space-y-2">
          <Label htmlFor="gender">Gender</Label>
          <Select
            value={filters.gender}
            onValueChange={(value) => handleFilterChange("gender", value)}
          >
            <SelectTrigger id="gender">
              <SelectValue placeholder="All genders" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="all">All genders</SelectItem>
              <SelectItem value="Male">Male</SelectItem>
              <SelectItem value="Female">Female</SelectItem>
            </SelectContent>
          </Select>
        </div>

        {/* ID Proof Type Filter */}
        <div className="space-y-2">
          <Label htmlFor="idProofType">ID Proof Type</Label>
          <Select
            value={filters.idProofType}
            onValueChange={(value) => handleFilterChange("idProofType", value)}
          >
            <SelectTrigger id="idProofType">
              <SelectValue placeholder="All ID types" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="all">All ID types</SelectItem>
              <SelectItem value="Passport">Passport</SelectItem>
              <SelectItem value="DriverLicense">Driver's License</SelectItem>
              <SelectItem value="NationalId">National ID</SelectItem>
            </SelectContent>
          </Select>
        </div>
      </CardContent>
    </Card>
  );
};