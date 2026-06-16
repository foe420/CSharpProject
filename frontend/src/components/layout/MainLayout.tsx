import { Outlet } from 'react-router-dom';
import { BottomPlayer } from './BottomPlayer';
import { Sidebar } from './Sidebar';

export function MainLayout() {
  return (
    <div className="grid min-h-screen grid-rows-[1fr_auto] bg-black">
      <div className="grid grid-cols-[16rem_1fr] gap-4 p-4">
        <Sidebar />
        <main className="rounded-lg bg-gradient-to-b from-zinc-900 to-black p-6">
          <Outlet />
        </main>
      </div>
      <BottomPlayer />
    </div>
  );
}
