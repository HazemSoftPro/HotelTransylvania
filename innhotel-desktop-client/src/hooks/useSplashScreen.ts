import { useState, useEffect } from 'react';

interface UseSplashScreenOptions {
  duration?: number;
  minDisplayTime?: number;
  skipOnDevelopment?: boolean;
}

export const useSplashScreen = (options: UseSplashScreenOptions = {}) => {
  const {
    duration = 3000,
    minDisplayTime = 2000,
    skipOnDevelopment = false
  } = options;

  const [isLoading, setIsLoading] = useState(true);
  const [showSplash, setShowSplash] = useState(true);
  const [appReady, setAppReady] = useState(false);

  useEffect(() => {
    // Skip splash screen in development if specified
    if (skipOnDevelopment && import.meta.env.DEV) {
      setShowSplash(false);
      setAppReady(true);
      setIsLoading(false);
      return;
    }

    const startTime = Date.now();

    // Simulate app initialization
    const initializeApp = async () => {
      try {
        // Add any initialization logic here
        // For example: loading user preferences, checking auth, etc.
        await new Promise(resolve => setTimeout(resolve, 1000));
        
        setAppReady(true);
      } catch (error) {
        console.error('App initialization failed:', error);
        setAppReady(true); // Still proceed even if initialization fails
      }
    };

    initializeApp();

    // Ensure minimum display time
    const minTimer = setTimeout(() => {
      setIsLoading(false);
    }, minDisplayTime);

    return () => {
      clearTimeout(minTimer);
    };
  }, [minDisplayTime, skipOnDevelopment]);

  const handleSplashComplete = () => {
    setShowSplash(false);
  };

  return {
    showSplash: showSplash && (isLoading || !appReady),
    isAppReady: appReady && !showSplash,
    handleSplashComplete
  };
};

export default useSplashScreen;
