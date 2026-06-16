import { Link } from 'react-router-dom';

export function LandingPage() {
  return (
    <div className="flex min-h-screen flex-col items-center justify-center bg-black px-6 py-16 text-white">
      <div className="mx-auto max-w-4xl text-center">
        <h1 className="text-5xl font-bold tracking-tight text-white sm:text-6xl">TuneVault</h1>
        <p className="mx-auto mt-6 max-w-2xl text-lg leading-8 text-zinc-300">
          Discover music, podcasts, and videos in a sleek, modern experience. Sign in or register to start saving your favorites and enjoying your personal audio library.
        </p>
        <div className="mt-10 flex flex-col items-center justify-center gap-4 sm:flex-row">
          <Link
            to="/login"
            className="inline-flex rounded-full bg-spotify-green px-8 py-3 text-sm font-semibold text-black transition hover:brightness-110"
          >
            Login
          </Link>
          <Link
            to="/register"
            className="inline-flex rounded-full border border-zinc-700 bg-zinc-900 px-8 py-3 text-sm font-semibold text-white transition hover:border-spotify-green hover:text-spotify-green"
          >
            Register
          </Link>
        </div>
      </div>

      <div className="mt-16 grid gap-6 sm:grid-cols-3">
        <div className="rounded-3xl border border-zinc-800 bg-zinc-950 p-6 text-left">
          <h2 className="text-xl font-semibold text-white">Personal DJ</h2>
          <p className="mt-3 text-sm text-zinc-400">Stream audio and video, create playlists, and keep your favorites ready.</p>
        </div>
        <div className="rounded-3xl border border-zinc-800 bg-zinc-950 p-6 text-left">
          <h2 className="text-xl font-semibold text-white">Curated experience</h2>
          <p className="mt-3 text-sm text-zinc-400">A clean UI built for modern music discovery and playback.</p>
        </div>
        <div className="rounded-3xl border border-zinc-800 bg-zinc-950 p-6 text-left">
          <h2 className="text-xl font-semibold text-white">Instant access</h2>
          <p className="mt-3 text-sm text-zinc-400">Sign in to access your library, playlists, and notifications.</p>
        </div>
      </div>
    </div>
  );
}
