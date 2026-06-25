import { useEffect, useState } from 'react';
import apiClient from '../services/apiClient';
import MediaItemCard from '../components/layout/MediaItemCard';
import type { MediaItemSummaryDto } from '../types';

export function SearchPage() {
  const [searchTerm, setSearchTerm] = useState('');
  const [results, setResults] = useState<MediaItemSummaryDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [filter, setFilter] = useState<'All' | 'Audio' | 'Video'>('All');

  useEffect(() => {
    const delayDebounceFn = setTimeout(async () => {
      if (!searchTerm.trim()) {
        setResults([]);
        return;
      }
      setLoading(true);
      try {
        const fileTypeQuery = filter !== 'All' ? `&fileType=${filter}` : '';
        const res = await apiClient.get(`/media/search?term=${searchTerm}${fileTypeQuery}&page=1&pageSize=20`);
        setResults(res.data.items || []);
      } catch (error) {
        console.error('Search error:', error);
      } finally {
        setLoading(false);
      }
    }, 400);

    return () => clearTimeout(delayDebounceFn);
  }, [searchTerm, filter]);

  return (
    <div className="space-y-6 text-white">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold">Search</h1>
        <div className="flex gap-2">
          {['All', 'Audio', 'Video'].map((f) => (
            <button key={f} onClick={() => setFilter(f as any)} className={`rounded-full px-4 py-1 text-sm transition ${filter === f ? 'bg-spotify-green text-black font-semibold' : 'bg-zinc-800 text-zinc-300 hover:bg-zinc-700'}`}>
              {f}
            </button>
          ))}
        </div>
      </div>
      <input type="search" placeholder="Search for song, artist..." value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} className="w-full rounded-full border border-zinc-700 bg-zinc-900 px-6 py-3 text-white outline-none focus:border-spotify-green" />
      {loading && <p className="text-zinc-400">Searching...</p>}
      {!loading && results.length > 0 && (
        <div className="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5">
          {results.map((item) => <MediaItemCard key={item.id} item={item} />)}
        </div>
      )}
      {!loading && searchTerm && results.length === 0 && (
        <div className="mt-10 text-center text-zinc-500">No results found for "{searchTerm}"</div>
      )}
    </div>
  );
}