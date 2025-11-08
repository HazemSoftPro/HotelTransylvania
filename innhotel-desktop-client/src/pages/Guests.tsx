import { useNavigate } from "react-router-dom";
import { Plus } from "lucide-react";
import { useState, useEffect, useMemo } from "react";

import { GuestsTable } from "@/components/guests/GuestsTable";
import { GuestFilters, type GuestFilterValues } from "@/components/guests/GuestFilters";
import type { GuestResponse } from "@/types/api/guest";
import { guestService } from "@/services/guestService";
import { ROUTES } from "@/constants/routes";
import { Button } from "@/components/ui/button";
import { Pagination } from "@/components/pagination/Pagination";
import { PAGE_SIZE_OPTIONS } from "@/constants/pagination";

const Guests = () => {
  const navigate = useNavigate();
  const [guests, setGuests] = useState<GuestResponse[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(5);
  const [totalPages, setTotalPages] = useState(0);
  const [hasPreviousPage, setHasPreviousPage] = useState(false);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [filters, setFilters] = useState<GuestFilterValues>({
    name: "",
    phone: "",
    email: "",
    gender: "",
    idProofType: "",
  });

  useEffect(() => {
    const fetchGuests = async () => {
      try {
        setIsLoading(true);
        const response = await guestService.getAll(currentPage, pageSize);
        setGuests(response.items);
        setTotalPages(response.totalPages);
        setHasPreviousPage(response.hasPreviousPage);
        setHasNextPage(response.hasNextPage);
      } catch (error) {
        console.error("Failed to fetch guests:", error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchGuests();
  }, [currentPage, pageSize]);

  const handleGuestClick = (guest: GuestResponse) => {
    navigate(`${ROUTES.GUESTS}/${guest.id}`);
  };

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
  };

  const handlePageSizeChange = (size: number) => {
    setPageSize(size);
    setCurrentPage(1);
  };

  const handleFilterChange = (newFilters: GuestFilterValues) => {
    setFilters(newFilters);
    setCurrentPage(1);
  };

  const handleResetFilters = () => {
    setFilters({
      name: "",
      phone: "",
      email: "",
      gender: "",
      idProofType: "",
    });
    setCurrentPage(1);
  };

  // Apply client-side filtering
  const filteredGuests = useMemo(() => {
    return guests.filter((guest) => {
      // Name filter
      if (filters.name) {
        const fullName = `${guest.firstName} ${guest.lastName}`.toLowerCase();
        if (!fullName.includes(filters.name.toLowerCase())) {
          return false;
        }
      }

      // Phone filter
      if (filters.phone && guest.phone && !guest.phone.includes(filters.phone)) {
        return false;
      }

      // Email filter
      if (filters.email && guest.email && !guest.email.toLowerCase().includes(filters.email.toLowerCase())) {
        return false;
      }

      // Gender filter
      if (filters.gender && filters.gender !== "all" && guest.gender !== filters.gender) {
        return false;
      }

      // ID Proof Type filter
      if (filters.idProofType && filters.idProofType !== "all" && guest.idProofType !== filters.idProofType) {
        return false;
      }

      return true;
    });
  }, [guests, filters]);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="space-y-1">
          <h1 className="text-2xl font-bold">Guests Management</h1>
          <p className="text-muted-foreground">
            View and manage guest information.
          </p>
        </div>
        <Button onClick={() => navigate(ROUTES.ADD_GUEST)}>
          <Plus className="mr-2 h-4 w-4" />
          Add Guest
        </Button>
      </div>

      {isLoading ? (
        <div className="text-center text-muted-foreground">
          Loading guests...
        </div>
      ) : (
        <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">
          <div className="lg:col-span-1">
            <GuestFilters 
              onFilterChange={handleFilterChange}
              onReset={handleResetFilters}
            />
          </div>
          
          <div className="lg:col-span-3 space-y-6">
            <GuestsTable
              guests={filteredGuests}
              onGuestClick={handleGuestClick}
            />
            
            <Pagination
              currentPage={currentPage}
              pageSize={pageSize}
              totalPages={totalPages}
              totalCount={filteredGuests.length}
              hasPreviousPage={hasPreviousPage}
              hasNextPage={hasNextPage}
              pageSizeOptions={PAGE_SIZE_OPTIONS}
              onPageChange={handlePageChange}
              onPageSizeChange={handlePageSizeChange}
              itemName="guests"
            />
          </div>
        </div>
      )}
    </div>
  );
};

export default Guests;
