import { useEffect, useRef, useState } from 'react';
import { usePlayerStore } from '../../stores/usePlayerStore';

const formatTime = (seconds: number) => {
  if (!Number.isFinite(seconds) || seconds <= 0) {
    return '0:00';
  }

  const minutes = Math.floor(seconds / 60);
  const remaining = Math.floor(seconds % 60);
  return `${minutes}:${remaining.toString().padStart(2, '0')}`;
};

export function BottomPlayer() {
  const audioRef = useRef<HTMLAudioElement | null>(null);
  const {
    currentTrack,
    isPlaying,
    volume,
    position,
    pause,
    resume,
    next,
    previous,
    setVolume,
    setPosition
  } = usePlayerStore();
  const [duration, setDuration] = useState(0);

  useEffect(() => {
    const audio = audioRef.current;
    if (!audio) {
      return;
    }

    audio.volume = volume;
    if (currentTrack?.src) {
      if (isPlaying) {
        void audio.play();
      } else {
        audio.pause();
      }
    } else {
      audio.pause();
    }
  }, [currentTrack, isPlaying, volume]);

  const handleLoadedMetadata = () => {
    const audio = audioRef.current;
    if (!audio) {
      return;
    }
    setDuration(audio.duration || 0);
  };

  const handleTimeUpdate = () => {
    const audio = audioRef.current;
    if (!audio) {
      return;
    }
    setPosition(audio.currentTime);
  };

  const currentProgress = duration ? Math.min((position / duration) * 100, 100) : 0;

  return (
    <footer className="fixed bottom-0 left-0 right-0 z-50 border-t border-zinc-800 bg-[#090a0d]/95 px-4 py-4 backdrop-blur-xl shadow-[0_-24px_60px_rgba(0,0,0,0.6)] md:px-6">
      {currentTrack?.src ? (
        <audio
          ref={audioRef}
          className="hidden"
          src={currentTrack.src}
          onEnded={next}
          onLoadedMetadata={handleLoadedMetadata}
          onTimeUpdate={handleTimeUpdate}
        />
      ) : null}

      <div className="mx-auto grid max-w-6xl gap-4 md:grid-cols-[1.2fr_1.8fr_1fr]">
        <div className="flex items-center gap-3 min-w-0">
          <div className="rounded-3xl bg-zinc-950/80 px-4 py-3 shadow-inner shadow-black/20">
            <p className="truncate text-sm font-semibold text-white">{currentTrack?.title ?? 'No track selected'}</p>
            <p className="truncate text-xs text-zinc-400">{currentTrack?.artist ?? 'TuneVault audio player'}</p>
          </div>
        </div>

        <div className="flex flex-col items-center gap-3">
          <div className="flex items-center gap-2 rounded-full bg-zinc-950/80 px-3 py-2 shadow-inner shadow-black/20">
            <button
              type="button"
              onClick={previous}
              className="rounded-full px-3 py-2 text-sm text-zinc-200 transition hover:bg-zinc-800"
            >
              ⏮
            </button>
            <button
              type="button"
              onClick={isPlaying ? pause : resume}
              className="rounded-full bg-spotify-green px-4 py-2 text-sm font-semibold text-black transition hover:brightness-110"
            >
              {isPlaying ? '⏸' : '▶'}
            </button>
            <button
              type="button"
              onClick={next}
              className="rounded-full px-3 py-2 text-sm text-zinc-200 transition hover:bg-zinc-800"
            >
              ⏭
            </button>
          </div>

          <div className="w-full max-w-2xl">
            <div className="flex items-center justify-between text-xs text-zinc-400">
              <span>{formatTime(position)}</span>
              <span>{formatTime(duration)}</span>
            </div>
            <div className="relative mt-2 h-2 overflow-hidden rounded-full bg-zinc-800">
              <div
                className="absolute inset-y-0 left-0 rounded-full bg-gradient-to-r from-spotify-green via-emerald-400 to-emerald-300"
                style={{ width: `${currentProgress}%` }}
              />
              <input
                type="range"
                min={0}
                max={duration || 0}
                value={position}
                onChange={(event) => {
                  const nextPosition = Number(event.target.value);
                  setPosition(nextPosition);
                  if (audioRef.current) {
                    audioRef.current.currentTime = nextPosition;
                  }
                }}
                className="absolute inset-0 h-full w-full cursor-pointer appearance-none bg-transparent"
              />
            </div>
          </div>
        </div>

        <div className="flex items-center justify-end gap-3">
          <div className="flex items-center gap-2 rounded-full bg-zinc-950/80 px-3 py-2 shadow-inner shadow-black/20">
            <span className="text-sm text-zinc-300">🔊</span>
            <input
              id="volume-range"
              type="range"
              min={0}
              max={1}
              step={0.01}
              value={volume}
              onChange={(event) => setVolume(Number(event.target.value))}
              className="h-2 w-full max-w-[160px] accent-spotify-green"
            />
          </div>
        </div>
      </div>
    </footer>
  );
}
