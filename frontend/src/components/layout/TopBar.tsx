import { Link } from 'react-router-dom';

export function TopBar() {
  return (
    <div className="grid grid-cols-1 gap-4 border-b border-zinc-800 bg-[#0e0e0e] px-6 py-4 text-zinc-100 sm:grid-cols-[auto_1fr_auto] sm:items-center">
      <div className="flex items-center gap-3">
        <Link to="/app/home" className="inline-flex h-11 w-11 items-center justify-center rounded-full border border-zinc-700 bg-zinc-950 text-zinc-200 transition hover:border-spotify-green hover:text-white">
          <span className="text-xl">🏠</span>
        </Link>
      </div>

      <div className="flex justify-center">
        <div className="w-full max-w-xl rounded-full border border-zinc-800 bg-zinc-950 px-4 py-2 text-zinc-300 shadow-black/20 sm:flex sm:items-center">
          <span className="mr-3 text-zinc-500">🔍</span>
          <input
            type="search"
            placeholder="Bạn muốn phát nội dung gì?"
            className="w-full bg-transparent text-sm text-white outline-none placeholder:text-zinc-500"
          />
        </div>
      </div>

      <div className="flex items-center justify-end gap-3">
        <Link
          to="/login"
          className="rounded-full border border-spotify-green bg-spotify-green/10 px-4 py-2 text-sm font-semibold text-spotify-green transition hover:bg-spotify-green/20"
        >
          Login
        </Link>
        <button className="rounded-full border border-zinc-800 bg-zinc-950 p-3 text-zinc-200 transition hover:border-spotify-green hover:text-white">
          🔔
        </button>
      </div>
    </div>
  );
}
