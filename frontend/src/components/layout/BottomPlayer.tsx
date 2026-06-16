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
    <footer className="fixed bottom-0 left-0 right-0 z-50 grid gap-4 border-t border-zinc-800 bg-[#081011]/95 px-6 py-3 text-white backdrop-blur-xl md:grid-cols-[1.5fr_2fr_1fr]">
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

      <div className="flex items-center gap-4 min-w-0">
        <div className="min-w-0">
          <p className="truncate text-sm font-semibold text-white">
            {currentTrack?.title ?? 'No track selected'}
          </p>
          <p className="truncate text-xs text-zinc-400">{currentTrack?.artist ?? 'TuneVault audio player'}</p>
        </div>
      </div>

      <div className="flex flex-col items-center gap-3">
        <div className="flex items-center gap-3">
          <button
            type="button"
            onClick={previous}
            className="rounded-full border border-zinc-700 px-3 py-2 text-sm text-zinc-200 transition hover:bg-zinc-800"
          >
            Prev
          </button>
          <button
            type="button"
            onClick={isPlaying ? pause : resume}
            className="rounded-full bg-spotify-green px-4 py-2 text-sm font-semibold text-black transition hover:brightness-110"
          >
            {isPlaying ? 'Pause' : 'Play'}
          </button>
          <button
            type="button"
            onClick={next}
            className="rounded-full border border-zinc-700 px-3 py-2 text-sm text-zinc-200 transition hover:bg-zinc-800"
          >
            Next
          </button>
        </div>
        <div className="w-full max-w-2xl">
          <div className="h-1 overflow-hidden rounded-full bg-zinc-800">
            <div className="h-full bg-spotify-green" style={{ width: `${currentProgress}%` }} />
          </div>
          <div className="mt-2 flex items-center justify-between text-xs text-zinc-400">
            <span>{formatTime(position)}</span>
            <span>{formatTime(duration)}</span>
          </div>
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
            className="mt-2 w-full accent-spotify-green"
          />
        </div>
      </div>

      <div className="flex items-center justify-end gap-3">
        <label className="sr-only" htmlFor="volume-range">
          Volume
        </label>
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
    </footer>
  );
}
