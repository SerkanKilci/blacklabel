import { CameraView, useCameraPermissions } from 'expo-camera';
import * as Haptics from 'expo-haptics';
import { useFocusEffect, useRouter } from 'expo-router';
import { useCallback, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { ActivityIndicator, Pressable, ScrollView, StyleSheet, Text, View } from 'react-native';

import { ManualBarcodeEntryModal } from '../src/components/ManualBarcodeEntryModal';
import { useScanHistoryStore } from '../src/store/useScanHistoryStore';

const SCAN_RESET_DELAY_MS = 1500;

export default function ScannerScreen() {
  const { t } = useTranslation();
  const router = useRouter();
  const [permission, requestPermission] = useCameraPermissions();
  const [isScanningPaused, setIsScanningPaused] = useState(false);
  const [isManualEntryVisible, setIsManualEntryVisible] = useState(false);
  const isProcessingRef = useRef(false);

  const recentScans = useScanHistoryStore((state) => state.recentScans);
  const addScan = useScanHistoryStore((state) => state.addScan);

  useFocusEffect(
    useCallback(() => {
      isProcessingRef.current = false;
      setIsScanningPaused(false);
    }, []),
  );

  const goToProduct = useCallback(
    (barcode: string) => {
      void Haptics.notificationAsync(Haptics.NotificationFeedbackType.Success);
      addScan(barcode);
      router.push(`/product/${barcode}`);
    },
    [addScan, router],
  );

  const handleBarcodeScanned = useCallback(
    ({ data }: { data: string }) => {
      if (isProcessingRef.current) {
        return;
      }
      isProcessingRef.current = true;
      setIsScanningPaused(true);

      goToProduct(data);

      setTimeout(() => {
        isProcessingRef.current = false;
        setIsScanningPaused(false);
      }, SCAN_RESET_DELAY_MS);
    },
    [goToProduct],
  );

  const handleManualSubmit = useCallback(
    (barcode: string) => {
      setIsManualEntryVisible(false);
      goToProduct(barcode);
    },
    [goToProduct],
  );

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
        <Pressable style={styles.permissionButton} onPress={requestPermission}>
          <Text style={styles.permissionButtonText}>{t('scanner.grantButton')}</Text>
        </Pressable>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <CameraView
        style={StyleSheet.absoluteFill}
        facing="back"
        barcodeScannerSettings={{ barcodeTypes: ['ean13', 'ean8', 'upc_a'] }}
        onBarcodeScanned={isScanningPaused ? undefined : handleBarcodeScanned}
      />

      <View style={styles.overlay} pointerEvents="box-none">
        <View style={styles.topBar}>
          <Pressable style={styles.topBarButton} onPress={() => router.push('/compare')}>
            <Text style={styles.topBarButtonText}>{t('compare.title')}</Text>
          </Pressable>
          <Pressable style={styles.topBarButton} onPress={() => router.push('/history')}>
            <Text style={styles.topBarButtonText}>{t('history.title')}</Text>
          </Pressable>
          <Pressable style={styles.topBarButton} onPress={() => router.push('/preferences')}>
            <Text style={styles.topBarButtonText}>{t('preferences.title')}</Text>
          </Pressable>
          <Pressable style={styles.topBarButton} onPress={() => router.push('/settings')}>
            <Text style={styles.topBarButtonText}>{t('settings.title')}</Text>
          </Pressable>
        </View>

        {recentScans.length > 0 && (
          <View style={styles.recentScansWrapper}>
            <Text style={styles.recentScansLabel}>{t('scanner.recentScans')}</Text>
            <ScrollView horizontal showsHorizontalScrollIndicator={false} contentContainerStyle={styles.recentScansRow}>
              {recentScans.map((entry) => (
                <Pressable
                  key={`${entry.barcode}-${entry.scannedAt}`}
                  style={styles.recentScanChip}
                  onPress={() => router.push(`/product/${entry.barcode}`)}
                >
                  <Text style={styles.recentScanChipText}>{entry.barcode}</Text>
                </Pressable>
              ))}
            </ScrollView>
          </View>
        )}

        <View style={styles.targetFrame} />
        <Text style={styles.targetHint}>{t('scanner.targetHint')}</Text>

        <View style={styles.bottomBar}>
          <Pressable style={styles.manualEntryButton} onPress={() => setIsManualEntryVisible(true)}>
            <Text style={styles.manualEntryButtonText}>{t('scanner.manualEntryButton')}</Text>
          </Pressable>
        </View>
      </View>

      <ManualBarcodeEntryModal
        visible={isManualEntryVisible}
        onClose={() => setIsManualEntryVisible(false)}
        onSubmit={handleManualSubmit}
      />
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
    alignItems: 'center',
    justifyContent: 'center',
  },
  topBar: {
    position: 'absolute',
    top: 60,
    right: 20,
    flexDirection: 'row',
    gap: 8,
  },
  topBarButton: {
    backgroundColor: 'rgba(255, 255, 255, 0.15)',
    borderRadius: 16,
    paddingHorizontal: 12,
    paddingVertical: 6,
  },
  topBarButtonText: {
    color: '#FFFFFF',
    fontSize: 12,
    fontWeight: '600',
  },
  recentScansWrapper: {
    position: 'absolute',
    top: 110,
    left: 0,
    right: 0,
  },
  recentScansLabel: {
    color: '#FFFFFF',
    fontSize: 12,
    marginLeft: 20,
    marginBottom: 8,
    opacity: 0.8,
  },
  recentScansRow: {
    paddingHorizontal: 20,
    gap: 8,
  },
  recentScanChip: {
    backgroundColor: 'rgba(255, 255, 255, 0.15)',
    borderRadius: 16,
    paddingHorizontal: 12,
    paddingVertical: 6,
  },
  recentScanChipText: {
    color: '#FFFFFF',
    fontSize: 12,
  },
  targetFrame: {
    width: 260,
    height: 160,
    borderWidth: 3,
    borderColor: '#FFFFFF',
    borderRadius: 16,
    backgroundColor: 'transparent',
  },
  targetHint: {
    color: '#FFFFFF',
    fontSize: 14,
    marginTop: 20,
    textAlign: 'center',
    paddingHorizontal: 40,
  },
  bottomBar: {
    position: 'absolute',
    bottom: 50,
    left: 0,
    right: 0,
    alignItems: 'center',
  },
  manualEntryButton: {
    backgroundColor: 'rgba(255, 255, 255, 0.15)',
    borderWidth: 1,
    borderColor: 'rgba(255, 255, 255, 0.4)',
    borderRadius: 24,
    paddingHorizontal: 20,
    paddingVertical: 12,
  },
  manualEntryButtonText: {
    color: '#FFFFFF',
    fontSize: 14,
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
  permissionButton: {
    marginTop: 24,
    backgroundColor: '#1A1A1A',
    borderRadius: 12,
    paddingVertical: 14,
    paddingHorizontal: 24,
  },
  permissionButtonText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: '600',
  },
});
