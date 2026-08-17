import * as Crypto from 'expo-crypto';

import { getDatabase, isLocalDatabaseSupported } from './database';

export interface LocalScan {
  id: string;
  barcode: string;
  scannedAt: string;
  scoreAtScanTime: number | null;
  synced: boolean;
}

interface ScanRow {
  id: string;
  barcode: string;
  scanned_at: string;
  score_at_scan_time: number | null;
  synced: number;
}

function toLocalScan(row: ScanRow): LocalScan {
  return {
    id: row.id,
    barcode: row.barcode,
    scannedAt: row.scanned_at,
    scoreAtScanTime: row.score_at_scan_time,
    synced: row.synced === 1,
  };
}

/**
 * Records a scan locally for offline-first history display.
 *
 * `alreadySyncedServerSide` should be `true` whenever this scan came from a successful
 * `GET /products/{barcode}` call — the backend records an authoritative Scan row itself on
 * every successful lookup (it's also where the daily free-tier limit is enforced), so marking
 * the local copy as already synced avoids `syncPendingScans()` re-posting a duplicate. Pass
 * `false` only for scans captured while offline, which genuinely still need to reach the server.
 */
export async function insertScan(
  barcode: string,
  scoreAtScanTime: number | null,
  alreadySyncedServerSide: boolean,
): Promise<LocalScan> {
  const id = Crypto.randomUUID();
  const scannedAt = new Date().toISOString();
  const synced = alreadySyncedServerSide ? 1 : 0;

  if (!isLocalDatabaseSupported()) {
    return { id, barcode, scannedAt, scoreAtScanTime, synced: alreadySyncedServerSide };
  }

  const db = await getDatabase();
  await db.runAsync(
    'INSERT INTO scans (id, barcode, scanned_at, score_at_scan_time, synced) VALUES (?, ?, ?, ?, ?)',
    id,
    barcode,
    scannedAt,
    scoreAtScanTime,
    synced,
  );

  return { id, barcode, scannedAt, scoreAtScanTime, synced: alreadySyncedServerSide };
}

export async function getAllScans(): Promise<LocalScan[]> {
  if (!isLocalDatabaseSupported()) {
    return [];
  }
  const db = await getDatabase();
  const rows = await db.getAllAsync<ScanRow>('SELECT * FROM scans ORDER BY scanned_at DESC');
  return rows.map(toLocalScan);
}

export async function getUnsyncedScans(): Promise<LocalScan[]> {
  if (!isLocalDatabaseSupported()) {
    return [];
  }
  const db = await getDatabase();
  const rows = await db.getAllAsync<ScanRow>('SELECT * FROM scans WHERE synced = 0 ORDER BY scanned_at ASC');
  return rows.map(toLocalScan);
}

export async function markScansSynced(ids: string[]): Promise<void> {
  if (ids.length === 0 || !isLocalDatabaseSupported()) {
    return;
  }

  const db = await getDatabase();
  const placeholders = ids.map(() => '?').join(', ');
  await db.runAsync(`UPDATE scans SET synced = 1 WHERE id IN (${placeholders})`, ...ids);
}
