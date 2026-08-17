import { useQueryClient } from '@tanstack/react-query';
import { CameraView, useCameraPermissions } from 'expo-camera';
import { useLocalSearchParams, useRouter } from 'expo-router';
import { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import {
  ActivityIndicator,
  Image,
  Pressable,
  StyleSheet,
  Text,
  View,
} from 'react-native';

import { ApiError } from '../../src/api/client';
import { contributeProduct, type CapturedPhoto, type ContributionPhotos } from '../../src/api/contribute';

const STEP_KEYS = ['front', 'ingredients', 'nutrition'] as const;
type StepKey = (typeof STEP_KEYS)[number];

export default function ContributeScreen() {
  const { barcode } = useLocalSearchParams<{ barcode: string }>();
  const router = useRouter();
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  const [permission, requestPermission] = useCameraPermissions();
  const [stepIndex, setStepIndex] = useState(0);
  const [photos, setPhotos] = useState<Partial<Record<StepKey, CapturedPhoto>>>({});
  const [previewUri, setPreviewUri] = useState<string | null>(null);
  const [isCameraReady, setIsCameraReady] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [submitError, setSubmitError] = useState(false);
  const [limitReached, setLimitReached] = useState(false);
  const cameraRef = useRef<CameraView>(null);

  const stepKey = STEP_KEYS[stepIndex];

  const handleCapture = async () => {
    if (!cameraRef.current || !isCameraReady) {
      return;
    }
    const photo = await cameraRef.current.takePictureAsync({ quality: 0.7 });
    if (photo) {
      setPreviewUri(photo.uri);
    }
  };

  const handleRetake = () => {
    setPreviewUri(null);
  };

  const submit = async (allPhotos: Partial<Record<StepKey, CapturedPhoto>>) => {
    setIsSubmitting(true);
    setSubmitError(false);
    setLimitReached(false);
    try {
      const product = await contributeProduct(barcode, allPhotos as ContributionPhotos);
      queryClient.setQueryData(['product', barcode], product);
      router.replace(`/product/${barcode}`);
    } catch (err) {
      if (err instanceof ApiError && err.status === 429) {
        setLimitReached(true);
      } else {
        setSubmitError(true);
      }
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleConfirm = () => {
    if (!previewUri) {
      return;
    }

    const capturedPhoto: CapturedPhoto = {
      uri: previewUri,
      fileName: `${stepKey}.jpg`,
      mimeType: 'image/jpeg',
    };
    const updatedPhotos = { ...photos, [stepKey]: capturedPhoto };
    setPhotos(updatedPhotos);
    setPreviewUri(null);

    if (stepIndex < STEP_KEYS.length - 1) {
      setStepIndex(stepIndex + 1);
    } else {
      void submit(updatedPhotos);
    }
  };

  if (!permission) {
    return (
      <View style={[styles.container, styles.centeredLoading]}>
        <ActivityIndicator size="large" color="#FFFFFF" />
      </View>
    );
  }

  if (!permission.granted) {
    return (
      <View style={styles.permissionContainer}>
        <Text style={styles.permissionTitle}>{t('scanner.permissionTitle')}</Text>
        <Text style={styles.permissionMessage}>{t('scanner.permissionMessage')}</Text>
        <Pressable style={styles.primaryButton} onPress={requestPermission}>
          <Text style={styles.primaryButtonText}>{t('scanner.grantButton')}</Text>
        </Pressable>
      </View>
    );
  }

  if (isSubmitting) {
    return (
      <View style={styles.centered}>
        <ActivityIndicator size="large" color="#1A1A1A" />
        <Text style={styles.centeredText}>{t('contribute.submitting')}</Text>
      </View>
    );
  }

  if (limitReached) {
    return (
      <View style={styles.centered}>
        <Text style={styles.centeredTitle}>{t('limits.contributeTitle')}</Text>
        <Text style={styles.centeredText}>{t('limits.contributeMessage')}</Text>
        <Pressable style={styles.primaryButton} onPress={() => router.push('/paywall')}>
          <Text style={styles.primaryButtonText}>{t('limits.upgrade')}</Text>
        </Pressable>
        <Pressable style={styles.secondaryButton} onPress={() => router.back()}>
          <Text style={styles.secondaryButtonText}>{t('contribute.cancel')}</Text>
        </Pressable>
      </View>
    );
  }

  if (submitError) {
    return (
      <View style={styles.centered}>
        <Text style={styles.centeredTitle}>{t('contribute.errorTitle')}</Text>
        <Text style={styles.centeredText}>{t('contribute.errorMessage')}</Text>
        <Pressable style={styles.primaryButton} onPress={() => void submit(photos)}>
          <Text style={styles.primaryButtonText}>{t('contribute.retry')}</Text>
        </Pressable>
        <Pressable style={styles.secondaryButton} onPress={() => router.back()}>
          <Text style={styles.secondaryButtonText}>{t('contribute.cancel')}</Text>
        </Pressable>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      {previewUri ? (
        <View style={styles.container}>
          <Image source={{ uri: previewUri }} style={styles.preview} resizeMode="cover" />
          <View style={styles.previewActions}>
            <Pressable style={styles.secondaryButtonDark} onPress={handleRetake}>
              <Text style={styles.secondaryButtonDarkText}>{t('contribute.retake')}</Text>
            </Pressable>
            <Pressable style={styles.primaryButton} onPress={handleConfirm}>
              <Text style={styles.primaryButtonText}>
                {stepIndex < STEP_KEYS.length - 1 ? t('contribute.confirmAndContinue') : t('contribute.submit')}
              </Text>
            </Pressable>
          </View>
        </View>
      ) : (
        <>
          <CameraView
            ref={cameraRef}
            style={StyleSheet.absoluteFill}
            facing="back"
            onCameraReady={() => setIsCameraReady(true)}
          />
          <View style={styles.overlay} pointerEvents="box-none">
            <View style={styles.header}>
              <Text style={styles.stepIndicator}>
                {t('contribute.stepIndicator', { current: stepIndex + 1, total: STEP_KEYS.length })}
              </Text>
              <Text style={styles.stepTitle}>{t(`contribute.steps.${stepKey}.title`)}</Text>
              <Text style={styles.stepInstruction}>{t(`contribute.steps.${stepKey}.instruction`)}</Text>
            </View>

            <Pressable style={styles.captureButton} onPress={handleCapture} disabled={!isCameraReady}>
              <View style={styles.captureButtonInner} />
            </Pressable>
          </View>
        </>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#000000',
  },
  centeredLoading: {
    alignItems: 'center',
    justifyContent: 'center',
  },
  overlay: {
    flex: 1,
    justifyContent: 'space-between',
    paddingVertical: 60,
  },
  header: {
    paddingHorizontal: 24,
  },
  stepIndicator: {
    color: '#FFFFFF',
    fontSize: 12,
    opacity: 0.8,
    marginBottom: 8,
  },
  stepTitle: {
    color: '#FFFFFF',
    fontSize: 22,
    fontWeight: '700',
  },
  stepInstruction: {
    color: '#FFFFFF',
    fontSize: 14,
    opacity: 0.85,
    marginTop: 8,
  },
  captureButton: {
    alignSelf: 'center',
    width: 76,
    height: 76,
    borderRadius: 38,
    borderWidth: 4,
    borderColor: '#FFFFFF',
    alignItems: 'center',
    justifyContent: 'center',
  },
  captureButtonInner: {
    width: 60,
    height: 60,
    borderRadius: 30,
    backgroundColor: '#FFFFFF',
  },
  preview: {
    flex: 1,
  },
  previewActions: {
    position: 'absolute',
    bottom: 40,
    left: 24,
    right: 24,
    gap: 12,
  },
  centered: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#FFFFFF',
    padding: 24,
  },
  centeredTitle: {
    fontSize: 18,
    fontWeight: '600',
    color: '#1A1A1A',
    textAlign: 'center',
  },
  centeredText: {
    fontSize: 14,
    color: '#6B6B6B',
    textAlign: 'center',
    marginTop: 8,
  },
  primaryButton: {
    marginTop: 12,
    backgroundColor: '#1A1A1A',
    borderRadius: 12,
    paddingVertical: 14,
    paddingHorizontal: 24,
    alignItems: 'center',
  },
  primaryButtonText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: '600',
  },
  secondaryButton: {
    marginTop: 12,
    paddingVertical: 12,
    paddingHorizontal: 24,
  },
  secondaryButtonText: {
    color: '#1A1A1A',
    fontSize: 14,
    fontWeight: '500',
  },
  secondaryButtonDark: {
    backgroundColor: 'rgba(255, 255, 255, 0.15)',
    borderRadius: 12,
    paddingVertical: 14,
    paddingHorizontal: 24,
    alignItems: 'center',
  },
  secondaryButtonDarkText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: '600',
  },
  permissionContainer: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#FFFFFF',
    padding: 24,
  },
  permissionTitle: {
    fontSize: 20,
    fontWeight: '600',
    color: '#1A1A1A',
    textAlign: 'center',
  },
  permissionMessage: {
    fontSize: 14,
    color: '#6B6B6B',
    textAlign: 'center',
    marginTop: 12,
  },
});
