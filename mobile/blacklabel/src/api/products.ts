import { authorizedGet } from './authorizedClient';
import type { ProductLookupResponse } from '../types/product';

export function getProductByBarcode(barcode: string): Promise<ProductLookupResponse> {
  return authorizedGet<ProductLookupResponse>(`/products/${encodeURIComponent(barcode)}`);
}
