import { useEffect, useState } from 'react';
import apiClient from '../services/apiClient';
import MediaItemCard from '../components/MediaItemCard';
import { MediaItemSummaryDto } from '../types';

export function LibraryPage() {
  const [activeTab, setActiveTab] = useState<'playlists' | 'favorites' | 'history'>('favorites');
  const [items, setItems] = useState<MediaItemSummaryDto[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchLibraryData = async () => {
      setLoading(true);
      try {
        if (activeTab === 'favorites') {
          const res = await apiClient.get('/users/me/favorites');
          setItems(res.data.items || []);
        } else if (activeTab === 'history') {
          const res = await apiClient.get('/users/me/history');
          setItems(res.data || []);
        } else {
          setItems([]); 
        }
      } catch (error) {
        console.error('Lỗi tải thư viện:', error);
      } finally {
        setLoading(false);
      }
    };
    fetchLibraryData();
  }, [activeTab]);

  return (
    <div className="space-y-6 text-white">
      <h1 className="text-3xl font-bold">Thư viện của bạn</h1>
      <div className="flex gap-4 border-b border-zinc-800 pb-2">
        {[{ id: 'playlists', label: 'Playlists' }, { id: 'favorites', label: 'Yêu thích' }, { id: 'history', label: 'Nghe gần đây' }].map((tab) => (
          <button key={tab.id} onClick={() => setActiveTab(tab.id as any)} className={`pb-2 text-sm font-semibold transition ${activeTab === tab.id ? 'border-b-2 border-spotify-green text-white' : 'text-zinc-500 hover:text-zinc-300'}`}>
            {tab.label}
          </button>
        ))}
      </div>
      {loading ? (
        <p className="text-zinc-400">Đang tải...</p>
      ) : activeTab === 'playlists' ? (
        <div className="text-zinc-400">Chưa có playlist nào.</div>
      ) : (
        <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5">
          {items.length > 0 ? items.map((item) => <MediaItemCard key={item.id} item={item} />) : <p className="col-span-full text-zinc-500">Chưa có dữ liệu.</p>}
        </div>
      )}
    </div>
  );
}