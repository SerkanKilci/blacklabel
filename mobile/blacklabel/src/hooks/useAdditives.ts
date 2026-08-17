import { useQuery } from '@tanstack/react-query';

import { getAdditives } from '../api/additives';

export function useAdditives() {
  return useQuery({
    queryKey: ['additives'],
    queryFn: getAdditives,
    staleTime: 1000 * 60 * 60 * 24,
  });
}
