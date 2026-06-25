import { create } from 'zustand';

export type PlayerTrack = {
  id: string;
  title: string;
  artist: string;
  duration: number;
  src: string;
  fileType: 'audio' | 'video';
};

export type PlayerState = {
  currentTrack: PlayerTrack | null;
  isPlaying: boolean;
  volume: number;
  position: number;
  queue: PlayerTrack[];
  playTrack: (track: PlayerTrack) => void;
  pause: () => void;
  resume: () => void;
  next: () => void;
  previous: () => void;
  setVolume: (value: number) => void;
  setPosition: (value: number) => void;
  stop: () => void;
  addToQueue: (track: PlayerTrack) => void;
};

export const usePlayerStore = create<PlayerState>((set, get) => ({
  currentTrack: null,
  isPlaying: false,
  volume: 0.7,
  position: 0,
  queue: [],
  playTrack: (track) =>
    set((state) => ({
      currentTrack: track,
      isPlaying: true,
      position: 0,
      queue: [track, ...state.queue.filter((item) => item.id !== track.id)]
    })),
  pause: () => set({ isPlaying: false }),
  resume: () => set({ isPlaying: true }),
  stop: () => set({ currentTrack: null, isPlaying: false, position: 0 }),
  next: () => {
    const { queue, currentTrack } = get();
    if (!queue.length) {
      return;
    }
    const currentIndex = currentTrack ? queue.findIndex((item) => item.id === currentTrack.id) : -1;
    const nextTrack = queue[currentIndex + 1] ?? queue[0];
    set({ currentTrack: nextTrack, isPlaying: true, position: 0 });
  },
  previous: () => {
    const { queue, currentTrack } = get();
    if (!queue.length) {
      return;
    }
    const currentIndex = currentTrack ? queue.findIndex((item) => item.id === currentTrack.id) : -1;
    const previousTrack = queue[currentIndex - 1] ?? queue[0];
    set({ currentTrack: previousTrack, isPlaying: true, position: 0 });
  },
  setVolume: (value) => set({ volume: value }),
  setPosition: (value) => set({ position: value }),
  addToQueue: (track) =>
    set((state) => ({
      queue: state.queue.some((item) => item.id === track.id) ? state.queue : [...state.queue, track]
    }))
}));
