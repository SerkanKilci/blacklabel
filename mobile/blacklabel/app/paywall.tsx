import { useQuery, useQueryClient } from '@tanstack/react-query';
import * as AppleAuthentication from 'expo-apple-authentication';
import { useRouter } from 'expo-router';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { ActivityIndicator, Pressable, ScrollView, StyleSheet, Text, View } from 'react-native';
import { PACKAGE_TYPE, type PurchasesPackage } from 'react-native-purchases';

import { Screen } from '../src/components/Screen';
import { useProfile } from '../src/hooks/useProfile';
import { useSocialLink } from '../src/hooks/useSocialLink';
import { getCurrentOffering, isPurchasesConfigured, purchasePackage, restorePurchases } from '../src/purchases/purchases';

const FEATURE_KEYS = [
  'unlimitedScans',
  'personalWarnings',
  'compareMode',
  'unlimitedAnalysis',
  'alternatives',
  'unlimitedHistory',
  'offlineMode',
] as const;

const DURATION_KEY_BY_PACKAGE_TYPE: Partial<Record<PACKAGE_TYPE, string>> = {
  [PACKAGE_TYPE.LIFETIME]: 'lifetime',
  [PACKAGE_TYPE.ANNUAL]: 'annual',
  [PACKAGE_TYPE.SIX_MONTH]: 'sixMonth',
  [PACKAGE_TYPE.THREE_MONTH]: 'threeMonth',
  [PACKAGE_TYPE.TWO_MONTH]: 'twoMonth',
  [PACKAGE_TYPE.MONTHLY]: 'monthly',
  [PACKAGE_TYPE.WEEKLY]: 'weekly',
};

export default function PaywallScreen() {
  const { t } = useTranslation();
  const router = useRouter();
  const queryClient = useQueryClient();
  const [isPurchasing, setIsPurchasing] = useState(false);
  const [isRestoring, setIsRestoring] = useState(false);
  const [feedback, setFeedback] = useState<string | null>(null);

  const { data: profile } = useProfile();
  const { appleAvailable, googleConfigured, linkingProvider, hasError: linkError, handleSignIn } = useSocialLink();
  const isAccountLinked = Boolean(profile?.hasAppleLink || profile?.hasGoogleLink);

  // RevenueCat is configured at app launch with our own (anonymous, device-based) user id as its
  // appUserID -- see src/purchases/purchases.ts -- so purchasing never depends on being signed in
  // with Apple/Google. Not gating this on isAccountLinked is required by App Store Guideline
  // 5.1.1(v): apps cannot require registration to buy an IAP that isn't account-based.
  const { data: offering, isLoading } = useQuery({
    queryKey: ['revenuecat-offering'],
    queryFn: getCurrentOffering,
  });

  const handlePurchase = async (pkg: PurchasesPackage) => {
    setIsPurchasing(true);
    setFeedback(null);
    try {
      await purchasePackage(pkg);
      await queryClient.invalidateQueries({ queryKey: ['subscription'] });
      router.back();
    } catch {
      setFeedback(t('paywall.purchaseError'));
    } finally {
      setIsPurchasing(false);
    }
  };

  const handleRestore = async () => {
    setIsRestoring(true);
    setFeedback(null);
    try {
      await restorePurchases();
      await queryClient.invalidateQueries({ queryKey: ['subscription'] });
      setFeedback(t('paywall.restoreSuccess'));
    } catch {
      setFeedback(t('paywall.purchaseError'));
    } finally {
      setIsRestoring(false);
    }
  };

  return (
    <Screen style={styles.container}>
    <ScrollView contentContainerStyle={styles.content}>
      <Text style={styles.title}>{t('paywall.title')}</Text>
      <Text style={styles.subtitle}>{t('paywall.subtitle')}</Text>

      <Text style={styles.featuresTitle}>{t('paywall.featuresTitle')}</Text>
      {FEATURE_KEYS.map((key) => (
        <View key={key} style={styles.featureRow}>
          <View style={styles.featureDot} />
          <Text style={styles.featureText}>{t(`paywall.feature.${key}`)}</Text>
        </View>
      ))}

      {/* Purchasing never requires being signed in -- RevenueCat is already configured with our
          own anonymous device account as its appUserID at app launch (see purchases.ts). Gating
          this behind Apple/Google sign-in would violate Guideline 5.1.1(v): apps can't require
          registration to buy an IAP that isn't account-based. */}
      <View style={styles.packagesSection}>
        {!isPurchasesConfigured() ? (
          <Text style={styles.notConfiguredText}>{t('paywall.notConfigured')}</Text>
        ) : isLoading ? (
          <ActivityIndicator size="large" color="#1A1A1A" />
        ) : offering && offering.availablePackages.length > 0 ? (
          offering.availablePackages.map((pkg) => {
            const durationKey = DURATION_KEY_BY_PACKAGE_TYPE[pkg.packageType];
            return (
              <Pressable
                key={pkg.identifier}
                style={styles.packageButton}
                onPress={() => void handlePurchase(pkg)}
                disabled={isPurchasing}
              >
                <Text style={styles.packageTitle}>{pkg.product.title}</Text>
                <Text style={styles.packagePrice}>{pkg.product.priceString}</Text>
                {durationKey && <Text style={styles.packageDuration}>{t(`paywall.duration.${durationKey}`)}</Text>}
              </Pressable>
            );
          })
        ) : (
          <Text style={styles.notConfiguredText}>{t('paywall.notConfigured')}</Text>
        )}
      </View>

      {isPurchasesConfigured() && (
        <>
          <Text style={styles.legalNoticeText}>{t('paywall.autoRenewalNotice')}</Text>

          <Text style={styles.legalIntroText}>{t('paywall.legalIntro')}</Text>
          <Pressable onPress={() => router.push('/settings/terms')}>
            <Text style={styles.legalLinkText}>{t('settings.termsLink')}</Text>
          </Pressable>
          <Pressable onPress={() => router.push('/settings/privacy')}>
            <Text style={styles.legalLinkText}>{t('settings.privacyLink')}</Text>
          </Pressable>
        </>
      )}

      {/* Optional: linking Apple/Google lets the same premium entitlement be recognized on
          another device. Never blocks purchase itself (see above). */}
      {!isAccountLinked && (appleAvailable || googleConfigured) && (
        <View style={styles.signInGate}>
          <Text style={styles.signInGateTitle}>{t('paywall.signInOptionalTitle')}</Text>
          <Text style={styles.signInGateText}>{t('paywall.signInOptionalMessage')}</Text>

          {linkingProvider !== null ? (
            <ActivityIndicator size="small" color="#1A1A1A" style={styles.signInLoading} />
          ) : (
            <>
              {appleAvailable && (
                <AppleAuthentication.AppleAuthenticationButton
                  buttonType={AppleAuthentication.AppleAuthenticationButtonType.SIGN_IN}
                  buttonStyle={AppleAuthentication.AppleAuthenticationButtonStyle.BLACK}
                  cornerRadius={12}
                  style={styles.appleButton}
                  onPress={() => void handleSignIn('apple')}
                />
              )}

              {googleConfigured && (
                <Pressable style={styles.signInButton} onPress={() => void handleSignIn('google')}>
                  <Text style={styles.signInButtonText}>{t('settings.signInWithGoogle')}</Text>
                </Pressable>
              )}
            </>
          )}

          {linkError && <Text style={styles.feedbackText}>{t('settings.linkError')}</Text>}
        </View>
      )}

      {feedback && <Text style={styles.feedbackText}>{feedback}</Text>}

      <Pressable style={styles.restoreButton} onPress={() => void handleRestore()} disabled={isRestoring}>
        <Text style={styles.restoreButtonText}>{isRestoring ? t('paywall.restoring') : t('paywall.restore')}</Text>
      </Pressable>

      <Pressable style={styles.closeButton} onPress={() => router.back()}>
        <Text style={styles.closeButtonText}>{t('paywall.close')}</Text>
      </Pressable>
    </ScrollView>
    </Screen>
  );
}

const styles = StyleSheet.create({
  container: {
    backgroundColor: '#FFFFFF',
  },
  content: {
    paddingHorizontal: 24,
    paddingTop: 16,
    paddingBottom: 40,
  },
  title: {
    fontSize: 26,
    fontWeight: '700',
    color: '#1A1A1A',
  },
  subtitle: {
    fontSize: 15,
    color: '#6B6B6B',
    marginTop: 8,
  },
  featuresTitle: {
    fontSize: 14,
    fontWeight: '700',
    color: '#1A1A1A',
    marginTop: 32,
    marginBottom: 12,
  },
  featureRow: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingVertical: 6,
  },
  featureDot: {
    width: 6,
    height: 6,
    borderRadius: 3,
    backgroundColor: '#1A1A1A',
    marginRight: 12,
  },
  featureText: {
    fontSize: 14,
    color: '#1A1A1A',
    flex: 1,
  },
  packagesSection: {
    marginTop: 32,
  },
  signInGate: {
    borderWidth: 1,
    borderColor: '#E0E0E0',
    borderRadius: 12,
    padding: 20,
  },
  signInGateTitle: {
    fontSize: 16,
    fontWeight: '700',
    color: '#1A1A1A',
  },
  signInGateText: {
    fontSize: 13,
    color: '#6B6B6B',
    marginTop: 6,
    marginBottom: 16,
    lineHeight: 18,
  },
  signInButton: {
    borderWidth: 1,
    borderColor: '#1A1A1A',
    borderRadius: 12,
    paddingVertical: 12,
    alignItems: 'center',
    marginBottom: 10,
  },
  signInButtonText: {
    fontSize: 14,
    fontWeight: '600',
    color: '#1A1A1A',
  },
  appleButton: {
    width: '100%',
    height: 44,
    marginBottom: 10,
  },
  signInLoading: {
    marginVertical: 8,
  },
  packageButton: {
    borderWidth: 1,
    borderColor: '#1A1A1A',
    borderRadius: 12,
    paddingVertical: 16,
    paddingHorizontal: 20,
    marginBottom: 12,
  },
  packageTitle: {
    fontSize: 16,
    fontWeight: '600',
    color: '#1A1A1A',
  },
  packagePrice: {
    fontSize: 14,
    color: '#6B6B6B',
    marginTop: 4,
  },
  packageDuration: {
    fontSize: 12,
    color: '#9E9E9E',
    marginTop: 2,
  },
  legalNoticeText: {
    fontSize: 12,
    color: '#9E9E9E',
    lineHeight: 18,
    marginTop: 20,
  },
  legalIntroText: {
    fontSize: 12,
    color: '#9E9E9E',
    marginTop: 12,
  },
  legalLinkText: {
    fontSize: 12,
    color: '#1A1A1A',
    fontWeight: '600',
    textDecorationLine: 'underline',
    marginTop: 4,
  },
  notConfiguredText: {
    fontSize: 13,
    color: '#9E9E9E',
    textAlign: 'center',
  },
  feedbackText: {
    fontSize: 13,
    color: '#C62828',
    textAlign: 'center',
    marginTop: 16,
  },
  restoreButton: {
    marginTop: 24,
    alignItems: 'center',
    paddingVertical: 12,
  },
  restoreButtonText: {
    fontSize: 14,
    color: '#1A1A1A',
    fontWeight: '500',
  },
  closeButton: {
    alignItems: 'center',
    paddingVertical: 12,
  },
  closeButtonText: {
    fontSize: 14,
    color: '#9E9E9E',
  },
});
