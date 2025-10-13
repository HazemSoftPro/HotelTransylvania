import { useRoomsStore } from '@/store/rooms.store';
import { cn } from '@/lib/utils';
import { Wifi, WifiOff, AlertCircle } from 'lucide-react';

interface ConnectionStatusProps {
  className?: string;
  showText?: boolean;
}

export function ConnectionStatus({ className, showText = true }: ConnectionStatusProps) {
  const { isConnected, connectionError } = useRoomsStore();

  const getStatusIcon = () => {
    if (connectionError) {
      return <AlertCircle className="h-4 w-4 text-red-500" />;
    }
    return isConnected ? (
      <Wifi className="h-4 w-4 text-green-500" />
    ) : (
      <WifiOff className="h-4 w-4 text-gray-400" />
    );
  };

  const getStatusText = () => {
    if (connectionError) {
      return 'Connection Error';
    }
    return isConnected ? 'Connected' : 'Disconnected';
  };

  const getStatusColor = () => {
    if (connectionError) {
      return 'text-red-500';
    }
    return isConnected ? 'text-green-500' : 'text-gray-400';
  };

  return (
    <div 
      className={cn(
        'flex items-center gap-2',
        className
      )}
      title={connectionError || getStatusText()}
    >
      {getStatusIcon()}
      {showText && (
        <span className={cn('text-sm font-medium', getStatusColor())}>
          {getStatusText()}
        </span>
      )}
    </div>
  );
}
