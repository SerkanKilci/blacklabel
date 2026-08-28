import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { Stack } from 'expo-router';
import { useEffect } from 'react';
import { I18nextProvider } from 'react-i18next';
import { SafeAreaProvider } from 'react-native-safe-area-context';

import { ensureUserId } from '../src/auth/deviceAuthClient';
import { initDatabase } from '../src/db/database';
import i18n from '../src/i18n';
import { configurePurchases } from '../src/purchases/purchases';

const queryClient = new QueryClient();

export default function RootLayout() {
  useEffect(() => {
    initDatabase();
    void ensureUserId().then((userId) => configurePurchases(userId));
  }, []);

  return (
    <SafeAreaProvider>
      <I18nextProvider i18n={i18n}>
        <QueryClientProvider client={queryClient}>
          <Stack screenOptions={{ headerShown: false }} />
        </QueryClientProvider>
      </I18nextProvider>
    </SafeAreaProvider>
  );
}
