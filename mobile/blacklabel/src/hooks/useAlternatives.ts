import { useQuery } from '@tanstack/react-query';

import { getAlternatives } from '../api/alternatives';

export function useAlternatives(barcode: string, enabled: boolean) {
  return useQuery({
    queryKey: ['alternatives', barcode],
    queryFn: () => getAlternatives(barcode),
    enabled: enabled && Boolean(barcode),
  });
}
