import { getUserEmailFromToken, isAuthenticated, logout } from '../../services/authService';

export function AuthHeader() {
  const email = isAuthenticated() ? getUserEmailFromToken() : null;

  return (
    <div className="mb-4 flex items-center justify-between rounded-3xl border border-zinc-800 bg-zinc-950 px-5 py-4 text-sm text-zinc-200">
      <div>
        <p className="text-xs uppercase tracking-[0.3em] text-spotify-green">Account</p>
        <p className="mt-1 text-sm text-white">{email ?? 'Guest user'}</p>
      </div>
      <button
        type="button"
        onClick={() => logout()}
        className="rounded-full border border-zinc-700 bg-zinc-900 px-3 py-2 text-xs uppercase text-zinc-300 transition hover:bg-zinc-800"
      >
        Logout
      </button>
    </div>
  );
}
