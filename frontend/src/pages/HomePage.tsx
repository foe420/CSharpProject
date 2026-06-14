import { type PlayerTrack, usePlayerStore } from '../stores/usePlayerStore';

const sampleTracks: PlayerTrack[] = [
  {
    id: '1',
    title: 'Summer Groove',
    artist: 'TuneVault Collective',
    duration: 198,
    src: 'https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3',
    fileType: 'audio'
  },
  {
    id: '2',
    title: 'Late Night Drive',
    artist: 'Neon Beats',
    duration: 242,
    src: 'https://www.soundhelix.com/examples/mp3/SoundHelix-Song-2.mp3',
    fileType: 'audio'
  },
  {
    id: '3',
    title: 'City Lights',
    artist: 'Vanity Pulse',
    duration: 215,
    src: 'https://www.soundhelix.com/examples/mp3/SoundHelix-Song-3.mp3',
    fileType: 'audio'
  }
];

const formatDuration = (seconds: number) => {
  const minutes = Math.floor(seconds / 60);
  const remainder = Math.floor(seconds % 60);
  return `${minutes}:${remainder.toString().padStart(2, '0')}`;
};

export function HomePage() {
  const { playTrack } = usePlayerStore();

  return (
    <section className="space-y-8">
      <header className="space-y-3">
        <div className="flex flex-col gap-2 sm:flex-row sm:items-end sm:justify-between">
          <div>
            <p className="text-sm uppercase tracking-[0.2em] text-spotify-green">Welcome back</p>
            <h1 className="text-4xl font-bold text-white">Good morning, and have a great listening session.</h1>
          </div>
        </div>
        <p className="max-w-2xl text-sm text-zinc-400">
          Your player is ready. Click a track below to instantly preview through the bottom audio bar.
        </p>
      </header>

      <section className="space-y-4">
        <div className="flex items-center justify-between">
          <h2 className="text-2xl font-semibold">Trending Tracks</h2>
          <span className="text-sm text-zinc-500">Updated just now</span>
        </div>
        <div className="grid gap-4 sm:grid-cols-3">
          {sampleTracks.map((track) => (
            <article key={track.id} className="rounded-3xl border border-zinc-800 bg-zinc-950 p-5 shadow-sm shadow-black/20 transition hover:border-spotify-green">
              <div className="flex items-start justify-between gap-4">
                <div>
                  <p className="text-lg font-semibold text-white">{track.title}</p>
                  <p className="mt-2 text-sm text-zinc-400">{track.artist}</p>
                </div>
                <button
                  type="button"
                  onClick={() => playTrack(track)}
                  className="rounded-full bg-spotify-green px-4 py-2 text-xs font-semibold uppercase text-black transition hover:brightness-110"
                >
                  Play
                </button>
              </div>
              <div className="mt-6 flex items-center justify-between text-sm text-zinc-400">
                <span>{track.fileType.toUpperCase()}</span>
                <span>{formatDuration(track.duration)}</span>
              </div>
            </article>
          ))}
        </div>
      </section>

      <section className="grid gap-4 lg:grid-cols-[1fr_0.9fr]">
        <div className="rounded-3xl border border-zinc-800 bg-zinc-950 p-6">
          <h2 className="text-2xl font-semibold">Your library highlights</h2>
          <p className="mt-3 text-sm text-zinc-400">
            Explore recommended mixes, saved playlists, and recently played content from your TuneVault experience.
          </p>
          <div className="mt-6 grid gap-3">
            <div className="rounded-2xl bg-zinc-900 p-4">
              <p className="text-sm text-zinc-400">Liked playlist</p>
              <p className="mt-2 font-semibold text-white">Midnight Jazz Sessions</p>
            </div>
            <div className="rounded-2xl bg-zinc-900 p-4">
              <p className="text-sm text-zinc-400">Recommended artist</p>
              <p className="mt-2 font-semibold text-white">Neon Beats</p>
            </div>
          </div>
        </div>

        <div className="rounded-3xl border border-zinc-800 bg-zinc-950 p-6">
          <div className="flex items-center justify-between">
            <h2 className="text-2xl font-semibold">Recently played</h2>
            <span className="text-sm text-zinc-500">3 items</span>
          </div>
          <ul className="mt-6 space-y-4">
            {sampleTracks.map((track) => (
              <li key={track.id} className="flex items-center justify-between rounded-2xl border border-zinc-800 bg-zinc-900 p-4">
                <div>
                  <p className="font-semibold text-white">{track.title}</p>
                  <p className="text-sm text-zinc-400">{track.artist}</p>
                </div>
                <button
                  type="button"
                  onClick={() => playTrack(track)}
                  className="rounded-full bg-zinc-800 px-4 py-2 text-sm text-zinc-200 transition hover:bg-zinc-700"
                >
                  Replay
                </button>
              </li>
            ))}
          </ul>
        </div>
      </section>
    </section>
  );
}
