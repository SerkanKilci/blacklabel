import { authorizedGet, authorizedPut } from './authorizedClient';
import type { UserPreferences } from '../types/preferences';

export function getPreferences(): Promise<UserPreferences> {
  return authorizedGet<UserPreferences>('/me/preferences');
}

export function updatePreferences(preferences: UserPreferences): Promise<UserPreferences> {
  return authorizedPut<UserPreferences>('/me/preferences', preferences);
}
