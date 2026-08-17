import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';

import { getPreferences, updatePreferences } from '../api/preferences';
import type { UserPreferences } from '../types/preferences';

const PREFERENCES_QUERY_KEY = ['preferences'];

export function usePreferences() {
  return useQuery({
    queryKey: PREFERENCES_QUERY_KEY,
    queryFn: getPreferences,
  });
}

export function useUpdatePreferences() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (preferences: UserPreferences) => updatePreferences(preferences),
    onSuccess: (data) => {
      queryClient.setQueryData(PREFERENCES_QUERY_KEY, data);
    },
  });
}
