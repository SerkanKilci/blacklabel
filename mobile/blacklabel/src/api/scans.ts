import { authorizedGet, authorizedPost } from './authorizedClient';
import type { CreateScanRequest, ScanPage, ScanRecord } from '../types/scan';

export function getScanHistory(page = 1, pageSize = 20): Promise<ScanPage> {
  return authorizedGet<ScanPage>(`/scans?page=${page}&pageSize=${pageSize}`);
}

export function createScans(scans: CreateScanRequest[]): Promise<ScanRecord[]> {
  return authorizedPost<ScanRecord[]>('/scans', { scans });
}
