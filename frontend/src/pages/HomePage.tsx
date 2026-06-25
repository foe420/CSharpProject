import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { usePlayerStore } from '../stores/usePlayerStore';
import apiClient from '../services/apiClient';
import { isAuthenticated } from '../services/authService';

interface MediaItemDto {
  id: string;
  title: string;
  artist: string;
  duration: number;
  fileType: string;
  streamUrl: string;
}

interface PlayHistoryDto {
  mediaItemId: string;
  title: string;
  artist: string;
  duration: number;
  fileType: string;
  streamUrl: string;
  playedAt: string;
}

interface PlaylistDto {
  id: string;
  title: string;
  trackCount: number;
}

const formatDuration = (seconds: number) => {
  const minutes = Math.floor(seconds / 60);
  const remainder = Math.floor(seconds % 60);
  return `${minutes}:${remainder.toString().padStart(2, '0')}`;
};

export function HomePage() {
  const { playTrack } = usePlayerStore();
  const [trending, setTrending] = useState<MediaItemDto[]>([]);
  const [history, setHistory] = useState<PlayHistoryDto[]>([]);
  const [playlists, setPlaylists] = useState<PlaylistDto[]>([]);
  const [favCount, setFavCount] = useState<number | null>(null);
  const [loading, setLoading] = useState(true);

  const userLoggedIn = isAuthenticated();

  useEffect(() => {
    const fetchHomeData = async () => {
      setLoading(true);
      try {
        const trendingRes = await apiClient.get('/media/trending');
        setTrending(trendingRes.data || []);

        if (userLoggedIn) {
          const historyRes = await apiClient.get('/users/me/history');
          setHistory(historyRes.data || []);

          const playlistsRes = await apiClient.get('/users/me/playlists');
          setPlaylists(playlistsRes.data || []);

          const favoritesRes = await apiClient.get('/users/me/favorites');
          setFavCount(favoritesRes.data.totalCount ?? favoritesRes.data.items?.length ?? 0);
        }
      } catch (error) {
        console.error('Error fetching homepage data:', error);
      } finally {
        setLoading(false);
      }
    };
    fetchHomeData();
  }, [userLoggedIn]);

  const handlePlayTrending = (track: MediaItemDto) => {
    const baseUrl = import.meta.env.VITE_API_BASE_URL || 'https://localhost:5001/api';
    playTrack({
      id: track.id,
      title: track.title,
      artist: track.artist,
      duration: track.duration,
      src: `${baseUrl}/media/${track.id}/stream`,
      fileType: track.fileType.toLowerCase() as 'audio' | 'video'
    });
  };

  const handlePlayHistory = (track: PlayHistoryDto) => {
    const baseUrl = import.meta.env.VITE_API_BASE_URL || 'https://localhost:5001/api';
    playTrack({
      id: track.mediaItemId,
      title: track.title,
      artist: track.artist,
      duration: track.duration,
      src: `${baseUrl}/media/${track.mediaItemId}/stream`,
      fileType: track.fileType.toLowerCase() as 'audio' | 'video'
    });
  };

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

      {loading ? (
        <p className="text-zinc-400">Loading your feed...</p>
      ) : (
        <>
          <section className="space-y-4">
            <div className="flex items-center justify-between">
              <h2 className="text-2xl font-semibold">Trending Tracks</h2>
              <span className="text-sm text-zinc-500">Updated just now</span>
            </div>
            {trending.length === 0 ? (
              <p className="text-zinc-500">No trending tracks available.</p>
            ) : (
              <div className="grid gap-4 sm:grid-cols-3">
                {trending.slice(0, 6).map((track) => (
                  <article key={track.id} className="rounded-3xl border border-zinc-800 bg-zinc-950 p-5 shadow-sm shadow-black/20 transition hover:border-spotify-green">
                    <div className="flex items-start justify-between gap-4">
                      <div className="min-w-0 flex-1">
                        <p className="text-lg font-semibold text-white truncate">{track.title}</p>
                        <p className="mt-2 text-sm text-zinc-400 truncate">{track.artist}</p>
                      </div>
                      <button
                        type="button"
                        onClick={() => handlePlayTrending(track)}
                        className="rounded-full bg-spotify-green px-4 py-2 text-xs font-semibold uppercase text-black transition hover:brightness-110 flex-shrink-0"
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
            )}
          </section>

          <section className="grid gap-4 lg:grid-cols-[1fr_0.9fr]">
            {userLoggedIn ? (
              <>
                <div className="rounded-3xl border border-zinc-800 bg-zinc-950 p-6 flex flex-col justify-between">
                  <div>
                    <h2 className="text-2xl font-semibold">Your library highlights</h2>
                    <p className="mt-3 text-sm text-zinc-400">
                      Explore dynamic highlights from your playlists and saved music library.
                    </p>
                    <div className="mt-6 grid gap-3">
                      {favCount !== null && (
                        <Link to="/library" className="block rounded-2xl bg-zinc-900 p-4 hover:bg-zinc-800 transition">
                          <p className="text-sm text-zinc-400">Liked Songs</p>
                          <p className="mt-2 font-semibold text-white">❤️ {favCount} tracks favorited</p>
                        </Link>
                      )}
                      {playlists.slice(0, 2).map((playlist) => (
                        <Link key={playlist.id} to={`/playlist/${playlist.id}`} className="block rounded-2xl bg-zinc-900 p-4 hover:bg-zinc-800 transition">
                          <p className="text-sm text-zinc-400">Playlist</p>
                          <p className="mt-2 font-semibold text-white">🎵 {playlist.title} ({playlist.trackCount} tracks)</p>
                        </Link>
                      ))}
                      {playlists.length === 0 && favCount === 0 && (
                        <div className="rounded-2xl border border-dashed border-zinc-800 p-4 text-center text-zinc-500">
                          Create a playlist or favorite tracks to see highlights.
                        </div>
                      )}
                    </div>
                  </div>
                </div>

                <div className="rounded-3xl border border-zinc-800 bg-zinc-950 p-6">
                  <div className="flex items-center justify-between">
                    <h2 className="text-2xl font-semibold">Recently played</h2>
                    <span className="text-sm text-zinc-500">{history.length} items</span>
                  </div>
                  {history.length === 0 ? (
                    <div className="mt-6 text-center text-zinc-500 py-8">
                      No recently played tracks. Try playing some music!
                    </div>
                  ) : (
                    <ul className="mt-6 space-y-4">
                      {history.slice(0, 3).map((track) => (
                        <li key={track.mediaItemId + track.playedAt} className="flex items-center justify-between rounded-2xl border border-zinc-800 bg-zinc-900 p-4">
                          <div>
                            <p className="font-semibold text-white">{track.title}</p>
                            <p className="text-sm text-zinc-400">{track.artist}</p>
                          </div>
                          <button
                            type="button"
                            onClick={() => handlePlayHistory(track)}
                            className="rounded-full bg-zinc-800 px-4 py-2 text-sm text-zinc-200 transition hover:bg-zinc-700"
                          >
                            Replay
                          </button>
                        </li>
                      ))}
                    </ul>
                  )}
                </div>
              </>
            ) : (
              <div className="col-span-2 rounded-3xl border border-zinc-800 bg-zinc-950 p-8 text-center space-y-4">
                <h2 className="text-2xl font-bold text-white">Unlock your full listening experience</h2>
                <p className="max-w-md mx-auto text-zinc-400 text-sm">
                  Log in to save tracks to your library, customize playlists, view your play history, and share your favorite mixes with friends.
                </p>
                <Link
                  to="/login"
                  className="inline-block rounded-full bg-spotify-green px-8 py-3 text-sm font-bold text-black transition hover:scale-105"
                >
                  Log In Now
                </Link>
              </div>
            )}
          </section>
        </>
      )}
    </section>
  );
}
