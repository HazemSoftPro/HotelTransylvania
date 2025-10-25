import { create } from 'zustand';

interface CachedSearchResult {
  data: unknown;
  timestamp: number;
}

interface SearchFilters {
  [key: string]: unknown;
}

interface SearchState {
  // Search history
  searchHistory: Record<string, string[]>;
  
  // Cached search results
  searchCache: Record<string, CachedSearchResult>;
  
  // Active filters
  activeFilters: Record<string, SearchFilters>;
  
  // Actions
  addToSearchHistory: (entity: string, term: string) => void;
  getSearchHistory: (entity: string) => string[];
  clearSearchHistory: (entity: string) => void;
  
  cacheSearchResults: (key: string, results: unknown) => void;
  getCachedResults: (key: string) => unknown;
  clearSearchCache: () => void;
  
  setActiveFilters: (entity: string, filters: SearchFilters) => void;
  getActiveFilters: (entity: string) => SearchFilters;
  clearActiveFilters: (entity: string) => void;
}

/**
 * Search Store
 * 
 * Global state management for search functionality including:
 * - Search history tracking
 * - Search result caching
 * - Active filter state
 */
export const useSearchStore = create<SearchState>((set, get) => ({
  searchHistory: {},
  searchCache: {},
  activeFilters: {},

  addToSearchHistory: (entity: string, term: string) => {
    if (!term.trim()) return;
    
    set((state) => {
      const history = state.searchHistory[entity] || [];
      const updatedHistory = [term, ...history.filter(t => t !== term)].slice(0, 10);
      
      return {
        searchHistory: {
          ...state.searchHistory,
          [entity]: updatedHistory,
        },
      };
    });
  },

  getSearchHistory: (entity: string) => {
    return get().searchHistory[entity] || [];
  },

  clearSearchHistory: (entity: string) => {
    set((state) => {
      // eslint-disable-next-line @typescript-eslint/no-unused-vars
      const { [entity]: _, ...rest } = state.searchHistory;
      return { searchHistory: rest };
    });
  },

  cacheSearchResults: (key: string, results: unknown) => {
    set((state) => ({
      searchCache: {
        ...state.searchCache,
        [key]: {
          data: results,
          timestamp: Date.now(),
        },
      },
    }));
  },

  getCachedResults: (key: string) => {
    const cached = get().searchCache[key];
    if (!cached) return null;
    
    // Cache expires after 5 minutes
    const isExpired = Date.now() - cached.timestamp > 5 * 60 * 1000;
    if (isExpired) {
      set((state) => {
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        const { [key]: _, ...rest } = state.searchCache;
        return { searchCache: rest };
      });
      return null;
    }
    
    return cached.data;
  },

  clearSearchCache: () => {
    set({ searchCache: {} });
  },

  setActiveFilters: (entity: string, filters: SearchFilters) => {
    set((state) => ({
      activeFilters: {
        ...state.activeFilters,
        [entity]: filters,
      },
    }));
  },

  getActiveFilters: (entity: string) => {
    return get().activeFilters[entity] || {};
  },

  clearActiveFilters: (entity: string) => {
    set((state) => {
      // eslint-disable-next-line @typescript-eslint/no-unused-vars
      const { [entity]: _, ...rest } = state.activeFilters;
      return { activeFilters: rest };
    });
  },
}));