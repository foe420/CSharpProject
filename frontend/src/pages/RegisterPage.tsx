import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { register, saveToken } from '../services/authService';

export function RegisterPage() {
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [displayName, setDisplayName] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setError('');
    setLoading(true);

    try {
      const response = await register({ email, password, displayName });
      saveToken(response.data.token);
      navigate('/home');
    } catch (err) {
      setError('Registration failed. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-black px-4 py-10 text-white">
      <div className="w-full max-w-md space-y-6 rounded-3xl border border-zinc-800 bg-zinc-950 p-8 shadow-xl shadow-black/40">
        <div className="space-y-2 text-center">
          <h1 className="text-3xl font-bold">Create your TuneVault</h1>
          <p className="text-sm text-zinc-400">Register a new account and start listening.</p>
        </div>

        <form className="space-y-4" onSubmit={handleSubmit}>
          <label className="block text-sm text-zinc-300">
            Full name
            <input
              type="text"
              value={displayName}
              onChange={(event) => setDisplayName(event.target.value)}
              className="mt-2 w-full rounded-2xl border border-zinc-800 bg-zinc-900 px-4 py-3 text-sm text-white outline-none transition focus:border-spotify-green"
              required
            />
          </label>

          <label className="block text-sm text-zinc-300">
            Email
            <input
              type="email"
              value={email}
              onChange={(event) => setEmail(event.target.value)}
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
            {loading ? 'Creating account...' : 'Register'}
          </button>
        </form>

        <p className="text-center text-sm text-zinc-500">
          Already have an account?{' '}
          <Link to="/login" className="text-spotify-green transition hover:text-white">
            Sign in
          </Link>
        </p>
      </div>
    </div>
  );
}
