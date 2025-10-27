import { useAuthStore } from "@/store/auth.store";
import { useRoomsStore } from "@/store/rooms.store";
import { useMemo, useEffect, useRef } from "react";
import LoadingSpinner from "../components/Loader/LoadingSpinner";
import { logger } from '@/utils/logger';
import { authService } from "@/services/authService";
import { isAxiosError } from "axios";
import { AuthContext } from "./auth-context";

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const { setAuth, isLoading, setLoading, accessToken } = useAuthStore();
  const { initializeRealTimeConnection } = useRoomsStore();
  const log = logger();
  const refreshAttempted = useRef(false);
  const realTimeInitialized = useRef(false);

  useEffect(() => {
    const initializeAuth = async () => {
      // Prevent multiple refresh attempts
      if (refreshAttempted.current) {
        return;
      }
      refreshAttempted.current = true;

      try {
        log.info('Checking authentication state...');
        const { accessToken, email, roles } = await authService.refresh();

        log.info('Refresh token successful:', {
          isAuthenticated: true,
          user: { email, roles }
        });

        setAuth({
          accessToken,
          email,
          roles,
        });
      } catch (error) {
        log.error('Authentication failed:', {
          isAuthenticated: false,
          error: error instanceof Error ? error.message : 'Unknown error'
        });
        
        // Only clear auth if it's not a 401 (which means no refresh token)
        if (isAxiosError(error) && error.response?.status !== 401) {
          setAuth({
            accessToken: '',
            email: '',
            roles: [],
          });
        }
      } finally {
        setLoading(false);
      }
    };
    initializeAuth();
  }, [log, setAuth, setLoading]);

  // Initialize real-time connection when authenticated
  useEffect(() => {
    if (accessToken && !realTimeInitialized.current) {
      realTimeInitialized.current = true;
      
      const apiBaseUrl = import.meta.env.VITE_API_BASE_URL || 'http://localhost:57679/api';
      
      initializeRealTimeConnection(apiBaseUrl, () => accessToken);
      
      log.info('Real-time connection initialized');
    }
  }, [accessToken, initializeRealTimeConnection, log]);

  const contextValue = useMemo(() => {
    log.debug('Auth context value updated:', { isLoading });
    return { isLoading, setAuth };
  }, [isLoading, log, setAuth]);

  if (isLoading) {
    log.info('Rendering loading state');
    return (
      <LoadingSpinner />
    );
  }

  log.info('Rendering AuthProvider children');
  return (
    <AuthContext.Provider value={contextValue}>
      {children}
    </AuthContext.Provider>
  );
};
