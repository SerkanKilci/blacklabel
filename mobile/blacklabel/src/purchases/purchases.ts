import { Platform } from 'react-native';
import Purchases, { type PurchasesOffering, type PurchasesPackage } from 'react-native-purchases';

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

export async function getCurrentOffering(): Promise<PurchasesOffering | null> {
  if (!isConfigured) {
    return null;
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
