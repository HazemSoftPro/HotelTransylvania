import { HashRouter as Router } from 'react-router-dom';
import AppRoutes from '@/routes';
import { AuthProvider } from '@/context/AuthProvider';
import { Toaster } from 'sonner';
import SplashScreen from '@/components/ui/SplashScreen';
import { useSplashScreen } from '@/hooks/useSplashScreen';

const App = () => {
  const { showSplash, isAppReady, handleSplashComplete } = useSplashScreen({
    duration: 3000,
    minDisplayTime: 2000,
    skipOnDevelopment: false // Set to true if you want to skip splash in development
  });

  if (showSplash) {
    return <SplashScreen onComplete={handleSplashComplete} />;
  }

  if (!isAppReady) {
    return (
      <div className="flex items-center justify-center min-h-screen bg-gray-100">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto mb-4"></div>
          <p className="text-gray-600">جاري التحميل...</p>
        </div>
      </div>
    );
  }

  return (
    <Router>
      <AuthProvider>
        <AppRoutes />
      </AuthProvider>
      <Toaster position="top-center" richColors />
    </Router>
  );
};

export default App;
