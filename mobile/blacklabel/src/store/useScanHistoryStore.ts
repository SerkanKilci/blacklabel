import { create } from 'zustand';

export interface ScanHistoryEntry {
  barcode: string;
  scannedAt: number;
}

interface ScanHistoryState {
  recentScans: ScanHistoryEntry[];
  addScan: (barcode: string) => void;
}

const MAX_RECENT_SCANS = 10;

export const useScanHistoryStore = create<ScanHistoryState>((set) => ({
  recentScans: [],
  addScan: (barcode) =>
    set((state) => ({
      recentScans: [
        { barcode, scannedAt: Date.now() },
        ...state.recentScans.filter((entry) => entry.barcode !== barcode),
      ].slice(0, MAX_RECENT_SCANS),
    })),
}));
