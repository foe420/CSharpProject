const cards = [
  { title: 'Trending Audio', subtitle: 'Fresh tracks for your day' },
  { title: 'Top Videos', subtitle: 'Curated visual playlists' },
  { title: 'Discover Weekly', subtitle: 'Based on your listening behavior' }
];

export function HomePage() {
  return (
    <section className="space-y-6">
      <header>
        <h2 className="text-3xl font-bold">Good Morning</h2>
        <p className="mt-2 text-sm text-zinc-400">Welcome to your TuneVault dashboard starter template.</p>
      </header>

      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
        {cards.map((card) => (
          <article key={card.title} className="rounded-lg bg-spotify-card p-4 transition hover:bg-zinc-700">
            <h3 className="text-lg font-semibold">{card.title}</h3>
            <p className="mt-2 text-sm text-zinc-300">{card.subtitle}</p>
          </article>
        ))}
      </div>
    </section>
  );
}
