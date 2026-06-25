import { useEffect, useState } from 'react';
import apiClient from '../services/apiClient';
import { useNotificationStore } from '../stores/useNotificationStore';

interface Notification {
  id: string;
  type: string;
  payloadJson: string;
  isRead: boolean;
  createdAt: string;
}

export function NotificationsPage() {
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [loading, setLoading] = useState(false);
  const { unreadCount, setUnreadCount, decrementUnreadCount, clearUnreadCount } = useNotificationStore();

  useEffect(() => {
    fetchNotifications();
  }, []);

  const fetchNotifications = async () => {
    setLoading(true);
    try {
      const res = await apiClient.get('/notifications');
      setNotifications(res.data.items || res.data || []);
      setUnreadCount(res.data.unreadCount || 0);
    } catch (error) {
      console.error('Notification load error:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleMarkAsRead = async (id: string) => {
    try {
      await apiClient.put(`/notifications/${id}/read`);
      setNotifications(
        notifications.map((n) =>
          n.id === id ? { ...n, isRead: true } : n
        )
      );
      decrementUnreadCount();
    } catch (error) {
      console.error('Mark as read error:', error);
    }
  };

  const handleMarkAllAsRead = async () => {
    try {
      await apiClient.put('/notifications/read-all');
      setNotifications(notifications.map((n) => ({ ...n, isRead: true })));
      clearUnreadCount();
    } catch (error) {
      console.error('Mark all as read error:', error);
    }
  };

  const getNotificationIcon = (type: string) => {
    switch (type?.toLowerCase()) {
      case 'share':
        return '📤';
      case 'follow':
        return '👤';
      case 'favorite':
        return '❤️';
      case 'comment':
        return '💬';
      default:
        return '🔔';
    }
  };

  const parsePayload = (json: string) => {
    try {
      return JSON.parse(json);
    } catch {
      return { message: json };
    }
  };

  const formatTime = (dateStr: string) => {
    const date = new Date(dateStr);
    const now = new Date();
    const diffMs = now.getTime() - date.getTime();
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMs / 3600000);
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffMins < 1) return 'Just now';
    if (diffMins < 60) return `${diffMins} minutes ago`;
    if (diffHours < 24) return `${diffHours} hours ago`;
    if (diffDays < 7) return `${diffDays} days ago`;
    return date.toLocaleDateString();
  };

  return (
    <div className="space-y-6 text-white">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold">Notifications</h1>
        {unreadCount > 0 && (
          <button
            onClick={handleMarkAllAsRead}
            className="rounded-full bg-spotify-green px-4 py-2 text-sm font-semibold text-black hover:bg-green-500 transition"
          >
            Mark all as read ({unreadCount})
          </button>
        )}
      </div>

      {loading ? (
        <p className="text-zinc-400">Loading notifications...</p>
      ) : notifications.length === 0 ? (
        <div className="rounded-2xl border border-zinc-800 bg-[#131313] p-8 text-center">
          <p className="text-zinc-500">No notifications</p>
        </div>
      ) : (
        <div className="space-y-3">
          {notifications.map((notif) => {
            const payload = parsePayload(notif.payloadJson);
            return (
              <div
                key={notif.id}
                className={`rounded-2xl border transition p-4 cursor-pointer hover:bg-zinc-900 ${
                  notif.isRead
                    ? 'border-zinc-800 bg-[#131313]'
                    : 'border-spotify-green bg-[#1a1a1a]'
                }`}
                onClick={() => !notif.isRead && handleMarkAsRead(notif.id)}
              >
                <div className="flex items-start gap-4">
                  <span className="text-2xl">{getNotificationIcon(notif.type)}</span>
                  <div className="flex-1">
                    <div className="flex items-start justify-between gap-4">
                      <div>
                        <p className="font-semibold text-white">
                          {payload.message || `Notification ${notif.type}`}
                        </p>
                        {payload.details && (
                          <p className="text-sm text-zinc-400 mt-1">{payload.details}</p>
                        )}
                      </div>
                      <div className="text-right flex-shrink-0">
                        <p className="text-xs text-zinc-500">
                          {formatTime(notif.createdAt)}
                        </p>
                        {!notif.isRead && (
                          <div className="mt-2 h-2 w-2 rounded-full bg-spotify-green"></div>
                        )}
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
}
