import { NavLink } from 'react-router-dom';

const navItems = [
  { label: 'Home', path: '/home' },
  { label: 'Search', path: '/search' },
  { label: 'Library', path: '/library' },
  { label: 'Playlists', path: '/playlist/1' },
  { label: 'Notifications', path: '/notifications' },
  { label: 'Profile', path: '/profile' }
];

export function Sidebar() {
  return (
    <aside className="w-64 rounded-lg bg-spotify-black p-4 text-zinc-100">
      <h1 className="mb-6 text-2xl font-bold text-spotify-green">TuneVault</h1>
      <nav>
        <ul className="space-y-2 text-sm">
          {navItems.map((item) => (
            <li key={item.path}>
              <NavLink
                to={item.path}
                className={({ isActive }) =>
                  `block rounded-lg px-3 py-2 transition ${isActive ? 'bg-zinc-800 text-white' : 'text-zinc-400 hover:bg-zinc-800 hover:text-white'}`
                }
              >
                {item.label}
              </NavLink>
            </li>
          ))}
        </ul>
      </nav>
      <div className="mt-8 rounded-lg border border-zinc-800 bg-zinc-950 p-4 text-sm text-zinc-300">
        <p className="font-semibold text-zinc-100">Quick actions</p>
        <p className="mt-2 text-xs text-zinc-400">Use the search page, play your favorites, or edit your profile.</p>
      </div>
    </aside>
  );
}
