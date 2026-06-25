import { create } from 'zustand';
import apiClient from '../services/apiClient';

interface NotificationState {
  unreadCount: number;
  setUnreadCount: (count: number) => void;
  decrementUnreadCount: () => void;
  clearUnreadCount: () => void;
  fetchUnreadCount: () => Promise<void>;
}

export const useNotificationStore = create<NotificationState>((set) => ({
  unreadCount: 0,
  setUnreadCount: (count) => set({ unreadCount: count }),
  decrementUnreadCount: () => set((state) => ({ unreadCount: Math.max(0, state.unreadCount - 1) })),
  clearUnreadCount: () => set({ unreadCount: 0 }),
  fetchUnreadCount: async () => {
    try {
      const res = await apiClient.get('/notifications');
      set({ unreadCount: res.data.unreadCount || 0 });
    } catch (error) {
      console.error('Error fetching notification count:', error);
    }
  }
}));
