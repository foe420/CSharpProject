import { Link } from 'react-router-dom';
import { useEffect } from 'react';
import { isAuthenticated } from '../../services/authService';
import { useNotificationStore } from '../../stores/useNotificationStore';

export function TopBar() {
  const userLoggedIn = isAuthenticated();
  const { unreadCount, fetchUnreadCount, setUnreadCount } = useNotificationStore();

  useEffect(() => {
    if (userLoggedIn) {
      fetchUnreadCount();
      const interval = setInterval(fetchUnreadCount, 30000);
      return () => clearInterval(interval);
    } else {
      setUnreadCount(0);
    }
  }, [userLoggedIn, fetchUnreadCount, setUnreadCount]);

  return (
    <div className="flex items-center justify-between border-b border-zinc-800 bg-[#0e0e0e] px-6 py-4 text-zinc-100">
      <div className="flex items-center gap-3">
        <Link to="/home" className="inline-flex h-11 w-11 items-center justify-center rounded-full border border-zinc-700 bg-zinc-950 text-zinc-200 transition hover:border-spotify-green hover:text-white">
          <span className="text-xl">🏠</span>
        </Link>
      </div>

      <div className="flex items-center justify-end gap-3">
        {userLoggedIn ? (
          <Link
            to="/profile"
            className="inline-flex h-11 w-11 items-center justify-center rounded-full border border-zinc-800 bg-zinc-950 text-zinc-200 transition hover:border-spotify-green hover:text-white"
            title="Profile"
          >
            👤
          </Link>
        ) : (
          <Link
            to="/login"
            className="rounded-full border border-spotify-green bg-spotify-green/10 px-4 py-2 text-sm font-semibold text-spotify-green transition hover:bg-spotify-green/20"
          >
            Login
          </Link>
        )}
        <Link
          to="/notifications"
          className="relative inline-flex h-11 w-11 items-center justify-center rounded-full border border-zinc-800 bg-zinc-950 text-zinc-200 transition hover:border-spotify-green hover:text-white"
          title="Notifications"
        >
          🔔
          {unreadCount > 0 && (
            <span className="absolute -top-1 -right-1 flex h-5 w-5 items-center justify-center rounded-full bg-spotify-green text-[10px] font-bold text-black shadow-lg shadow-black/50">
              {unreadCount}
            </span>
          )}
        </Link>
      </div>
    </div>
  );
}
