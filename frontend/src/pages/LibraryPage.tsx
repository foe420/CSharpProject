import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import apiClient from '../services/apiClient';
import MediaItemCard from '../components/layout/MediaItemCard';

interface Playlist {
  id: string;
  title: string;
  ownerName: string;
  trackCount: number;
}

export function LibraryPage() {
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState<'playlists' | 'favorites' | 'history'>('playlists');
  const [favorites, setFavorites] = useState<any[]>([]);
  const [history, setHistory] = useState<any[]>([]);
  const [playlists, setPlaylists] = useState<Playlist[]>([]);
  const [loading, setLoading] = useState(false);
  const [newPlaylistName, setNewPlaylistName] = useState('');
  const [creatingPlaylist, setCreatingPlaylist] = useState(false);

  useEffect(() => {
    fetchLibraryData();
  }, [activeTab]);

  const fetchLibraryData = async () => {
    setLoading(true);
    try {
      if (activeTab === 'playlists') {
        const res = await apiClient.get('/playlists/me');
        setPlaylists(res.data || []);
      } else if (activeTab === 'favorites') {
        const res = await apiClient.get('/Media/users/me/favorites?page=1&pageSize=50');
        setFavorites(res.data.items || []);
      } else if (activeTab === 'history') {
        const [historyRes, favRes] = await Promise.all([
          apiClient.get('/Media/users/me/history'),
          apiClient.get('/Media/users/me/favorites?page=1&pageSize=999')
        ]);
        
        // Tạo Set các ID đã favorite
        const favIds = new Set((favRes.data.items || []).map((f: any) => f.mediaItemId));
        
        // Thêm isFavorited vào từng item
        const historyWithFav = (historyRes.data || []).map((item: any) => ({
          ...item,
          isFavorited: favIds.has(item.mediaItemId || item.id)
        }));
        
        setHistory(historyWithFav);
      }
    } catch (error) {
      console.error('Lỗi tải thư viện:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleCreatePlaylist = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newPlaylistName.trim()) return;

    setCreatingPlaylist(true);
    try {
      const res = await apiClient.post('/playlists', {
        title: newPlaylistName,
        isPublic: false
      });
      setPlaylists([res.data, ...playlists]);
      setNewPlaylistName('');
    } catch (error) {
      console.error('Lỗi tạo playlist:', error);
      alert('Tạo playlist thất bại');
    } finally {
      setCreatingPlaylist(false);
    }
  };

  const renderMediaItems = (items: any[], type: 'favorites' | 'history') => {
  if (items.length === 0) {
    return (
      <div className="col-span-full text-center text-zinc-500 py-12">
        {type === 'favorites' ? '💔 Chưa có bài hát yêu thích nào' : '🎵 Chưa có lịch sử nghe nào'}
      </div>
    );
  }

  return items.map((item) => {
    const isFavorited = type === 'favorites' ? true : (item.isFavorited || false);  // ← QUAN TRỌNG
    
    const mediaItem = {
      id: item.mediaItemId || item.id,
      title: item.title || 'Untitled',
      artist: item.artist || 'Unknown Artist',
      genre: item.genre || null,
      fileType: item.fileType === 2 ? 'Video' : 'Audio',
      duration: item.duration || 0,
      isFavorited: isFavorited,  // ← QUAN TRỌNG
      createdAt: item.favoritedAt || item.playedAt || new Date().toISOString()
    };
    return <MediaItemCard key={mediaItem.id} item={mediaItem} />;
  });
};

  return (
    <div className="space-y-6 text-white">
      <h1 className="text-3xl font-bold">Thư viện của bạn</h1>

      <div className="flex gap-4 border-b border-zinc-800 pb-2">
        {[
          { id: 'playlists', label: '🎵 Playlist' },
          { id: 'favorites', label: '❤️ Yêu thích' },
          { id: 'history', label: '⏱️ Nghe gần đây' }
        ].map((tab) => (
          <button
            key={tab.id}
            onClick={() => setActiveTab(tab.id as any)}
            className={`pb-2 text-sm font-semibold transition ${
              activeTab === tab.id
                ? 'border-b-2 border-spotify-green text-white'
                : 'text-zinc-500 hover:text-zinc-300'
            }`}
          >
            {tab.label}
          </button>
        ))}
      </div>

      {loading ? (
        <p className="text-zinc-400">Đang tải...</p>
      ) : activeTab === 'playlists' ? (
        <div className="space-y-6">
          <form onSubmit={handleCreatePlaylist} className="rounded-2xl border border-zinc-800 bg-[#131313] p-6">
            <h3 className="text-lg font-semibold mb-3">Tạo playlist mới</h3>
            <div className="flex gap-3">
              <input
                type="text"
                placeholder="Tên playlist..."
                value={newPlaylistName}
                onChange={(e) => setNewPlaylistName(e.target.value)}
                maxLength={100}
                className="flex-1 rounded-full border border-zinc-700 bg-zinc-900 px-4 py-2 text-white outline-none focus:border-spotify-green"
              />
              <button
                type="submit"
                disabled={creatingPlaylist || !newPlaylistName.trim()}
                className="rounded-full bg-spotify-green px-6 py-2 font-semibold text-black hover:bg-green-500 transition disabled:opacity-50"
              >
                Tạo
              </button>
            </div>
          </form>

          {playlists.length === 0 ? (
            <div className="rounded-2xl border border-zinc-800 bg-[#131313] p-8 text-center text-zinc-500">
              Chưa có playlist nào. Hãy tạo một cái mới!
            </div>
          ) : (
            <div className="grid gap-3">
              {playlists.map((playlist) => (
                <div
                  key={playlist.id}
                  onClick={() => navigate(`/playlist/${playlist.id}`)}
                  className="flex items-center gap-4 rounded-lg bg-[#1a1a1a] p-4 hover:bg-[#2a2a2a] transition cursor-pointer"
                >
                  <div className="flex h-16 w-16 items-center justify-center rounded-lg bg-zinc-800 text-2xl">
                    🎵
                  </div>
                  <div className="flex-1">
                    <p className="font-semibold hover:text-spotify-green transition">
                      {playlist.title}
                    </p>
                    <p className="text-sm text-zinc-400">
                      {playlist.trackCount} bài • {playlist.ownerName}
                    </p>
                  </div>
                  <button
                    onClick={(e) => {
                      e.stopPropagation();
                      navigate(`/playlist/${playlist.id}`);
                    }}
                    className="rounded-full bg-spotify-green px-4 py-2 text-sm font-semibold text-black hover:bg-green-500 transition"
                  >
                    ▶ Phát
                  </button>
                </div>
              ))}
            </div>
          )}
        </div>
      ) : (
        <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5">
          {activeTab === 'favorites' 
            ? renderMediaItems(favorites, 'favorites')
            : renderMediaItems(history, 'history')}
        </div>
      )}
    </div>
  );
}