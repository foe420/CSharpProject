const navItems = ['Home', 'Search', 'Your Library', 'Playlists', 'Liked Songs'];

export function Sidebar() {
  return (
    <aside className="w-64 rounded-lg bg-spotify-black p-4">
      <h1 className="mb-6 text-2xl font-bold text-spotify-green">TuneVault</h1>
      <nav>
        <ul className="space-y-3 text-sm text-zinc-200">
          {navItems.map((item) => (
            <li key={item} className="cursor-pointer rounded px-3 py-2 transition hover:bg-zinc-800">
              {item}
            </li>
          ))}
        </ul>
      </nav>
    </aside>
  );
}
