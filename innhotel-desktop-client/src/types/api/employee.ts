import type { Pagination, UpdateResponse } from './global';

// Employee request for creation
export interface CreateEmployeeRequest {
  firstName: string;
  lastName: string;
  branchId: number;
  email?: string | null;
  phone?: string | null;
  hireDate: string; // DateOnly format: YYYY-MM-DD
  position: string;
  userId: string | null;
}

// Employee interface (with id)
export interface Employee {
  id: number;
  firstName: string;
  lastName: string;
  branchId: number;
  email?: string | null;
  phone?: string | null;
  hireDate: string; // DateOnly format: YYYY-MM-DD
  position: string;
  userId: string | null;
}

// Response of successful employee creation and getById
export interface EmployeeResponse {
  id: number;
  branchId: number;
  firstName: string;
  lastName: string;
  email?: string | null;
  phone?: string | null;
  hireDate: string;
  position: string;
  userId: string | null;
}

// Response of successful get all employees
export interface EmployeesResponse extends Pagination {
  items: EmployeeResponse[];
}

// Response of successful employee update
export type UpdateEmployeeResponse = UpdateResponse<EmployeeResponse>;