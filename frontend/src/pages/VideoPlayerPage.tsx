import { useEffect, useRef, useState } from 'react';
import { usePlayerStore } from '../stores/usePlayerStore';

export function VideoPlayerPage() {
  const { currentTrack, stop, setDuration } = usePlayerStore();
  const videoRef = useRef<HTMLVideoElement>(null);
  const [isFullscreen, setIsFullscreen] = useState(false);

  useEffect(() => {
    const video = videoRef.current;
    if (video) {
      const handleLoadedMetadata = () => {
        setDuration(video.duration || 0);
      };
      video.addEventListener('loadedmetadata', handleLoadedMetadata);
      return () => {
        video.removeEventListener('loadedmetadata', handleLoadedMetadata);
      };
    }
  }, [currentTrack, setDuration]);

  useEffect(() => {
    if (videoRef.current && currentTrack?.src) {
      videoRef.current.src = currentTrack.src;
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

  const handleClose = () => {
    stop();
    if (document.fullscreenElement) {
      document.exitFullscreen?.();
      setIsFullscreen(false);
    }
  };

  if (!currentTrack || currentTrack.fileType !== 'video') {
    return null;
  }

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/90 backdrop-blur-sm px-4 py-6 sm:px-8">
      <div className="relative w-full max-w-5xl overflow-hidden rounded-[28px] border border-zinc-800 bg-zinc-950/95 shadow-2xl">
        <video
          ref={videoRef}
          controls
          className="h-[68vh] w-full bg-black object-contain"
          autoPlay
        >
          <source src={currentTrack.src} type="video/mp4" />
          Trình duyệt của bạn không hỗ trợ video tag.
        </video>

        <div className="absolute inset-x-0 top-4 flex items-center justify-between px-4 sm:px-6">
          <div className="rounded-full bg-black/50 px-3 py-2 text-sm text-white shadow-sm">
            Now playing
          </div>
          <div className="flex items-center gap-2">
            <button
              type="button"
              onClick={toggleFullscreen}
              className="inline-flex h-10 w-10 items-center justify-center rounded-full bg-black/50 text-white transition hover:bg-black/70"
              aria-label="Toggle full screen"
            >
              ⛶
            </button>
            <button
              type="button"
              onClick={handleClose}
              className="inline-flex h-10 w-10 items-center justify-center rounded-full bg-rose-500/95 text-white transition hover:bg-rose-400"
              aria-label="Close video"
            >
              ✕
            </button>
          </div>
        </div>

        <div className="absolute inset-x-0 bottom-0 rounded-b-[28px] border-t border-white/10 bg-black/70 px-5 py-4 backdrop-blur-sm">
          <div className="flex flex-col gap-2 sm:flex-row sm:items-center sm:justify-between">
            <div className="min-w-0">
              <h3 className="truncate text-lg font-semibold text-white">{currentTrack.title}</h3>
              <p className="truncate text-sm text-zinc-300">{currentTrack.artist}</p>
            </div>
            <div className="flex flex-wrap items-center gap-2 text-xs text-zinc-300">
              <span className="rounded-full border border-zinc-700 bg-white/5 px-3 py-1">Tap ✕ to close</span>
              <span className="rounded-full border border-zinc-700 bg-white/5 px-3 py-1">Use controls to adjust volume</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
