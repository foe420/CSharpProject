import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { login, saveToken } from '../services/authService';

export function LoginPage() {
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setError('');

    if (!emailPattern.test(email)) {
      setError('Please enter a valid email address.');
      return;
    }

    setLoading(true);

    try {
      const response = await login({ email, password });
      saveToken(response.data.token);
      navigate('/home');
    } catch (err) {
      setError('Unable to login. Please check credentials.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-black px-4 py-10 text-white">
      <div className="w-full max-w-md space-y-6 rounded-3xl border border-zinc-800 bg-zinc-950 p-8 shadow-xl shadow-black/40">
        <div className="space-y-2 text-center">
          <div className="flex items-center justify-between gap-4">
            <h1 className="text-3xl font-bold">Welcome back</h1>
            <Link
              to="/home"
              className="text-sm text-spotify-green transition hover:text-white"
            >
              ← Back to home
            </Link>
          </div>
          <p className="text-sm text-zinc-400">Sign in to continue to TuneVault.</p>
        </div>

        <form className="space-y-4" onSubmit={handleSubmit}>
          <label className="block text-sm text-zinc-300">
            Email
            <input
              type="email"
              value={email}
              onChange={(event) => setEmail(event.target.value)}
              pattern={"^[^\\s@]+@[^\\s@]+\\.[^\\s@]+$"}
              title="Enter a valid email address"
              className="mt-2 w-full rounded-2xl border border-zinc-800 bg-zinc-900 px-4 py-3 text-sm text-white outline-none transition focus:border-spotify-green"
              required
            />
          </label>

          <label className="block text-sm text-zinc-300">
            Password
            <input
              type="password"
              value={password}
              onChange={(event) => setPassword(event.target.value)}
              className="mt-2 w-full rounded-2xl border border-zinc-800 bg-zinc-900 px-4 py-3 text-sm text-white outline-none transition focus:border-spotify-green"
              required
            />
          </label>

          {error && <p className="text-sm text-red-400">{error}</p>}

          <button
            type="submit"
            disabled={loading}
            className="w-full rounded-full bg-spotify-green px-4 py-3 text-sm font-semibold text-black transition hover:brightness-110 disabled:cursor-not-allowed disabled:opacity-60"
          >
            {loading ? 'Signing in...' : 'Sign in'}
          </button>
        </form>

        <p className="text-center text-sm text-zinc-500">
          New to TuneVault?{' '}
          <Link to="/register" className="text-spotify-green transition hover:text-white">
            Create an account
          </Link>
        </p>
      </div>
    </div>
  );
}
