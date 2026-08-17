import { createScans } from '../api/scans';
import { getUnsyncedScans, markScansSynced } from '../db/scanHistory';

export async function syncPendingScans(): Promise<void> {
  const unsynced = await getUnsyncedScans();
  if (unsynced.length === 0) {
    return;
  }

  try {
    await createScans(
      unsynced.map((scan) => ({
        barcode: scan.barcode,
        scannedAt: scan.scannedAt,
        scoreAtScanTime: scan.scoreAtScanTime,
      })),
    );
    await markScansSynced(unsynced.map((scan) => scan.id));
  } catch {
    // Network or server error — scans stay unsynced locally and are retried on the next opportunity.
  }
}
