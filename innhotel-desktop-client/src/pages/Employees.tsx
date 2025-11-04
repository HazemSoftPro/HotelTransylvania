import { useState, useEffect, useMemo } from "react";
import { useNavigate } from "react-router-dom";
import { ROUTES } from "@/constants/routes";
import { EmployeesTable } from "@/components/employees/EmployeesTable";
import { EmployeeFilters, type EmployeeFilterValues } from "@/components/employees/EmployeeFilters";
import { Button } from "@/components/ui/button";
import { UserPlus } from "lucide-react";
import type { EmployeeResponse } from "@/types/api/employee";
import { employeeService } from "@/services/employeeService";
import { RoleGuard } from "@/hooks/RoleGuard";
import { UserRole } from "@/types/api/user";
import { Pagination } from "@/components/pagination/Pagination";
import { PAGE_SIZE_OPTIONS } from "@/constants/pagination";

const Employees = () => {
  RoleGuard(UserRole.SuperAdmin);
  const navigate = useNavigate();
  const [employees, setEmployees] = useState<EmployeeResponse[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalPages, setTotalPages] = useState(0);
  const [totalCount, setTotalCount] = useState(0);
  const [hasPreviousPage, setHasPreviousPage] = useState(false);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [filters, setFilters] = useState<EmployeeFilterValues>({
    name: "",
    position: "",
    branchId: "",
    hireDateFrom: "",
    hireDateTo: "",
  });

  useEffect(() => {
    const fetchEmployees = async () => {
      try {
        setIsLoading(true);
        const response = await employeeService.getAll(currentPage, pageSize);
        setEmployees(response.items);
        setTotalPages(response.totalPages);
        setTotalCount(response.totalCount);
        setHasPreviousPage(response.hasPreviousPage);
        setHasNextPage(response.hasNextPage);
      } catch (error) {
        console.error("Failed to fetch employees:", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchEmployees();
  }, [currentPage, pageSize]);

  const handleEmployeeClick = (employee: EmployeeResponse) => {
    navigate(`/employees/${employee.id}`);
  };

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
  };

  const handlePageSizeChange = (size: number) => {
    setPageSize(size);
    setCurrentPage(1);
  };

  const handleFilterChange = (newFilters: EmployeeFilterValues) => {
    setFilters(newFilters);
    setCurrentPage(1);
  };

  const handleResetFilters = () => {
    setFilters({
      name: "",
      position: "",
      branchId: "",
      hireDateFrom: "",
      hireDateTo: "",
    });
    setCurrentPage(1);
  };

  // Apply client-side filtering
  const filteredEmployees = useMemo(() => {
    return employees.filter((employee) => {
      // Name filter
      if (filters.name) {
        const fullName = `${employee.firstName} ${employee.lastName}`.toLowerCase();
        if (!fullName.includes(filters.name.toLowerCase())) {
          return false;
        }
      }

      // Position filter
      if (filters.position && employee.position !== filters.position) {
        return false;
      }

      // Branch filter
      if (filters.branchId && employee.branchId.toString() !== filters.branchId) {
        return false;
      }

      // Hire date range filter
      if (filters.hireDateFrom && employee.hireDate < filters.hireDateFrom) {
        return false;
      }

      if (filters.hireDateTo && employee.hireDate > filters.hireDateTo) {
        return false;
      }

      return true;
    });
  }, [employees, filters]);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="space-y-1">
          <h1 className="text-2xl font-bold">Employees Management</h1>
          <p className="text-muted-foreground">
            View and manage employee information.
          </p>
        </div>
        <Button onClick={() => navigate(ROUTES.REGISTER_EMPLOYEE)}>
          <UserPlus className="mr-2 h-4 w-4" />
          Register Employee
        </Button>
      </div>

      {isLoading ? (
        <div className="text-center text-muted-foreground">Loading employees...</div>
      ) : (
        <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">
          <div className="lg:col-span-1">
            <EmployeeFilters 
              onFilterChange={handleFilterChange}
              onReset={handleResetFilters}
            />
          </div>
          
          <div className="lg:col-span-3 space-y-6">
            <EmployeesTable 
              employees={filteredEmployees}
              onEmployeeClick={handleEmployeeClick}
            />
            
            <Pagination
              currentPage={currentPage}
              pageSize={pageSize}
              totalPages={totalPages}
              totalCount={filteredEmployees.length}
              hasPreviousPage={hasPreviousPage}
              hasNextPage={hasNextPage}
              pageSizeOptions={PAGE_SIZE_OPTIONS}
              onPageChange={handlePageChange}
              onPageSizeChange={handlePageSizeChange}
              itemName="employees"
            />
          </div>
        </div>
      )}
    </div>
  );
};

export default Employees;
