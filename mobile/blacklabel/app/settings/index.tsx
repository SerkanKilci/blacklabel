import { useQueryClient } from '@tanstack/react-query';
import Constants from 'expo-constants';
import { useRouter } from 'expo-router';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { ActivityIndicator, Alert, Pressable, ScrollView, StyleSheet, Text, View } from 'react-native';

import { deleteAccount } from '../../src/api/auth';
import { debugGrantPremium } from '../../src/api/subscription';
import { clearAuth, ensureUserId } from '../../src/auth/deviceAuthClient';
import { BackButton } from '../../src/components/BackButton';
import { Screen } from '../../src/components/Screen';
import { clearLocalData } from '../../src/db/database';
import { useProfile } from '../../src/hooks/useProfile';
import { useSocialLink } from '../../src/hooks/useSocialLink';
import { useSubscription } from '../../src/hooks/useSubscription';

const LANGUAGES = [
  { code: 'tr', label: 'Türkçe' },
  { code: 'en', label: 'English' },
  { code: 'de', label: 'Deutsch' },
  { code: 'fr', label: 'Français' },
  { code: 'es', label: 'Español' },
] as const;

export default function SettingsScreen() {
  const { t, i18n } = useTranslation();
  const router = useRouter();
  const queryClient = useQueryClient();
  const { data: subscription } = useSubscription();
  const { data: profile } = useProfile();
  const { appleAvailable, googleConfigured, linkingProvider, hasError: linkError, handleSignIn } = useSocialLink();
  const [userId, setUserId] = useState<string | null>(null);
  const [isDeleting, setIsDeleting] = useState(false);
  const [isGrantingPremium, setIsGrantingPremium] = useState(false);

  useEffect(() => {
    void ensureUserId().then(setUserId);
  }, []);

  const performDeleteAccount = async () => {
    setIsDeleting(true);
    try {
      await deleteAccount();
      await clearLocalData();
      await clearAuth();
      queryClient.clear();
      router.replace('/');
    } catch {
      Alert.alert(t('settings.deleteAccountErrorTitle'), t('settings.deleteAccountError'));
    } finally {
      setIsDeleting(false);
    }
  };

  const handleDeleteAccount = () => {
    Alert.alert(t('settings.deleteAccountConfirmTitle'), t('settings.deleteAccountConfirmMessage'), [
      { text: t('settings.cancel'), style: 'cancel' },
      { text: t('settings.deleteAccountConfirmButton'), style: 'destructive', onPress: () => void performDeleteAccount() },
    ]);
  };

  const handleDebugGrantPremium = async () => {
    setIsGrantingPremium(true);
    try {
      await debugGrantPremium();
      await queryClient.invalidateQueries({ queryKey: ['subscription'] });
    } catch {
      Alert.alert(t('settings.deleteAccountErrorTitle'), t('settings.debugGrantPremiumError'));
    } finally {
      setIsGrantingPremium(false);
    }
  };

  return (
    <Screen style={styles.container}>
    <ScrollView contentContainerStyle={styles.content}>
      <BackButton />
      <Text style={styles.title}>{t('settings.title')}</Text>

      <Text style={styles.sectionTitle}>{t('settings.languageSection')}</Text>
      <View style={styles.languageRow}>
        {LANGUAGES.map((language) => {
          const isActive = i18n.language === language.code;
          return (
            <Pressable
              key={language.code}
              style={[styles.languageButton, isActive && styles.languageButtonActive]}
              onPress={() => void i18n.changeLanguage(language.code)}
            >
              <Text style={[styles.languageButtonText, isActive && styles.languageButtonTextActive]}>
                {language.label}
              </Text>
            </Pressable>
          );
        })}
      </View>

      <Text style={styles.sectionTitle}>{t('settings.accountSection')}</Text>
      <View style={styles.row}>
        <Text style={styles.rowLabel}>{t('settings.deviceIdLabel')}</Text>
        <Text style={styles.rowValue}>{userId ?? '—'}</Text>
      </View>
      <View style={styles.row}>
        <Text style={styles.rowLabel}>
          {subscription?.isPremium ? t('settings.premiumActive') : t('settings.premiumInactive')}
        </Text>
        <Pressable onPress={() => router.push('/paywall')}>
          <Text style={styles.linkText}>{t('settings.managePremium')}</Text>
        </Pressable>
      </View>

      {__DEV__ && !subscription?.isPremium && (
        <Pressable style={styles.debugButton} onPress={() => void handleDebugGrantPremium()} disabled={isGrantingPremium}>
          {isGrantingPremium ? (
            <ActivityIndicator size="small" color="#1A1A1A" />
          ) : (
            <Text style={styles.debugButtonText}>{t('settings.debugGrantPremium')}</Text>
          )}
        </Pressable>
      )}

      <Text style={styles.sectionTitle}>{t('settings.linkedAccountsSection')}</Text>
      <Text style={styles.hintText}>{t('settings.linkedAccountsHint')}</Text>

      {appleAvailable && (
        profile?.hasAppleLink ? (
          <View style={styles.row}>
            <Text style={styles.rowLabel}>{t('settings.appleLinked')}</Text>
          </View>
        ) : (
          <Pressable
            style={styles.signInButton}
            onPress={() => void handleSignIn('apple')}
            disabled={linkingProvider !== null}
          >
            {linkingProvider === 'apple' ? (
              <ActivityIndicator size="small" color="#1A1A1A" />
            ) : (
              <Text style={styles.signInButtonText}>{t('settings.signInWithApple')}</Text>
            )}
          </Pressable>
        )
      )}

      {profile?.hasGoogleLink ? (
        <View style={styles.row}>
          <Text style={styles.rowLabel}>{t('settings.googleLinked')}</Text>
        </View>
      ) : googleConfigured ? (
        <Pressable
          style={styles.signInButton}
          onPress={() => void handleSignIn('google')}
          disabled={linkingProvider !== null}
        >
          {linkingProvider === 'google' ? (
            <ActivityIndicator size="small" color="#1A1A1A" />
          ) : (
            <Text style={styles.signInButtonText}>{t('settings.signInWithGoogle')}</Text>
          )}
        </Pressable>
      ) : (
        <Text style={styles.hintText}>{t('settings.googleNotConfigured')}</Text>
      )}

      {linkError && <Text style={styles.errorText}>{t('settings.linkError')}</Text>}

      <Pressable style={styles.navRow} onPress={() => router.push('/settings/data-sources')}>
        <Text style={styles.navRowText}>{t('settings.dataSourcesLink')}</Text>
      </Pressable>
      <Pressable style={styles.navRow} onPress={() => router.push('/settings/privacy')}>
        <Text style={styles.navRowText}>{t('settings.privacyLink')}</Text>
      </Pressable>
      <Pressable style={styles.navRow} onPress={() => router.push('/settings/terms')}>
        <Text style={styles.navRowText}>{t('settings.termsLink')}</Text>
      </Pressable>

      <Text style={styles.sectionTitle}>{t('settings.dangerZoneSection')}</Text>
      <Text style={styles.hintText}>{t('settings.deleteAccountHint')}</Text>
      <Pressable style={styles.deleteButton} onPress={handleDeleteAccount} disabled={isDeleting}>
        {isDeleting ? (
          <ActivityIndicator size="small" color="#C62828" />
        ) : (
          <Text style={styles.deleteButtonText}>{t('settings.deleteAccountButton')}</Text>
        )}
      </Pressable>

      <Text style={styles.versionText}>
        {t('settings.version', { version: Constants.expoConfig?.version ?? '—' })}
      </Text>
    </ScrollView>
    </Screen>
  );
}

const styles = StyleSheet.create({
  container: {
    backgroundColor: '#FFFFFF',
  },
  content: {
    paddingHorizontal: 20,
    paddingTop: 16,
    paddingBottom: 40,
  },
  title: {
    fontSize: 24,
    fontWeight: '700',
    color: '#1A1A1A',
    marginBottom: 20,
  },
  sectionTitle: {
    fontSize: 14,
    fontWeight: '700',
    color: '#1A1A1A',
    marginTop: 24,
    marginBottom: 8,
  },
  languageRow: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 8,
  },
  languageButton: {
    borderWidth: 1,
    borderColor: '#CCCCCC',
    borderRadius: 12,
    paddingVertical: 10,
    paddingHorizontal: 16,
  },
  languageButtonActive: {
    backgroundColor: '#1A1A1A',
    borderColor: '#1A1A1A',
  },
  languageButtonText: {
    fontSize: 14,
    color: '#1A1A1A',
    fontWeight: '500',
  },
  languageButtonTextActive: {
    color: '#FFFFFF',
  },
  row: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingVertical: 10,
    borderBottomWidth: 1,
    borderBottomColor: '#EEEEEE',
  },
  rowLabel: {
    fontSize: 14,
    color: '#1A1A1A',
  },
  rowValue: {
    fontSize: 12,
    color: '#9E9E9E',
  },
  linkText: {
    fontSize: 13,
    color: '#1A1A1A',
    fontWeight: '600',
  },
  hintText: {
    fontSize: 12,
    color: '#9E9E9E',
    marginBottom: 12,
  },
  debugButton: {
    borderWidth: 1,
    borderColor: '#9E9E9E',
    borderStyle: 'dashed',
    borderRadius: 12,
    paddingVertical: 10,
    alignItems: 'center',
    marginTop: 10,
  },
  debugButtonText: {
    fontSize: 13,
    fontWeight: '600',
    color: '#6B6B6B',
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
  errorText: {
    fontSize: 13,
    color: '#C62828',
    marginBottom: 10,
  },
  deleteButton: {
    borderWidth: 1,
    borderColor: '#C62828',
    borderRadius: 12,
    paddingVertical: 12,
    alignItems: 'center',
    marginTop: 4,
  },
  deleteButtonText: {
    fontSize: 14,
    fontWeight: '600',
    color: '#C62828',
  },
  navRow: {
    paddingVertical: 14,
    borderBottomWidth: 1,
    borderBottomColor: '#EEEEEE',
  },
  navRowText: {
    fontSize: 15,
    color: '#1A1A1A',
    fontWeight: '500',
  },
  versionText: {
    fontSize: 12,
    color: '#9E9E9E',
    textAlign: 'center',
    marginTop: 32,
  },
});
