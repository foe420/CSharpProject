import { type PlayerTrack } from '../../stores/usePlayerStore';

type BottomPlayerProps = {
  currentTrack: PlayerTrack | null;
  isPlaying: boolean;
  onToggle: () => void;
};

export function BottomPlayer({ currentTrack, isPlaying, onToggle }: BottomPlayerProps) {
  return (
    <footer className="flex h-20 items-center justify-between border-t border-zinc-800 bg-spotify-black px-6">
      <div>
        <p className="text-sm font-semibold">{currentTrack?.title ?? 'No track selected'}</p>
        <p className="text-xs text-zinc-400">{currentTrack?.artist ?? 'TuneVault starter player'}</p>
      </div>
      <button
        type="button"
        onClick={onToggle}
        className="rounded-full bg-spotify-green px-5 py-2 text-sm font-semibold text-black transition hover:brightness-110"
      >
        {isPlaying ? 'Pause' : 'Play'}
      </button>
    </footer>
  );
}
