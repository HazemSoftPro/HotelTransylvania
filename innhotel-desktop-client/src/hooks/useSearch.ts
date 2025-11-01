import { useState, useCallback, useEffect } from 'react';
import { useSearchStore } from '@/store/search.store';
import { toast } from 'sonner';

interface SearchParams {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  [key: string]: unknown;
}

interface SearchResponse<T> {
  data: T[];
  pagination: {
    pageNumber: number;
    pageSize: number;
    totalResults: number;
  };
}

interface UseSearchOptions<T> {
  searchFn: (params: SearchParams) => Promise<SearchResponse<T>>;
  entity: string;
  initialParams?: SearchParams;
  onSuccess?: (data: T[]) => void;
  onError?: (error: Error) => void;
  cacheResults?: boolean;
}

interface UseSearchReturn<T> {
  data: T[];
  isLoading: boolean;
  error: Error | null;
  pagination: {
    currentPage: number;
    pageSize: number;
    totalResults: number;
    totalPages: number;
  };
  search: (params: SearchParams) => Promise<void>;
  reset: () => void;
  setPage: (page: number) => void;
  setPageSize: (size: number) => void;
}

/**
 * useSearch Hook
 * 
 * A custom hook for managing search functionality with caching, pagination, and error handling.
 * 
 * @param options - Configuration options
 * @returns Search state and control functions
 */
export function useSearch<T>(options: UseSearchOptions<T>): UseSearchReturn<T> {
  const {
    searchFn,
    entity,
    initialParams = {},
    onSuccess,
    onError,
    cacheResults = true,
  } = options;

  const [data, setData] = useState<T[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);
  const [searchParams, setSearchParams] = useState(initialParams);
  const [pagination, setPagination] = useState({
    currentPage: 1,
    pageSize: 10,
    totalResults: 0,
    totalPages: 0,
  });

  const { cacheSearchResults, getCachedResults, addToSearchHistory } = useSearchStore();

  const search = useCallback(async (params: SearchParams) => {
    setIsLoading(true);
    setError(null);

    const fullParams = {
      ...searchParams,
      ...params,
      pageNumber: params.pageNumber || pagination.currentPage,
      pageSize: params.pageSize || pagination.pageSize,
    };

    // Generate cache key
    const cacheKey = `${entity}:${JSON.stringify(fullParams)}`;

    // Check cache first
    if (cacheResults) {
      const cached = getCachedResults(cacheKey) as SearchResponse<T> | null;
      if (cached) {
        setData(cached.data);
        setPagination({
          currentPage: cached.pagination.pageNumber,
          pageSize: cached.pagination.pageSize,
          totalResults: cached.pagination.totalResults,
          totalPages: Math.ceil(cached.pagination.totalResults / cached.pagination.pageSize),
        });
        setIsLoading(false);
        return;
      }
    }

    try {
      const response = await searchFn(fullParams);
      
      setData(response.data);
      setPagination({
        currentPage: response.pagination.pageNumber,
        pageSize: response.pagination.pageSize,
        totalResults: response.pagination.totalResults,
        totalPages: Math.ceil(response.pagination.totalResults / response.pagination.pageSize),
      });
      setSearchParams(fullParams);

      // Cache results
      if (cacheResults) {
        cacheSearchResults(cacheKey, response);
      }

      // Add to search history
      if (fullParams.searchTerm) {
        addToSearchHistory(entity, fullParams.searchTerm);
      }

      onSuccess?.(response.data);
    } catch (err) {
      const error = err as Error;
      setError(error);
      toast.error('Search failed', {
        description: error.message || 'An error occurred while searching',
      });
      onError?.(error);
    } finally {
      setIsLoading(false);
    }
  }, [searchFn, entity, searchParams, pagination, cacheResults, getCachedResults, cacheSearchResults, addToSearchHistory, onSuccess, onError]);

  const reset = useCallback(() => {
    setData([]);
    setSearchParams(initialParams);
    setPagination({
      currentPage: 1,
      pageSize: 10,
      totalResults: 0,
      totalPages: 0,
    });
    setError(null);
  }, [initialParams]);

  const setPage = useCallback((page: number) => {
    search({ ...searchParams, pageNumber: page });
  }, [search, searchParams]);

  const setPageSize = useCallback((size: number) => {
    search({ ...searchParams, pageSize: size, pageNumber: 1 });
  }, [search, searchParams]);

  // Initial search
  useEffect(() => {
    if (Object.keys(initialParams).length > 0) {
      search(initialParams);
    }
  }, []); // eslint-disable-line react-hooks/exhaustive-deps

  return {
    data,
    isLoading,
    error,
    pagination,
    search,
    reset,
    setPage,
    setPageSize,
  };
}