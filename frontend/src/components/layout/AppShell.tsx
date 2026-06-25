import { NavLink, Outlet } from 'react-router-dom';
import { useState } from 'react';
import { BottomPlayer } from './BottomPlayer';
import { TopBar } from './TopBar';
import { UploadModal } from './UploadModal';
import { VideoPlayerPage } from '../../pages/VideoPlayerPage';

const navItems = [
  { label: '🏠 Home', path: '/home' },
  { label: '🔍 Search', path: '/search' },
  { label: '📚 Library', path: '/library' },
  { label: '🔔 Notifications', path: '/notifications' },
  { label: '💬 Share Inbox', path: '/share-inbox' },
  { label: '👤 Profile', path: '/profile' }
];

export function AppShell() {
  const [isUploadOpen, setIsUploadOpen] = useState(false);

  return (
    <div className="min-h-screen bg-[#090909] text-white pb-[120px]">
      <TopBar />
      <VideoPlayerPage />
      <div className="grid grid-cols-1 gap-4 px-6 py-5 xl:grid-cols-[18rem_1fr]">
        <aside className="rounded-3xl border border-zinc-800 bg-[#111111] p-5 shadow-black/20">
          <div className="mb-6 flex items-center justify-between gap-3">
            <h2 className="text-lg font-semibold">Library</h2>
            <button 
              onClick={() => setIsUploadOpen(true)}
              className="rounded-full border border-zinc-700 bg-zinc-950 px-3 py-2 text-xs text-zinc-300 transition hover:border-spotify-green hover:text-white"
            >
              ⬆️ Upload
            </button>
          </div>
          <nav className="space-y-2 text-sm text-zinc-300">
            {navItems.map((item) => (
              <NavLink
                key={item.path}
                to={item.path}
                className={({ isActive }) =>
                  `block rounded-2xl px-4 py-3 transition ${isActive ? 'bg-spotify-green text-black' : 'hover:bg-zinc-800'}`
                }
              >
                {item.label}
              </NavLink>
            ))}
          </nav>
        </aside>

        <main className="space-y-6">
          <Outlet />
        </main>
      </div>
      <BottomPlayer />
      <UploadModal isOpen={isUploadOpen} onClose={() => setIsUploadOpen(false)} />
    </div>
  );
}
