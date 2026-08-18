import { authorizedDelete, authorizedGet, authorizedPost, authorizedPut } from './authorizedClient';
import type { HouseholdProfile, ProfileFormValues } from '../types/preferences';

export function getHouseholdProfiles(): Promise<HouseholdProfile[]> {
  return authorizedGet<HouseholdProfile[]>('/me/household-profiles');
}

export function createHouseholdProfile(name: string): Promise<HouseholdProfile> {
  return authorizedPost<HouseholdProfile>('/me/household-profiles', { name });
}

export function updateHouseholdProfile(id: string, values: ProfileFormValues): Promise<HouseholdProfile> {
  return authorizedPut<HouseholdProfile>(`/me/household-profiles/${id}`, values);
}

export function deleteHouseholdProfile(id: string): Promise<void> {
  return authorizedDelete(`/me/household-profiles/${id}`);
}
