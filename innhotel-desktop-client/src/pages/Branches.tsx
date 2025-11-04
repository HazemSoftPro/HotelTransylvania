import { BranchesTable } from "@/components/branches/BranchesTable";
import { BranchFilters, type BranchFilterValues } from "@/components/branches/BranchFilters";
import type { BranchResponse } from "@/types/api/branch";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { ROUTES } from "@/constants/routes";
import { branchService } from "@/services/branchService";
import { useState, useEffect, useMemo } from "react";
import { RoleGuard } from "@/hooks/RoleGuard";
import { UserRole } from "@/types/api/user";
import { Pagination } from "@/components/pagination/Pagination";
import { PAGE_SIZE_OPTIONS } from "@/constants/pagination";

const Branches = () => {
  const navigate = useNavigate();
  const [branches, setBranches] = useState<BranchResponse[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalPages, setTotalPages] = useState(0);
  const [hasPreviousPage, setHasPreviousPage] = useState(false);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [filters, setFilters] = useState<BranchFilterValues>({
    name: "",
    location: "",
  });

  RoleGuard(UserRole.SuperAdmin);

  useEffect(() => {
    const fetchBranches = async () => {
      try {
        setIsLoading(true);
        const response = await branchService.getAll(currentPage, pageSize);
        setBranches(response.items);
        setTotalPages(response.totalPages);
        setHasPreviousPage(response.hasPreviousPage);
        setHasNextPage(response.hasNextPage);
      } catch (error) {
        console.error('Failed to fetch branches:', error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchBranches();
  }, [currentPage, pageSize]);

  const handleBranchClick = (branch: BranchResponse) => {
    navigate(`/branches/${branch.id}`);
  };

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
  };

  const handlePageSizeChange = (size: number) => {
    setPageSize(size);
    setCurrentPage(1);
  };

  const handleFilterChange = (newFilters: BranchFilterValues) => {
    setFilters(newFilters);
    setCurrentPage(1);
  };

  const handleResetFilters = () => {
    setFilters({
      name: "",
      location: "",
    });
    setCurrentPage(1);
  };

  // Apply client-side filtering
  const filteredBranches = useMemo(() => {
    return branches.filter((branch) => {
      // Name filter
      if (filters.name && !branch.name.toLowerCase().includes(filters.name.toLowerCase())) {
        return false;
      }

      // Location filter
      if (filters.location && !branch.location.toLowerCase().includes(filters.location.toLowerCase())) {
        return false;
      }

      return true;
    });
  }, [branches, filters]);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="space-y-1">
          <h1 className="text-2xl font-bold">Branches Management</h1>
          <p className="text-muted-foreground">
            View and manage branch information.
          </p>
        </div>
        <Button onClick={() => navigate(ROUTES.ADD_BRANCH)}>
          <Plus className="mr-2 h-4 w-4" />
          Add Branch
        </Button>
      </div>

      {isLoading ? (
        <div className="text-center text-muted-foreground">Loading branches...</div>
      ) : (
        <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">
          <div className="lg:col-span-1">
            <BranchFilters 
              onFilterChange={handleFilterChange}
              onReset={handleResetFilters}
            />
          </div>
          
          <div className="lg:col-span-3 space-y-6">
            <BranchesTable 
              branches={filteredBranches} 
              onBranchClick={handleBranchClick}
            />
            
            <Pagination
              currentPage={currentPage}
              pageSize={pageSize}
              totalPages={totalPages}
              totalCount={filteredBranches.length}
              hasPreviousPage={hasPreviousPage}
              hasNextPage={hasNextPage}
              pageSizeOptions={PAGE_SIZE_OPTIONS}
              onPageChange={handlePageChange}
              onPageSizeChange={handlePageSizeChange}
              itemName="branches"
            />
          </div>
        </div>
      )}
    </div>
  );
};

export default Branches;
