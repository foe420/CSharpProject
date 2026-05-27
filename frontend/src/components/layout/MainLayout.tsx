import { type ReactNode } from 'react';
import { BottomPlayer } from './BottomPlayer';
import { Sidebar } from './Sidebar';
import { usePlayerStore } from '../../stores/usePlayerStore';

type MainLayoutProps = {
  children: ReactNode;
};

export function MainLayout({ children }: MainLayoutProps) {
  const { state, togglePlay } = usePlayerStore();

  return (
    <div className="grid min-h-screen grid-rows-[1fr_auto] bg-black">
      <div className="grid grid-cols-[16rem_1fr] gap-4 p-4">
        <Sidebar />
        <main className="rounded-lg bg-gradient-to-b from-zinc-900 to-black p-6">{children}</main>
      </div>
      <BottomPlayer currentTrack={state.currentTrack} isPlaying={state.isPlaying} onToggle={togglePlay} />
    </div>
  );
}
