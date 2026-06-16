import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import apiClient from '../services/apiClient';
import { usePlayerStore, type PlayerTrack } from '../stores/usePlayerStore';
import MediaItemCard from '../components/layout/MediaItemCard';

interface PlaylistDetail {
  id: string;
  title: string;
  isPublic: boolean;
  ownerName: string;
  tracks: Array<{
    id: string;
    title: string;
    artist: string;
    duration: number;
    fileType: 'Audio' | 'Video';
    isFavorited?: boolean;
  }>;
}

export function PlaylistDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { playTrack } = usePlayerStore();
  const [playlist, setPlaylist] = useState<PlaylistDetail | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [isSharing, setIsSharing] = useState(false);
  const [shareReceiverEmail, setShareReceiverEmail] = useState('');

  useEffect(() => {
    if (!id) return;
    fetchPlaylist();
  }, [id]);

  const fetchPlaylist = async () => {
    setLoading(true);
    try {
      const res = await apiClient.get(`/playlists/${id}`);
      setPlaylist(res.data);
      setError('');
    } catch (err) {
      setError('Không thể tải playlist');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handlePlayTrack = (track: PlaylistDetail['tracks'][0]) => {
    const baseUrl = import.meta.env.VITE_API_BASE_URL || 'https://localhost:5001/api';
    const trackToPlay: PlayerTrack = {
      id: track.id,
      title: track.title,
      artist: track.artist,
      duration: track.duration,
      src: `${baseUrl}/media/${track.id}/stream`,
      fileType: track.fileType.toLowerCase() as 'audio' | 'video'
    };
    playTrack(trackToPlay);
  };

  const handleRemoveTrack = async (trackId: string) => {
    if (!id) return;
    try {
      await apiClient.delete(`/playlists/${id}/tracks/${trackId}`);
      setPlaylist((prev) =>
        prev
          ? { ...prev, tracks: prev.tracks.filter((t) => t.id !== trackId) }
          : null
      );
    } catch (err) {
      console.error('Lỗi xóa track:', err);
    }
  };

  const handleSharePlaylist = async () => {
    if (!id || !shareReceiverEmail.trim()) return;
    try {
      await apiClient.post('/shares', {
        receiverEmail: shareReceiverEmail,
        playlistId: id
      });
      alert('Chia sẻ thành công!');
      setShareReceiverEmail('');
      setIsSharing(false);
    } catch (err) {
      console.error('Lỗi chia sẻ:', err);
      alert('Chia sẻ thất bại');
    }
  };

  const formatDuration = (seconds: number) => {
    const m = Math.floor(seconds / 60);
    const s = Math.floor(seconds % 60);
    return `${m}:${s < 10 ? '0' : ''}${s}`;
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-screen text-white">
        <p>Đang tải...</p>
      </div>
    );
  }

  if (error || !playlist) {
    return (
      <div className="space-y-6 text-white">
        <div className="text-center">
          <p className="text-red-500 mb-4">{error}</p>
          <button
            onClick={() => navigate('/library')}
            className="rounded-full bg-spotify-green px-6 py-2 font-semibold text-black hover:bg-green-500 transition"
          >
            Quay lại thư viện
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6 text-white">
      {/* Header */}
      <div className="rounded-2xl border border-zinc-800 bg-gradient-to-b from-[#1a1a1a] to-[#0a0a0a] p-8">
        <div className="flex items-end gap-8">
          <div className="flex h-48 w-48 items-center justify-center rounded-2xl bg-zinc-800 text-6xl">
            🎵
          </div>
          <div className="flex-1">
            <p className="text-sm uppercase tracking-widest text-spotify-green">Playlist</p>
            <h1 className="text-5xl font-bold mb-2">{playlist.title}</h1>
            <p className="text-zinc-400 mb-4">
              Bởi <span className="text-white font-semibold">{playlist.ownerName}</span>
            </p>
            <p className="text-sm text-zinc-400">
              {playlist.tracks.length} bài • {playlist.isPublic ? 'Công khai' : 'Riêng tư'}
            </p>
            <div className="flex gap-3 mt-6">
              <button
                onClick={() => playlist.tracks.length > 0 && handlePlayTrack(playlist.tracks[0])}
                className="rounded-full bg-spotify-green px-8 py-3 font-semibold text-black hover:bg-green-500 transition disabled:opacity-50"
                disabled={playlist.tracks.length === 0}
              >
                ▶ Phát
              </button>
              <button
                onClick={() => setIsSharing(!isSharing)}
                className="rounded-full border border-zinc-700 px-8 py-3 font-semibold hover:border-white transition"
              >
                📤 Chia sẻ
              </button>
            </div>
          </div>
        </div>
      </div>

      {/* Share form */}
      {isSharing && (
        <div className="rounded-2xl border border-zinc-800 bg-[#131313] p-6">
          <h3 className="font-semibold mb-4">Chia sẻ playlist với</h3>
          <div className="flex gap-3">
            <input
              type="email"
              placeholder="Email người nhận..."
              value={shareReceiverEmail}
              onChange={(e) => setShareReceiverEmail(e.target.value)}
              className="flex-1 rounded-full border border-zinc-700 bg-zinc-900 px-4 py-2 text-white outline-none focus:border-spotify-green"
            />
            <button
              onClick={handleSharePlaylist}
              disabled={!shareReceiverEmail.trim()}
              className="rounded-full bg-spotify-green px-6 py-2 font-semibold text-black hover:bg-green-500 transition disabled:opacity-50"
            >
              Gửi
            </button>
          </div>
        </div>
      )}

      {/* Tracks list */}
      <div>
        <h2 className="text-2xl font-bold mb-4">Các bài hát</h2>
        {playlist.tracks.length === 0 ? (
          <div className="rounded-2xl border border-zinc-800 bg-[#131313] p-8 text-center text-zinc-500">
            Playlist này chưa có bài hát nào
          </div>
        ) : (
          <div className="space-y-2">
            {playlist.tracks.map((track, idx) => (
              <div
                key={track.id}
                className="group flex items-center gap-4 rounded-lg bg-[#1a1a1a] p-4 hover:bg-[#2a2a2a] transition"
              >
                <span className="w-8 text-center text-zinc-500">{idx + 1}</span>
                <div
                  className="flex-1 cursor-pointer"
                  onClick={() => handlePlayTrack(track)}
                >
                  <p className="font-semibold hover:text-spotify-green transition">
                    {track.title}
                  </p>
                  <p className="text-sm text-zinc-400">{track.artist}</p>
                </div>
                <span className="text-xs text-zinc-500 px-2 py-1 rounded bg-zinc-900">
                  {track.fileType}
                </span>
                <span className="w-12 text-right text-sm text-zinc-400">
                  {formatDuration(track.duration)}
                </span>
                <button
                  onClick={() => handleRemoveTrack(track.id)}
                  className="opacity-0 group-hover:opacity-100 text-red-500 hover:text-red-400 transition"
                >
                  ✕
                </button>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
