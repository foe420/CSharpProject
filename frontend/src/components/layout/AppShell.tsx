import { NavLink, Outlet } from 'react-router-dom';
import { BottomPlayer } from './BottomPlayer';
import { TopBar } from './TopBar';

const navItems = [
  { label: 'Thư viện', path: '/app/home' },
  { label: 'Tìm kiếm', path: '/app/search' },
  { label: 'Playlist', path: '/app/library' },
  { label: 'Thông báo', path: '/app/notifications' }
];

export function AppShell() {
  return (
    <div className="min-h-screen bg-[#090909] text-white pb-[120px]">
      <TopBar />
      <div className="grid grid-cols-1 gap-4 px-6 py-5 xl:grid-cols-[18rem_1fr]">
        <aside className="rounded-3xl border border-zinc-800 bg-[#111111] p-5 shadow-black/20">
          <div className="mb-6 flex items-center justify-between gap-3">
            <h2 className="text-lg font-semibold">Thư viện</h2>
            <button className="rounded-full border border-zinc-700 bg-zinc-950 px-3 py-2 text-xs text-zinc-300 transition hover:border-spotify-green hover:text-white">
              + Tạo
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
          <div className="grid gap-4 xl:grid-cols-[1.35fr_0.75fr]">
            <section className="rounded-3xl border border-zinc-800 bg-[#131313] p-6">
              <div className="mb-4 flex items-center justify-between">
                <div>
                  <p className="text-xs uppercase tracking-[0.3em] text-spotify-green">Xin chào</p>
                  <h1 className="mt-3 text-3xl font-bold text-white">Lắng nghe hôm nay</h1>
                </div>
                <button className="rounded-full bg-spotify-green px-4 py-2 text-sm font-semibold text-black transition hover:brightness-110">
                  Tạo playlist mới
                </button>
              </div>
              <div className="grid gap-4 sm:grid-cols-2 xl:grid-cols-3">
                <article className="rounded-3xl bg-[#1f1f1f] p-5 shadow-black/20 transition hover:shadow-xl">
                  <div className="mb-4 flex items-center justify-between">
                    <span className="rounded-full bg-white/5 px-3 py-1 text-xs uppercase tracking-[0.2em] text-zinc-300">Gợi ý</span>
                    <button className="rounded-full border border-zinc-700 p-2 text-zinc-300 transition hover:border-spotify-green">
                      …
                    </button>
                  </div>
                  <h2 className="text-xl font-semibold text-white">Discover Weekly</h2>
                  <p className="mt-3 text-sm text-zinc-400">Playlist được cá nhân hóa hàng tuần.</p>
                </article>
                <article className="rounded-3xl bg-[#1f1f1f] p-5 shadow-black/20 transition hover:shadow-xl">
                  <div className="mb-4 flex items-center justify-between">
                    <span className="rounded-full bg-white/5 px-3 py-1 text-xs uppercase tracking-[0.2em] text-zinc-300">Radio</span>
                    <button className="rounded-full border border-zinc-700 p-2 text-zinc-300 transition hover:border-spotify-green">
                      …
                    </button>
                  </div>
                  <h2 className="text-xl font-semibold text-white">Radio phổ biến</h2>
                  <p className="mt-3 text-sm text-zinc-400">Nghe những playlist đã được nhiều người yêu thích.</p>
                </article>
                <article className="rounded-3xl bg-[#1f1f1f] p-5 shadow-black/20 transition hover:shadow-xl">
                  <div className="mb-4 flex items-center justify-between">
                    <span className="rounded-full bg-white/5 px-3 py-1 text-xs uppercase tracking-[0.2em] text-zinc-300">Khám phá</span>
                    <button className="rounded-full border border-zinc-700 p-2 text-zinc-300 transition hover:border-spotify-green">
                      …
                    </button>
                  </div>
                  <h2 className="text-xl font-semibold text-white">Top mới</h2>
                  <p className="mt-3 text-sm text-zinc-400">Theo dõi bài hát mới phát hành mỗi ngày.</p>
                </article>
              </div>
            </section>
            <section className="rounded-3xl border border-zinc-800 bg-[#131313] p-6">
              <h2 className="text-xl font-semibold text-white">Nghe gần đây</h2>
              <div className="mt-5 space-y-3">
                <div className="flex items-center justify-between rounded-3xl bg-[#1d1d1d] p-4">
                  <div>
                    <p className="font-semibold text-white">Noi Dau Muon Mang</p>
                    <p className="text-sm text-zinc-500">Tuấn Ngọc</p>
                  </div>
                  <button className="rounded-full bg-spotify-green px-4 py-2 text-sm font-semibold text-black transition hover:brightness-110">
                    Play
                  </button>
                </div>
                <div className="flex items-center justify-between rounded-3xl bg-[#1d1d1d] p-4">
                  <div>
                    <p className="font-semibold text-white">Phương Anh</p>
                    <p className="text-sm text-zinc-500">Nhạc Việt</p>
                  </div>
                  <button className="rounded-full bg-spotify-green px-4 py-2 text-sm font-semibold text-black transition hover:brightness-110">
                    Play
                  </button>
                </div>
              </div>
            </section>
          </div>
          <Outlet />
        </main>
      </div>
      <BottomPlayer />
    </div>
  );
}
