import { authorizedDelete, authorizedGet, authorizedPost } from './authorizedClient';
import { apiPost } from './client';
import type { AuthResponse } from '../types/auth';
import type { Profile } from '../types/profile';

export function authenticateDevice(deviceId: string): Promise<AuthResponse> {
  return apiPost<AuthResponse>('/auth/device', { deviceId });
}

export function linkAccount(provider: 'apple' | 'google', identityToken: string): Promise<AuthResponse> {
  return authorizedPost<AuthResponse>('/auth/link', { provider, identityToken });
}

export function getProfile(): Promise<Profile> {
  return authorizedGet<Profile>('/me/profile');
}

export function deleteAccount(): Promise<void> {
  return authorizedDelete('/me');
}
