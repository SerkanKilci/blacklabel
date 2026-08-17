import { Platform } from 'react-native';
import * as SQLite from 'expo-sqlite';

let dbPromise: Promise<SQLite.SQLiteDatabase> | null = null;

async function openAndMigrate(): Promise<SQLite.SQLiteDatabase> {
  const db = await SQLite.openDatabaseAsync('blacklabel.db');

  await db.execAsync(`
    CREATE TABLE IF NOT EXISTS scans (
      id TEXT PRIMARY KEY NOT NULL,
      barcode TEXT NOT NULL,
      scanned_at TEXT NOT NULL,
      score_at_scan_time INTEGER,
      synced INTEGER NOT NULL DEFAULT 0
    );

    CREATE TABLE IF NOT EXISTS product_cache (
      barcode TEXT PRIMARY KEY NOT NULL,
      data TEXT NOT NULL,
      cached_at TEXT NOT NULL
    );
  `);

  return db;
}

/**
 * expo-sqlite's web backend runs in a Worker backed by the Origin Private File System, where a
 * stale access handle from a previous Fast Refresh (or another tab) makes every reopen throw —
 * and unlike a native rebuild, a browser reload doesn't reliably clear that handle. Web was never
 * a target platform for this app (no camera, no native sign-in — see README), so callers must
 * check this before touching the database at all rather than let the worker spawn and fail.
 */
export function isLocalDatabaseSupported(): boolean {
  return Platform.OS !== 'web';
}

export function getDatabase(): Promise<SQLite.SQLiteDatabase> {
  if (!dbPromise) {
    dbPromise = openAndMigrate();
  }
  return dbPromise;
}

export function initDatabase(): void {
  if (!isLocalDatabaseSupported()) {
    return;
  }
  void getDatabase();
}

/** Wipes local scan history and the cached product list — used after account deletion so the
 * fresh anonymous account that replaces it doesn't inherit the old one's local data. */
export async function clearLocalData(): Promise<void> {
  if (!isLocalDatabaseSupported()) {
    return;
  }
  const db = await getDatabase();
  await db.execAsync('DELETE FROM scans; DELETE FROM product_cache;');
}
