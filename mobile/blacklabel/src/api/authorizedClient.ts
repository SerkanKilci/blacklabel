import { ensureAuthToken } from '../auth/deviceAuthClient';
import { apiDelete, apiGet, apiPost, apiPut } from './client';

export async function authorizedGet<T>(path: string): Promise<T> {
  const token = await ensureAuthToken();
  return apiGet<T>(path, { token });
}

export async function authorizedPost<T>(path: string, body: unknown): Promise<T> {
  const token = await ensureAuthToken();
  return apiPost<T>(path, body, { token });
}

export async function authorizedPut<T>(path: string, body: unknown): Promise<T> {
  const token = await ensureAuthToken();
  return apiPut<T>(path, body, { token });
}

export async function authorizedDelete(path: string): Promise<void> {
  const token = await ensureAuthToken();
  return apiDelete(path, { token });
}
