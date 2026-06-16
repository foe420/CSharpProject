import { useState, useRef } from 'react';
import apiClient from '../../services/apiClient';

interface Props {
  isOpen: boolean;
  onClose: () => void;
}

export function UploadModal({ isOpen, onClose }: Props) {
  const [file, setFile] = useState<File | null>(null);
  const [title, setTitle] = useState('');
  const [artist, setArtist] = useState('');
  const [genre, setGenre] = useState('');
  const [progress, setProgress] = useState(0);
  const [uploading, setUploading] = useState(false);
  const fileInputRef = useRef<HTMLInputElement>(null);

  if (!isOpen) return null;

  const handleUpload = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!file || !title || !artist) return;

    setUploading(true);
    const ext = file.name.split('.').pop()?.toLowerCase();
    const fileType = (ext === 'mp4' || ext === 'webm') ? 'Video' : 'Audio';

    const formData = new FormData();
    formData.append('file', file);
    formData.append('title', title);
    formData.append('artist', artist);
    formData.append('fileType', fileType);
    if (genre) formData.append('genre', genre);

    try {
      await apiClient.post('/media/upload', formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
        onUploadProgress: (progressEvent) => {
          if (progressEvent.total) {
            const percentCompleted = Math.round((progressEvent.loaded * 100) / progressEvent.total);
            setProgress(percentCompleted);
          }
        }
      });
      alert('Tải lên thành công!');
      onClose();
    } catch (error) {
      console.error('Lỗi tải lên:', error);
      alert('Tải lên thất bại, vui lòng thử lại.');
    } finally {
      setUploading(false);
      setProgress(0);
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/80 backdrop-blur-sm">
      <div className="w-full max-w-md rounded-2xl border border-zinc-800 bg-[#111111] p-6 text-white shadow-2xl">
        <h2 className="mb-4 text-2xl font-bold">Tải lên bài hát/Video</h2>
        <form onSubmit={handleUpload} className="space-y-4">
          <div className="rounded-xl border-2 border-dashed border-zinc-700 p-6 text-center transition hover:border-spotify-green">
            <input type="file" ref={fileInputRef} accept=".mp3,.wav,.mp4,.webm" className="hidden" onChange={(e) => setFile(e.target.files?.[0] || null)} />
            <button type="button" onClick={() => fileInputRef.current?.click()} className="rounded-full bg-zinc-800 px-4 py-2 text-sm text-white hover:bg-zinc-700">Chọn tệp</button>
            {file && <p className="mt-3 text-sm text-spotify-green">{file.name}</p>}
          </div>
          <div>
            <label className="mb-1 block text-sm text-zinc-400">Tiêu đề *</label>
            <input required value={title} onChange={(e) => setTitle(e.target.value)} className="w-full rounded-lg border border-zinc-700 bg-zinc-900 p-2.5 text-white outline-none focus:border-spotify-green" />
          </div>
          <div>
            <label className="mb-1 block text-sm text-zinc-400">Nghệ sĩ *</label>
            <input required value={artist} onChange={(e) => setArtist(e.target.value)} className="w-full rounded-lg border border-zinc-700 bg-zinc-900 p-2.5 text-white outline-none focus:border-spotify-green" />
          </div>
          <div>
            <label className="mb-1 block text-sm text-zinc-400">Thể loại (Không bắt buộc)</label>
            <input value={genre} onChange={(e) => setGenre(e.target.value)} className="w-full rounded-lg border border-zinc-700 bg-zinc-900 p-2.5 text-white outline-none focus:border-spotify-green" />
          </div>
          {uploading && (
            <div className="h-2 w-full overflow-hidden rounded-full bg-zinc-800">
              <div className="h-full bg-spotify-green transition-all" style={{ width: `${progress}%` }}></div>
            </div>
          )}
          <div className="mt-6 flex justify-end gap-3">
            <button type="button" onClick={onClose} className="rounded-full px-4 py-2 text-zinc-400 hover:text-white">Hủy</button>
            <button type="submit" disabled={!file || uploading} className="rounded-full bg-spotify-green px-6 py-2 font-semibold text-black hover:brightness-110 disabled:opacity-50">
              {uploading ? 'Đang xử lý...' : 'Tải lên'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}