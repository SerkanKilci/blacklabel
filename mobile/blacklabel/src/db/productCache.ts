import type { ProductFound } from '../types/product';
import { getDatabase, isLocalDatabaseSupported } from './database';

interface ProductCacheRow {
  barcode: string;
  data: string;
  cached_at: string;
}

export async function cacheProduct(product: ProductFound): Promise<void> {
  if (!isLocalDatabaseSupported()) {
    return;
  }
  const db = await getDatabase();
  await db.runAsync(
    'INSERT OR REPLACE INTO product_cache (barcode, data, cached_at) VALUES (?, ?, ?)',
    product.barcode,
    JSON.stringify(product),
    new Date().toISOString(),
  );
}

export async function getCachedProduct(barcode: string): Promise<ProductFound | null> {
  if (!isLocalDatabaseSupported()) {
    return null;
  }
  const db = await getDatabase();
  const row = await db.getFirstAsync<ProductCacheRow>('SELECT * FROM product_cache WHERE barcode = ?', barcode);
  return row ? (JSON.parse(row.data) as ProductFound) : null;
}

export async function getCachedProductName(barcode: string): Promise<string | null> {
  const cached = await getCachedProduct(barcode);
  return cached?.name ?? null;
}
