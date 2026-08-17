import * as AppleAuthentication from 'expo-apple-authentication';
import { Platform } from 'react-native';
import { GoogleSignin } from '@react-native-google-signin/google-signin';

import { linkAccount } from '../api/auth';
import { setAuthToken } from './deviceAuthClient';
import type { AuthResponse } from '../types/auth';

const GOOGLE_WEB_CLIENT_ID = process.env.EXPO_PUBLIC_GOOGLE_WEB_CLIENT_ID;

let googleConfigured = false;

export async function isAppleSignInAvailable(): Promise<boolean> {
  if (Platform.OS !== 'ios') {
    return false;
  }
  return AppleAuthentication.isAvailableAsync();
}

export function isGoogleSignInConfigured(): boolean {
  return Boolean(GOOGLE_WEB_CLIENT_ID);
}

function ensureGoogleConfigured(): void {
  if (googleConfigured || !GOOGLE_WEB_CLIENT_ID) {
    return;
  }
  GoogleSignin.configure({ webClientId: GOOGLE_WEB_CLIENT_ID });
  googleConfigured = true;
}

/**
 * Runs the native Sign in with Apple flow and links the result to the current device account.
 * Returns null if the user cancels; throws for any other failure (network, verification, etc.)
 * so the caller can show an error state.
 */
export async function signInWithApple(): Promise<AuthResponse | null> {
  let credential: AppleAuthentication.AppleAuthenticationCredential;
  try {
    credential = await AppleAuthentication.signInAsync({
      requestedScopes: [
        AppleAuthentication.AppleAuthenticationScope.FULL_NAME,
        AppleAuthentication.AppleAuthenticationScope.EMAIL,
      ],
    });
  } catch (error) {
    if (isCancelError(error)) {
      return null;
    }
    throw error;
  }

  if (!credential.identityToken) {
    throw new Error('Apple did not return an identity token.');
  }

  return linkAndStoreToken('apple', credential.identityToken);
}

/**
 * Runs the native Google Sign-In flow and links the result to the current device account.
 * Returns null if not configured (no EXPO_PUBLIC_GOOGLE_WEB_CLIENT_ID) or if the user cancels.
 */
export async function signInWithGoogle(): Promise<AuthResponse | null> {
  if (!isGoogleSignInConfigured()) {
    return null;
  }
  ensureGoogleConfigured();

  await GoogleSignin.hasPlayServices({ showPlayServicesUpdateDialog: true });
  const result = await GoogleSignin.signIn();

  if (result.type !== 'success' || !result.data.idToken) {
    return null;
  }

  return linkAndStoreToken('google', result.data.idToken);
}

async function linkAndStoreToken(provider: 'apple' | 'google', identityToken: string): Promise<AuthResponse> {
  const response = await linkAccount(provider, identityToken);
  await setAuthToken(response.token);
  return response;
}

function isCancelError(error: unknown): boolean {
  const code = (error as { code?: string } | null)?.code;
  return code === 'ERR_REQUEST_CANCELED' || code === 'ERR_CANCELED';
}
