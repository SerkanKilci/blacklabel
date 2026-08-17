import { useQuery } from '@tanstack/react-query';

import { getProductByBarcode } from '../api/products';

export function useProduct(barcode: string | undefined) {
  return useQuery({
    queryKey: ['product', barcode],
    queryFn: () => getProductByBarcode(barcode as string),
    enabled: Boolean(barcode),
    retry: 1,
  });
}
