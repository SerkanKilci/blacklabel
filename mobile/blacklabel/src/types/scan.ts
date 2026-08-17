export interface ScanRecord {
  id: string;
  barcode: string;
  productId: string | null;
  scannedAt: string;
  scoreAtScanTime: number | null;
}

export interface ScanPage {
  items: ScanRecord[];
  page: number;
  pageSize: number;
  totalCount: number;
}

export interface CreateScanRequest {
  barcode: string;
  scannedAt: string;
  scoreAtScanTime: number | null;
}
