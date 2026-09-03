import type { QueryClient } from '@tanstack/react-query';
import { Platform } from 'react-native';
import Purchases, { type PurchasesOffering, type PurchasesPackage } from 'react-native-purchases';

import { getSubscription } from '../api/subscription';
import type { Subscription } from '../types/subscription';

const IOS_API_KEY = process.env.EXPO_PUBLIC_REVENUECAT_IOS_KEY;
const ANDROID_API_KEY = process.env.EXPO_PUBLIC_REVENUECAT_ANDROID_KEY;

let isConfigured = false;

/**
 * Configures the RevenueCat SDK once per app session, using our backend's user id as the
 * RevenueCat appUserID so `POST /api/v1/auth/link`-independent purchase events can be tied back
 * to the same account server-side via the RevenueCat webhook (see WebhooksController).
 *
 * No-ops if no API key is configured — this app was built without real App Store
 * Connect / Play Console / RevenueCat project credentials, so purchases cannot be tested here.
 */
export function configurePurchases(userId: string): void {
  if (isConfigured) {
    return;
  }

  const apiKey = Platform.OS === 'ios' ? IOS_API_KEY : ANDROID_API_KEY;
  if (!apiKey) {
    return;
  }

  Purchases.configure({ apiKey, appUserID: userId });
  isConfigured = true;
}

export function isPurchasesConfigured(): boolean {
  return isConfigured;
}

/**
 * Throws (rather than returning null) when RevenueCat isn't configured yet, so that a caller
 * mounted before `configurePurchases` finishes (e.g. the Paywall query firing before the root
 * layout's `ensureUserId().then(configurePurchases)` resolves) gets a React Query retry instead
 * of a permanently-cached null result — returning null here would look like "successfully
 * fetched, no offering" and React Query never retries a successful response.
 */
export async function getCurrentOffering(): Promise<PurchasesOffering | null> {
  if (!isConfigured) {
    throw new Error('RevenueCat is not configured yet');
  }

  const offerings = await Purchases.getOfferings();
  return offerings.current ?? null;
}

export async function purchasePackage(pkg: PurchasesPackage): Promise<void> {
  await Purchases.purchasePackage(pkg);
}

export async function restorePurchases(): Promise<void> {
  await Purchases.restorePurchases();
}

/**
 * RevenueCat confirms a purchase/restore to the client the moment StoreKit does, but our own
 * backend's IsPremium flag only flips once RevenueCat's webhook reaches our server -- a separate,
 * asynchronous delivery (WebhooksController) that can lag a few seconds behind the client-side
 * confirmation. Checking /me/subscription once immediately after a purchase can and does race
 * that webhook, silently showing "not premium" even though the purchase succeeded. This polls a
 * few times with a short delay to give the webhook a realistic window to land before giving up.
 */
export async function waitForPremiumConfirmation(
  queryClient: QueryClient,
  { attempts = 6, delayMs = 1500 }: { attempts?: number; delayMs?: number } = {},
): Promise<boolean> {
  for (let attempt = 0; attempt < attempts; attempt++) {
    const subscription = await getSubscription();
    if (subscription.isPremium) {
      queryClient.setQueryData<Subscription>(['subscription'], subscription);
      return true;
    }
    if (attempt < attempts - 1) {
      await new Promise((resolve) => setTimeout(resolve, delayMs));
    }
  }
  return false;
}
