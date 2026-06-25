import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import apiClient from '../services/apiClient';
import MediaItemCard from '../components/layout/MediaItemCard';
import type { MediaItemSummaryDto } from '../types';

interface Playlist {
  id: string;
  title: string;
  ownerName: string;
  trackCount: number;
}

export function LibraryPage() {
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState<'playlists' | 'favorites' | 'history'>('playlists');
  const [items, setItems] = useState<MediaItemSummaryDto[]>([]);
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
        const res = await apiClient.get('/users/me/playlists');
        setPlaylists(res.data || []);
      } else if (activeTab === 'favorites') {
        const res = await apiClient.get('/users/me/favorites');
        setItems(res.data.items || res.data || []);
      } else if (activeTab === 'history') {
        const res = await apiClient.get('/users/me/history');
        setItems(res.data || []);
      }
    } catch (error) {
      console.error('Library load error:', error);
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
      setPlaylists([...playlists, res.data]);
      setNewPlaylistName('');
    } catch (error) {
      console.error('Create playlist error:', error);
      alert('Playlist creation failed');
    } finally {
      setCreatingPlaylist(false);
    }
  };

  return (
    <div className="space-y-6 text-white">
      <h1 className="text-3xl font-bold">Your library</h1>
      <div className="flex gap-4 border-b border-zinc-800 pb-2">
        {[
          { id: 'playlists', label: '🎵 Playlist' },
          { id: 'favorites', label: '❤️ Favorites' },
          { id: 'history', label: '⏱️ Recent' }
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
        <p className="text-zinc-400">Loading...</p>
      ) : activeTab === 'playlists' ? (
        <div className="space-y-6">
          <form onSubmit={handleCreatePlaylist} className="rounded-2xl border border-zinc-800 bg-[#131313] p-6">
            <h3 className="text-lg font-semibold mb-3">Create new playlist</h3>
            <div className="flex gap-3">
              <input
                type="text"
                placeholder="Playlist name..."
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
                Create
              </button>
            </div>
          </form>

          {playlists.length === 0 ? (
            <div className="rounded-2xl border border-zinc-800 bg-[#131313] p-8 text-center text-zinc-500">
              No playlists found. Create one now!
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
                      {playlist.trackCount} tracks • {playlist.ownerName}
                    </p>
                  </div>
                  <button
                    onClick={(e) => {
                      e.stopPropagation();
                      navigate(`/playlist/${playlist.id}`);
                    }}
                    className="rounded-full bg-spotify-green px-4 py-2 text-sm font-semibold text-black hover:bg-green-500 transition"
                  >
                    ▶ Play
                  </button>
                </div>
              ))}
            </div>
          )}
        </div>
      ) : (
        <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5">
          {items.length > 0 ? (
            items.map((item) => <MediaItemCard key={item.id} item={item} />)
          ) : (
            <p className="col-span-full text-center text-zinc-500">No data available.</p>
          )}
        </div>
      )}
    </div>
  );
}