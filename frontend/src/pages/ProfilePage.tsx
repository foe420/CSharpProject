import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import apiClient from '../services/apiClient';
import { getUserEmailFromToken, logout } from '../services/authService';

interface UserProfile {
  bio: string;
  avatarPath: string;
}

export function ProfilePage() {
  const [profile, setProfile] = useState<UserProfile>({ bio: '', avatarPath: '' });
  const [isEditing, setIsEditing] = useState(false);
  const [saving, setSaving] = useState(false);
  const email = getUserEmailFromToken();

  useEffect(() => {
    apiClient.get('/users/me/profile')
      .then(res => setProfile(res.data))
      .catch(err => console.error(err));
  }, []);

  const handleSave = async () => {
    setSaving(true);
    try {
      await apiClient.put('/users/me/profile', profile);
      setIsEditing(false);
      alert('Update successful!');
    } catch (err) {
      console.error(err);
      alert('Profile update failed');
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="mx-auto max-w-2xl rounded-2xl border border-zinc-800 bg-[#131313] p-8 text-white">
      <div className="mb-8 flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 className="mb-4 text-3xl font-bold">Profile</h1>
          <p className="text-sm text-zinc-400">Manage your account and logout.</p>
        </div>
        <div className="flex items-center gap-3">
          <Link
            to="/home"
            className="rounded-full border border-zinc-700 bg-zinc-950 px-4 py-2 text-sm text-zinc-200 transition hover:border-spotify-green hover:text-white"
          >
            ← Home
          </Link>
          <button
            onClick={() => logout()}
            className="rounded-full bg-rose-500 px-4 py-2 text-sm font-semibold text-white transition hover:bg-rose-400"
          >
            Logout
          </button>
        </div>
      </div>
      <div className="flex items-center gap-6 mb-8">
        <div className="flex h-24 w-24 items-center justify-center rounded-full bg-zinc-800 text-3xl overflow-hidden">
          {profile.avatarPath ? <img src={profile.avatarPath} alt="Avatar" className="h-full w-full object-cover" /> : '👤'}
        </div>
        <div>
          <p className="text-xl font-semibold">{email || 'User'}</p>
          <p className="text-sm text-spotify-green">TuneVault member</p>
        </div>
      </div>
      <div className="space-y-4">
        <div>
          <label className="block text-sm font-medium text-zinc-400 mb-1">Avatar URL</label>
          <input type="text" disabled={!isEditing} value={profile.avatarPath || ''} onChange={(e) => setProfile({ ...profile, avatarPath: e.target.value })} className="w-full rounded-lg border border-zinc-700 bg-zinc-900 p-3 text-white focus:border-spotify-green focus:outline-none disabled:opacity-50" />
        </div>
        <div>
          <label className="block text-sm font-medium text-zinc-400 mb-1">Bio</label>
          <textarea disabled={!isEditing} value={profile.bio || ''} onChange={(e) => setProfile({ ...profile, bio: e.target.value })} className="h-32 w-full rounded-lg border border-zinc-700 bg-zinc-900 p-3 text-white focus:border-spotify-green focus:outline-none disabled:opacity-50" />
        </div>
        <div className="pt-4">
          {isEditing ? (
            <div className="flex gap-3">
              <button onClick={handleSave} disabled={saving} className="rounded-full bg-spotify-green px-6 py-2 font-semibold text-black hover:brightness-110">{saving ? 'Saving...' : 'Save changes'}</button>
              <button onClick={() => setIsEditing(false)} className="rounded-full border border-zinc-700 px-6 py-2 text-zinc-300 hover:bg-zinc-800">Cancel</button>
            </div>
          ) : (
            <button onClick={() => setIsEditing(true)} className="rounded-full border border-zinc-700 px-6 py-2 text-zinc-300 hover:bg-zinc-800 hover:text-white transition">Edit profile</button>
          )}
        </div>
      </div>
    </div>
  );
}