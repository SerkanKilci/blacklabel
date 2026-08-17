import { authorizedGet } from './authorizedClient';
import type { ProductFound } from '../types/product';

export function getAlternatives(barcode: string): Promise<ProductFound[]> {
  return authorizedGet<ProductFound[]>(`/products/${encodeURIComponent(barcode)}/alternatives`);
}
