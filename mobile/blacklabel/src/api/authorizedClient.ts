import { clearAuth, ensureAuthToken } from '../auth/deviceAuthClient';
import { ApiError, apiDelete, apiGet, apiPost, apiPut } from './client';

/**
 * Runs `call` with a valid bearer token, and — if the token turns out to be rejected (e.g. it
 * was issued by a backend/secret that no longer matches, such as after pointing the app at a
 * different deployment) — discards it and retries exactly once with a freshly issued token.
 * Without this, a stale cached token permanently 401s every authorized call until the user
 * manually deletes and reinstalls the app, since `ensureAuthToken` trusts whatever's cached
 * without ever validating it against the current backend.
 */
async function withTokenRetry<T>(call: (token: string) => Promise<T>): Promise<T> {
  const token = await ensureAuthToken();
  try {
    return await call(token);
  } catch (error) {
    if (error instanceof ApiError && error.status === 401) {
      await clearAuth();
      const freshToken = await ensureAuthToken();
      return call(freshToken);
    }
    throw error;
  }
}

export async function authorizedGet<T>(path: string): Promise<T> {
  return withTokenRetry((token) => apiGet<T>(path, { token }));
}

export async function authorizedPost<T>(path: string, body: unknown): Promise<T> {
  return withTokenRetry((token) => apiPost<T>(path, body, { token }));
}

export async function authorizedPut<T>(path: string, body: unknown): Promise<T> {
  return withTokenRetry((token) => apiPut<T>(path, body, { token }));
}

export async function authorizedDelete(path: string): Promise<void> {
  return withTokenRetry((token) => apiDelete(path, { token }));
}
