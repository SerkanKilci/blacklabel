import { useQueryClient } from '@tanstack/react-query';
import { useEffect, useState } from 'react';

import { isAppleSignInAvailable, isGoogleSignInConfigured, signInWithApple, signInWithGoogle } from '../auth/socialAuth';

export function useSocialLink() {
  const queryClient = useQueryClient();
  const [appleAvailable, setAppleAvailable] = useState(false);
  const [linkingProvider, setLinkingProvider] = useState<'apple' | 'google' | null>(null);
  const [hasError, setHasError] = useState(false);

  useEffect(() => {
    void isAppleSignInAvailable().then(setAppleAvailable);
  }, []);

  const handleSignIn = async (provider: 'apple' | 'google') => {
    setLinkingProvider(provider);
    setHasError(false);
    try {
      const result = provider === 'apple' ? await signInWithApple() : await signInWithGoogle();
      if (result) {
        await queryClient.invalidateQueries({ queryKey: ['profile'] });
      }
    } catch {
      setHasError(true);
    } finally {
      setLinkingProvider(null);
    }
  };

  return {
    appleAvailable,
    googleConfigured: isGoogleSignInConfigured(),
    linkingProvider,
    hasError,
    handleSignIn,
  };
}
