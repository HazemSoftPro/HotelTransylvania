import { create } from 'zustand';
import { devtools } from 'zustand/middleware';
import type { AuthResponse } from '@/types/api/auth';

interface AuthState extends AuthResponse {
  isAuthenticated: boolean;
  isLoading: boolean;
}

interface AuthActions {
  setAuth: (auth: AuthResponse) => void;
  clearAuth: () => void;
  setLoading: (loading: boolean) => void;
}

type AuthStore = AuthState & AuthActions;

const initialState: AuthState = {
  accessToken: '',
  email: '',
  roles: [],
  isAuthenticated: false,
  isLoading: false,
};

export const useAuthStore = create<AuthStore>()(
  devtools(
    (set) => ({
      ...initialState,
      setAuth: (auth: AuthResponse) => 
        set({ 
          ...auth, 
          isAuthenticated: true 
        }, false, 'setAuth'),
      clearAuth: () => 
        set(initialState, false, 'clearAuth'),
      setLoading: (loading: boolean) => 
        set({ isLoading: loading }, false, 'setLoading'),
    }),
    { name: 'auth-store' }
  )
);
