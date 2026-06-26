import { useEffect, useRef } from 'react';
import { HubConnectionBuilder, HubConnection, LogLevel } from '@microsoft/signalr';
import { getToken, isAuthenticated } from '../services/authService';
import { useNotificationStore } from '../stores/useNotificationStore';

export function useSignalR() {
  const connectionRef = useRef<HubConnection | null>(null);
  const { setUnreadCount } = useNotificationStore();
  const authenticated = isAuthenticated();

  useEffect(() => {
    if (!authenticated) {
      if (connectionRef.current) {
        void connectionRef.current.stop();
        connectionRef.current = null;
      }
      return;
    }

    const token = getToken();
    if (!token) return;

    const apiBaseUrl = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5237/api';
    const hubUrl = apiBaseUrl.replace(/\/api$/, '') + '/hubs/notifications';

    const connection = new HubConnectionBuilder()
      .withUrl(hubUrl, {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    connection.on('ReceiveNotification', (notification: any) => {
      console.log('Real-time notification received:', notification);
      
      // Increment unread count
      setUnreadCount(useNotificationStore.getState().unreadCount + 1);

      // Dispatch custom event so pages can listen and update instantly
      window.dispatchEvent(new CustomEvent('new-notification', { detail: notification }));
    });

    connection.start()
      .then(() => {
        console.log('SignalR Notification Hub connected.');
        connectionRef.current = connection;
      })
      .catch((err) => {
        console.error('SignalR Connection Error: ', err);
      });

    return () => {
      if (connectionRef.current) {
        void connectionRef.current.stop();
        connectionRef.current = null;
      }
    };
  }, [authenticated, setUnreadCount]);
}
