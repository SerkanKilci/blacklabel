import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';

import {
  createHouseholdProfile,
  deleteHouseholdProfile,
  getHouseholdProfiles,
  updateHouseholdProfile,
} from '../api/householdProfiles';
import type { ProfileFormValues } from '../types/preferences';

const HOUSEHOLD_PROFILES_QUERY_KEY = ['householdProfiles'];

export function useHouseholdProfiles() {
  return useQuery({
    queryKey: HOUSEHOLD_PROFILES_QUERY_KEY,
    queryFn: getHouseholdProfiles,
  });
}

export function useCreateHouseholdProfile() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (name: string) => createHouseholdProfile(name),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: HOUSEHOLD_PROFILES_QUERY_KEY });
    },
  });
}

export function useUpdateHouseholdProfile() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, values }: { id: string; values: ProfileFormValues }) => updateHouseholdProfile(id, values),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: HOUSEHOLD_PROFILES_QUERY_KEY });
    },
  });
}

export function useDeleteHouseholdProfile() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => deleteHouseholdProfile(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: HOUSEHOLD_PROFILES_QUERY_KEY });
    },
  });
}
