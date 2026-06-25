import { useState } from 'react';
import { usePlayerStore } from '../../stores/usePlayerStore';
import type { PlayerTrack } from '../../stores/usePlayerStore';
import apiClient from '../../services/apiClient';
import type { MediaItemSummaryDto } from '../../types';
import { isAuthenticated } from '../../services/authService';

interface Props {
  item: MediaItemSummaryDto;
}

export default function MediaItemCard({ item }: Props) {
  const { playTrack } = usePlayerStore();
  const [isFav, setIsFav] = useState(item.isFavorited || false);
  const [playlists, setPlaylists] = useState<Array<{ id: string; title: string }>>([]);
  const [isFetched, setIsFetched] = useState(false);

  const formatDuration = (seconds: number) => {
    const m = Math.floor(seconds / 60);
    const s = Math.floor(seconds % 60);
    return `${m}:${s < 10 ? '0' : ''}${s}`;
  };

  const fetchPlaylists = async () => {
    if (!isAuthenticated() || isFetched) return;
    try {
      const res = await apiClient.get('/users/me/playlists');
      setPlaylists(res.data || []);
      setIsFetched(true);
    } catch (error) {
      console.error("Error fetching playlists for dropdown", error);
    }
  };

  const handlePlay = () => {
    apiClient.post(`/media/${item.id}/play`).catch(console.error);
    const baseUrl = import.meta.env.VITE_API_BASE_URL || 'https://localhost:5001/api';
    const trackToPlay: PlayerTrack = {
      id: item.id,
      title: item.title,
      artist: item.artist,
      duration: item.duration,
      src: `${baseUrl}/media/${item.id}/stream`,
      fileType: item.fileType.toLowerCase() as 'audio' | 'video'
    };
    playTrack(trackToPlay);
  };

  const toggleFavorite = async () => {
    try {
      const res = await apiClient.post(`/media/${item.id}/favorite`);
      setIsFav(res.data.isFavorited);
    } catch (error) {
      console.error("Favorite toggle error", error);
    }
  };

  return (
    <div className="group rounded-3xl bg-[#1f1f1f] p-5 shadow-black/20 transition hover:shadow-xl w-full">
      <div className="relative mb-4 flex aspect-square items-center justify-center rounded-2xl bg-[#2a2a2a] overflow-hidden">
        <span className="absolute top-2 right-2 rounded-full bg-black/60 px-2 py-1 text-xs text-white backdrop-blur-md">
          {item.fileType}
        </span>
        <button 
          onClick={handlePlay}
          className="absolute bottom-2 right-2 flex h-12 w-12 translate-y-4 items-center justify-center rounded-full bg-spotify-green opacity-0 shadow-xl transition-all group-hover:translate-y-0 group-hover:opacity-100 hover:scale-105"
        >
          <span className="text-xl text-black">▶</span>
        </button>
      </div>
      <h3 className="truncate text-lg font-semibold text-white">{item.title}</h3>
      <p className="truncate text-sm text-zinc-400">{item.artist}</p>
      <div className="mt-3 flex items-center justify-between gap-2">
        <span className="text-xs text-zinc-500">{formatDuration(item.duration)}</span>
        <div className="flex items-center gap-2">
          {isAuthenticated() && (
            <select
              value=""
              onChange={async (e) => {
                const playlistId = e.target.value;
                if (!playlistId) return;
                try {
                  await apiClient.post(`/playlists/${playlistId}/tracks`, {
                    mediaItemId: item.id
                  });
                  alert('Added to playlist successfully!');
                } catch (err: any) {
                  console.error(err);
                  alert(err.response?.data?.message || err.message || 'Failed to add to playlist');
                }
              }}
              onFocus={fetchPlaylists}
              className="rounded-full bg-zinc-800 text-[11px] text-zinc-300 px-2.5 py-1 outline-none cursor-pointer border border-zinc-700 hover:border-spotify-green max-w-[100px]"
            >
              <option value="" disabled>➕ Add...</option>
              {playlists.map((pl) => (
                <option key={pl.id} value={pl.id}>
                  {pl.title}
                </option>
              ))}
            </select>
          )}
          <button onClick={toggleFavorite} className="text-lg transition hover:scale-110 focus:outline-none">
            {isFav ? '❤️' : '🤍'}
          </button>
        </div>
      </div>
    </div>
  );
}