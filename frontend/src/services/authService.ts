import apiClient from './apiClient';

export type AuthResponse = {
  token: string;
};

export type RegisterRequest = {
  email: string;
  password: string;
  displayName: string;
};

export type LoginRequest = {
  email: string;
  password: string;
};

export function login(request: LoginRequest) {
  return apiClient.post<AuthResponse>('/auth/login', request);
}

export function register(request: RegisterRequest) {
  return apiClient.post<AuthResponse>('/auth/register', request);
}

export function logout() {
  localStorage.removeItem('tunevault_token');
  window.location.href = '/login';
}

export function saveToken(token: string) {
  localStorage.setItem('tunevault_token', token);
}

export function getToken() {
  return localStorage.getItem('tunevault_token');
}

export function isAuthenticated() {
  return Boolean(getToken());
}

export function getUserEmailFromToken() {
  const token = getToken();
  if (!token) {
    return null;
  }

  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.email as string | null;
  } catch {
    return null;
  }
}
