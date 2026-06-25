import { Link } from 'react-router-dom';
import { isAuthenticated } from '../../services/authService';

export function TopBar() {
  const userLoggedIn = isAuthenticated();

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
        <button className="rounded-full border border-zinc-800 bg-zinc-950 p-3 text-zinc-200 transition hover:border-spotify-green hover:text-white">
          🔔
        </button>
      </div>
    </div>
  );
}
