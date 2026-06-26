import { usePlayerStore } from '../../stores/usePlayerStore';

export function RightDetailsPanel() {
  const { currentTrack, isPlaying, duration } = usePlayerStore();

  const formatDuration = (seconds: number) => {
    if (!Number.isFinite(seconds) || seconds <= 0) return '0:00';
    const mins = Math.floor(seconds / 60);
    const secs = Math.floor(seconds % 60);
    return `${mins}:${secs.toString().padStart(2, '0')}`;
  };

  return (
    <aside className="hidden rounded-3xl border border-zinc-800 bg-[#111111] p-5 shadow-black/20 lg:block lg:w-[20rem] xl:w-[22rem] shrink-0 h-fit">
      <h2 className="text-lg font-semibold mb-4 text-zinc-100">Now Playing</h2>
      
      {currentTrack ? (
        <div className="space-y-6">
          {/* Cover Art Placeholder */}
          <div className="relative aspect-square w-full rounded-2xl bg-gradient-to-br from-spotify-green/20 via-zinc-800 to-zinc-950 flex items-center justify-center border border-zinc-800/80 shadow-md group overflow-hidden">
            <span className="text-5xl transition-transform duration-500 group-hover:scale-110">
              {currentTrack.fileType === 'video' ? '🎬' : '🎵'}
            </span>
            {isPlaying && (
              <div className="absolute bottom-4 right-4 flex items-center gap-1">
                <span className="h-3 w-1 bg-spotify-green animate-pulse rounded-full"></span>
                <span className="h-4 w-1 bg-spotify-green animate-pulse rounded-full delay-75"></span>
                <span className="h-2 w-1 bg-spotify-green animate-pulse rounded-full delay-150"></span>
              </div>
            )}
          </div>

          <div className="space-y-2">
            <h3 className="text-xl font-bold text-white truncate" title={currentTrack.title}>
              {currentTrack.title}
            </h3>
            <p className="text-sm text-zinc-400 truncate" title={currentTrack.artist}>
              {currentTrack.artist}
            </p>
          </div>

          <div className="border-t border-zinc-800/80 pt-4 space-y-3 text-xs text-zinc-400">
            <div className="flex justify-between">
              <span>Type</span>
              <span className="font-semibold text-zinc-200 capitalize">{currentTrack.fileType}</span>
            </div>
            <div className="flex justify-between">
              <span>Length</span>
              <span className="font-semibold text-zinc-200">{formatDuration(duration)}</span>
            </div>
            <div className="flex justify-between">
              <span>Status</span>
              <span className={`font-semibold ${isPlaying ? 'text-spotify-green' : 'text-zinc-500'}`}>
                {isPlaying ? 'Playing' : 'Paused'}
              </span>
            </div>
          </div>

          {currentTrack.fileType === 'video' && (
            <div className="rounded-xl bg-zinc-900/60 p-3 border border-zinc-800/60 text-xs text-zinc-300">
              💡 <strong>Video Mode</strong>: A full-screen player has overlayed to display your video stream. Use the overlay controls to close it.
            </div>
          )}
        </div>
      ) : (
        <div className="flex flex-col items-center justify-center py-20 text-center space-y-4">
          <div className="flex h-20 w-20 items-center justify-center rounded-full border border-zinc-800 bg-zinc-900/50 text-3xl">
            💤
          </div>
          <div>
            <p className="text-sm font-medium text-zinc-300">No track playing</p>
            <p className="text-xs text-zinc-500 mt-1 max-w-[15rem]">
              Select a music track or video stream from your library or homepage to see details.
            </p>
          </div>
        </div>
      )}
    </aside>
  );
}
