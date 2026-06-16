import { useEffect, useState } from 'react';
import apiClient from '../services/apiClient';
import { usePlayerStore,type   PlayerTrack } from '../stores/usePlayerStore';

interface ShareItem {
  id: string;
  senderName: string;
  receiverName: string;
  sharedAt: string;
  mediaItem: { id: string; title: string; artist: string; duration: number; fileType: 'Audio' | 'Video'; };
}

export function ShareInboxPage() {
  const [activeTab, setActiveTab] = useState<'inbox' | 'sent'>('inbox');
  const [shares, setShares] = useState<ShareItem[]>([]);
  const [loading, setLoading] = useState(false);
  const { playTrack } = usePlayerStore();

  useEffect(() => {
    const fetchShares = async () => {
      setLoading(true);
      try {
        const endpoint = activeTab === 'inbox' ? '/shares/inbox' : '/shares/sent';
        const res = await apiClient.get(endpoint);
        setShares(res.data);
      } catch (error) {
        console.error('Lỗi tải danh sách chia sẻ:', error);
      } finally {
        setLoading(false);
      }
    };
    fetchShares();
  }, [activeTab]);

  const handlePlay = (item: ShareItem['mediaItem']) => {
    const baseUrl = import.meta.env.VITE_API_BASE_URL || 'https://localhost:5001/api';
    const trackToPlay: PlayerTrack = {
      id: item.id, title: item.title, artist: item.artist, duration: item.duration,
      src: `${baseUrl}/media/${item.id}/stream`, fileType: item.fileType.toLowerCase() as 'audio' | 'video'
    };
    playTrack(trackToPlay);
  };

  return (
    <div className="space-y-6 text-white">
      <h1 className="text-3xl font-bold">Hộp thư chia sẻ</h1>
      <div className="flex gap-4 border-b border-zinc-800 pb-2">
        <button onClick={() => setActiveTab('inbox')} className={`pb-2 text-sm font-semibold transition ${activeTab === 'inbox' ? 'border-b-2 border-spotify-green text-white' : 'text-zinc-500 hover:text-zinc-300'}`}>Nhận được</button>
        <button onClick={() => setActiveTab('sent')} className={`pb-2 text-sm font-semibold transition ${activeTab === 'sent' ? 'border-b-2 border-spotify-green text-white' : 'text-zinc-500 hover:text-zinc-300'}`}>Đã gửi</button>
      </div>
      {loading ? (
        <p className="text-zinc-400">Đang tải...</p>
      ) : shares.length === 0 ? (
        <p className="text-zinc-500">Chưa có nội dung chia sẻ nào.</p>
      ) : (
        <div className="space-y-3">
          {shares.map((share) => (
            <div key={share.id} className="flex items-center justify-between rounded-xl bg-[#1d1d1d] p-4 transition hover:bg-[#252525]">
              <div>
                <p className="text-sm text-spotify-green mb-1">{activeTab === 'inbox' ? `Từ: ${share.senderName}` : `Gửi đến: ${share.receiverName}`}</p>
                <p className="font-semibold text-white">{share.mediaItem?.title || 'Playlist'}</p>
                <p className="text-xs text-zinc-400">{share.mediaItem?.artist}</p>
                <p className="text-xs text-zinc-500 mt-2">{new Date(share.sharedAt).toLocaleString('vi-VN')}</p>
              </div>
              {share.mediaItem && (
                <button onClick={() => handlePlay(share.mediaItem)} className="rounded-full bg-spotify-green px-4 py-2 text-sm font-semibold text-black transition hover:brightness-110">Phát ngay</button>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}