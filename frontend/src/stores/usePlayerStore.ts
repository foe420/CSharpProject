import { useState } from 'react';

export type PlayerTrack = {
  title: string;
  artist: string;
};

export type PlayerState = {
  currentTrack: PlayerTrack | null;
  isPlaying: boolean;
};

const initialState: PlayerState = {
  currentTrack: {
    title: 'Starter Track',
    artist: 'TuneVault'
  },
  isPlaying: false
};

export function usePlayerStore() {
  const [state, setState] = useState<PlayerState>(initialState);

  const togglePlay = () => {
    setState((previousState) => ({
      ...previousState,
      isPlaying: !previousState.isPlaying
    }));
  };

  return { state, togglePlay };
}
