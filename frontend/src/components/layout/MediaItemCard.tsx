import { useState } from 'react';
import { usePlayerStore } from '../../stores/usePlayerStore';
import type { PlayerTrack } from '../../stores/usePlayerStore';
import apiClient from '../../services/apiClient';

interface Props {
  item: any;
}

export default function MediaItemCard({ item }: Props) {
  const { playTrack } = usePlayerStore();
  const [isFav, setIsFav] = useState(item.isFavorited || false);

  const formatDuration = (seconds: number) => {
    const m = Math.floor(seconds / 60);
    const s = Math.floor(seconds % 60);
    return `${m}:${s < 10 ? '0' : ''}${s}`;
  };

  const getFileType = (): 'audio' | 'video' => {
    const type = item.fileType;
    if (typeof type === 'string') {
      return type.toLowerCase() === 'video' ? 'video' : 'audio';
    }
    if (typeof type === 'number') {
      return type === 2 ? 'video' : 'audio';  // 2 = Video
    }
    return 'audio';
  };

  const handlePlay = () => {
    apiClient.post(`/Media/${item.id}/play`).catch(console.error);
    
    const baseUrl = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5237';
    const fileType = getFileType();
    
    const trackToPlay: PlayerTrack = {
      id: item.id,
      title: item.title || 'Untitled',
      artist: item.artist || 'Unknown Artist',
      duration: item.duration || 0,
      src: `${baseUrl}/Media/${item.id}/stream`,
      fileType: fileType
    };
    playTrack(trackToPlay);
  };

  const toggleFavorite = async () => {
    try {
      const res = await apiClient.post(`/Media/${item.id}/favorite`);
      setIsFav(res.data.isFavorited);
    } catch (error) {
      console.error("Lỗi khi thêm yêu thích", error);
    }
  };

  const fileType = getFileType();

  return (
    <div className="group rounded-3xl bg-[#1f1f1f] p-5 shadow-black/20 transition hover:shadow-xl w-full">
      <div className="relative mb-4 flex aspect-square items-center justify-center rounded-2xl bg-[#2a2a2a] overflow-hidden">
        <span className="absolute top-2 right-2 rounded-full bg-black/60 px-2 py-1 text-xs text-white backdrop-blur-md">
          {fileType === 'video' ? '🎬 Video' : '🎵 Audio'}
        </span>
        <button 
          onClick={handlePlay}
          className="absolute bottom-2 right-2 flex h-12 w-12 translate-y-4 items-center justify-center rounded-full bg-spotify-green opacity-0 shadow-xl transition-all group-hover:translate-y-0 group-hover:opacity-100 hover:scale-105"
        >
          <span className="text-xl text-black">▶</span>
        </button>
      </div>
      <h3 className="truncate text-lg font-semibold text-white">{item.title || 'Untitled'}</h3>
      <p className="truncate text-sm text-zinc-400">{item.artist || 'Unknown Artist'}</p>
      <div className="mt-3 flex items-center justify-between">
        <span className="text-xs text-zinc-500">{formatDuration(item.duration || 0)}</span>
        <button onClick={toggleFavorite} className="text-lg transition hover:scale-110 focus:outline-none">
          {isFav ? '❤️' : '🤍'}
        </button>
      </div>
    </div>
  );
}