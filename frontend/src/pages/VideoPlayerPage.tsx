import { useEffect, useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { usePlayerStore } from '../stores/usePlayerStore';

export function VideoPlayerPage() {
  const navigate = useNavigate();
  const { currentTrack } = usePlayerStore();
  const videoRef = useRef<HTMLVideoElement>(null);
  const [isFullscreen, setIsFullscreen] = useState(false);

  useEffect(() => {
    if (videoRef.current && currentTrack?.src) {
      videoRef.current.src = currentTrack.src;
      videoRef.current.load();
      videoRef.current.play().catch(console.error);
    }
  }, [currentTrack?.src]);

  const toggleFullscreen = () => {
    if (!videoRef.current) return;

    if (isFullscreen) {
      document.exitFullscreen?.();
    } else {
      videoRef.current.requestFullscreen?.().catch(console.error);
    }
    setIsFullscreen(!isFullscreen);
  };

  if (!currentTrack || currentTrack.fileType !== 'video') {
    return null;
  }

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/90 backdrop-blur">
      <div className="relative w-full max-w-4xl">
        <video
          ref={videoRef}
          controls
          className="w-full rounded-lg"
          onEnded={toggleFullscreen}
          autoPlay
          playsInline
        >
          <source src={currentTrack.src} type="video/mp4" />
          <source src={currentTrack.src} type="video/webm" />
          Trình duyệt của bạn không hỗ trợ video tag.
        </video>
        <button
          onClick={() => navigate('/')}
          className="absolute top-4 left-4 rounded-full bg-black/50 p-2 text-white hover:bg-black/70 transition"
        >
          ✕ Đóng
        </button>
        <button
          onClick={toggleFullscreen}
          className="absolute top-4 right-4 rounded-full bg-black/50 p-2 text-white hover:bg-black/70 transition"
        >
          ⛶
        </button>
        <div className="absolute bottom-4 left-4 text-white">
          <h3 className="font-semibold">{currentTrack.title}</h3>
          <p className="text-sm text-zinc-300">{currentTrack.artist}</p>
        </div>
      </div>
    </div>
  );
}