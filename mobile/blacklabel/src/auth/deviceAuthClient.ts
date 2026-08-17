import * as Crypto from 'expo-crypto';

import { authenticateDevice } from '../api/auth';
import * as SecureStore from './secureStorage';

const DEVICE_ID_KEY = 'blacklabel_device_id';
const AUTH_TOKEN_KEY = 'blacklabel_auth_token';
const USER_ID_KEY = 'blacklabel_user_id';

let cachedToken: string | null = null;
let cachedUserId: string | null = null;
let inFlightAuth: Promise<string> | null = null;

async function getOrCreateDeviceId(): Promise<string> {
  const existing = await SecureStore.getItemAsync(DEVICE_ID_KEY);
  if (existing) {
    return existing;
  }

  const newId = Crypto.randomUUID();
  await SecureStore.setItemAsync(DEVICE_ID_KEY, newId);
  return newId;
}

async function authenticateAndStoreToken(): Promise<string> {
  const deviceId = await getOrCreateDeviceId();
  const response = await authenticateDevice(deviceId);
  await SecureStore.setItemAsync(AUTH_TOKEN_KEY, response.token);
  await SecureStore.setItemAsync(USER_ID_KEY, response.userId);
  cachedToken = response.token;
  cachedUserId = response.userId;
  return response.token;
}

/**
 * Returns a bearer token for the current device, authenticating (and persisting
 * the result) on first use. Concurrent callers share a single in-flight request.
 */
export async function ensureAuthToken(): Promise<string> {
  if (cachedToken) {
    return cachedToken;
  }

  const stored = await SecureStore.getItemAsync(AUTH_TOKEN_KEY);
  if (stored) {
    cachedToken = stored;
    return stored;
  }

  if (!inFlightAuth) {
    inFlightAuth = authenticateAndStoreToken().finally(() => {
      inFlightAuth = null;
    });
  }

  return inFlightAuth;
}

/**
 * Overwrites the cached/stored bearer token after `POST /auth/link` issues a fresh one for the
 * same account. The user id is unchanged by linking (it identifies the same device-created
 * account, now with a provider attached), so only the token needs updating.
 */
export async function setAuthToken(token: string): Promise<void> {
  await SecureStore.setItemAsync(AUTH_TOKEN_KEY, token);
  cachedToken = token;
}

/**
 * Clears the cached/stored token and user id after account deletion. Deliberately keeps the
 * device id — the next `ensureAuthToken()` call re-registers it as a brand new anonymous
 * account, which is exactly the "fresh start" a deleted account should get, without needing a
 * new random device id.
 */
export async function clearAuth(): Promise<void> {
  await SecureStore.deleteItemAsync(AUTH_TOKEN_KEY);
  await SecureStore.deleteItemAsync(USER_ID_KEY);
  cachedToken = null;
  cachedUserId = null;
}

/**
 * Returns our backend's user id for the current device, authenticating first if needed.
 * Used to configure RevenueCat's appUserID so purchase events tie back to this account.
 */
export async function ensureUserId(): Promise<string> {
  if (cachedUserId) {
    return cachedUserId;
  }

  await ensureAuthToken();

  if (cachedUserId) {
    return cachedUserId;
  }

  const stored = await SecureStore.getItemAsync(USER_ID_KEY);
  if (stored) {
    cachedUserId = stored;
    return stored;
  }

  throw new Error('User id unavailable after authentication.');
}
