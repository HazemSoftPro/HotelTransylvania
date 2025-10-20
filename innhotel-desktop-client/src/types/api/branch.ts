import type { Pagination, UpdateResponse } from './global';

// Branch interface for creation req and update
export interface Branch {
  name: string;
  location: string;
}

// res of successful Branch creation and getById
export interface BranchResponse extends Branch {
  id: number;
}

// res of successful get all branches
export interface BranchesResponse extends Pagination {
  data: BranchResponse[];
  items: BranchResponse[];
}

// res of successful branch update
export type UpdateBranchResponse = UpdateResponse<BranchResponse>;