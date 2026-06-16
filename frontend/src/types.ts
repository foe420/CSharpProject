// Auth types
export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
}

export interface AuthResponse {
  token: string;
  user: {
    id: string;
    email: string;
  };
}

// User types
export interface UserProfile {
  bio: string;
  avatarPath: string;
}

// Media types
export interface MediaItemSummaryDto {
  id: string;
  title: string;
  artist: string;
  duration: number;
  fileType: 'Audio' | 'Video';
  isFavorited?: boolean;
}

export interface MediaItemDetailDto extends MediaItemSummaryDto {
  description?: string;
  genre?: string;
  createdAt: string;
}

// Playlist types
export interface PlaylistDto {
  id: string;
  title: string;
  isPublic: boolean;
  ownerName: string;
  trackCount: number;
}

export interface PlaylistDetailDto extends PlaylistDto {
  tracks: MediaItemSummaryDto[];
}

// Share types
export interface ShareDto {
  id: string;
  senderName: string;
  receiverName: string;
  sharedAt: string;
  mediaItem?: MediaItemSummaryDto;
  playlistId?: string;
}

// Notification types
export interface NotificationDto {
  id: string;
  type: string;
  payloadJson: string;
  isRead: boolean;
  createdAt: string;
}

// API response types
export interface PagedResultDto<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  unreadCount?: number;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}
