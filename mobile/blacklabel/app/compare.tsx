import { useQueries, type UseQueryResult } from '@tanstack/react-query';
import { CameraView, useCameraPermissions } from 'expo-camera';
import * as Haptics from 'expo-haptics';
import { useFocusEffect, useRouter } from 'expo-router';
import { useCallback, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { ActivityIndicator, Pressable, ScrollView, StyleSheet, Text, View } from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';

import { getProductByBarcode } from '../src/api/products';
import { ManualBarcodeEntryModal } from '../src/components/ManualBarcodeEntryModal';
import { useSubscription } from '../src/hooks/useSubscription';
import type { ComparisonLevel, ProductFound, ProductLookupResponse } from '../src/types/product';
import { getComparisonLevelColor, getScoreColor } from '../src/utils/score';

const SCAN_RESET_DELAY_MS = 1200;
const MAX_COMPARE_ITEMS = 5;

const CATEGORY_KEYS = ['sugar', 'saturatedFat', 'salt', 'additives'] as const;
type CategoryKey = (typeof CATEGORY_KEYS)[number];

const BAND_ORDER: Record<ComparisonLevel, number> = { Good: 0, Medium: 1, Bad: 2 };

type ProductQueryResult = UseQueryResult<ProductLookupResponse>;
type FoundItem = { barcode: string; data: ProductFound };

function formatGrams(value: number | null | undefined): string | null {
  return value === null || value === undefined ? null : `${value.toFixed(1)} g`;
}

// Compare mode doesn't (yet) let you pick which household profile's lens to view through, so it
// conservatively flags a conflict if it applies to ANY profile on the account — erring toward
// showing a warning rather than hiding one that matters for someone in the household.
function hasAnyProfileAllergenConflict(data: ProductFound): boolean {
  return data.profileWarnings.some((pw) => pw.warnings.some((w) => w.type === 'allergen'));
}

interface SummaryRow {
  key: CategoryKey | 'allergen';
  // null = every item ties (all equally good) -> render as "similar"
  winnerNames: string[] | null;
  // Allergen row only: every item conflicts with someone, so "similar" would understate it.
  allIncompatible?: boolean;
}

// Derived purely from the same `comparisonBands`/`profileWarnings` the cards already render —
// never invents a ranking that the band system itself wouldn't support.
function buildSummaryRows(items: FoundItem[]): SummaryRow[] {
  if (items.length < 2) {
    return [];
  }

  const rows: SummaryRow[] = [];

  for (const key of CATEGORY_KEYS) {
    const considered = items
      .map((item) => ({ name: item.data.name, band: item.data.comparisonBands[key] }))
      .filter((entry): entry is { name: string; band: ComparisonLevel } => entry.band !== null);
    if (considered.length < 2) {
      continue;
    }
    const bestOrder = Math.min(...considered.map((entry) => BAND_ORDER[entry.band]));
    const winners = considered.filter((entry) => BAND_ORDER[entry.band] === bestOrder);
    rows.push({ key, winnerNames: winners.length === considered.length ? null : winners.map((w) => w.name) });
  }

  const allergenConsidered = items.map((item) => ({
    name: item.data.name,
    hasConflict: hasAnyProfileAllergenConflict(item.data),
  }));
  const compatible = allergenConsidered.filter((entry) => !entry.hasConflict);
  if (compatible.length === 0) {
    rows.push({ key: 'allergen', winnerNames: null, allIncompatible: true });
  } else {
    const allergenTie = compatible.length === allergenConsidered.length;
    rows.push({ key: 'allergen', winnerNames: allergenTie ? null : compatible.map((w) => w.name) });
  }

  return rows;
}

export default function CompareScreen() {
  const { t } = useTranslation();
  const router = useRouter();
  const insets = useSafeAreaInsets();
  const { data: subscription, isLoading: isSubscriptionLoading } = useSubscription();
  const isPremium = subscription?.isPremium ?? false;

  const [permission, requestPermission] = useCameraPermissions();
  const [isScanningPaused, setIsScanningPaused] = useState(false);
  const [isManualEntryVisible, setIsManualEntryVisible] = useState(false);
  const [barcodes, setBarcodes] = useState<string[]>([]);
  const [cameraKey, setCameraKey] = useState(0);
  const isProcessingRef = useRef(false);

  // See app/index.tsx's identical fix: this screen stays mounted (just hidden) behind
  // /product/[barcode], and a backgrounded camera stream can get suspended by the browser and
  // never resume on its own. Bumping this key forces a fresh CameraView mount on every refocus.
  useFocusEffect(
    useCallback(() => {
      isProcessingRef.current = false;
      setIsScanningPaused(false);
      setCameraKey((key) => key + 1);
    }, []),
  );

  const results = useQueries({
    queries: barcodes.map((barcode) => ({
      queryKey: ['product', barcode],
      queryFn: () => getProductByBarcode(barcode),
    })),
  });

  const addBarcode = useCallback((barcode: string) => {
    setBarcodes((prev) => (prev.includes(barcode) || prev.length >= MAX_COMPARE_ITEMS ? prev : [...prev, barcode]));
  }, []);

  const removeBarcode = useCallback((barcode: string) => {
    setBarcodes((prev) => prev.filter((b) => b !== barcode));
  }, []);

  const clearAll = useCallback(() => setBarcodes([]), []);

  const handleBarcodeScanned = useCallback(
    ({ data }: { data: string }) => {
      if (isProcessingRef.current) {
        return;
      }
      isProcessingRef.current = true;
      setIsScanningPaused(true);

      void Haptics.notificationAsync(Haptics.NotificationFeedbackType.Success);
      addBarcode(data);

      setTimeout(() => {
        isProcessingRef.current = false;
        setIsScanningPaused(false);
      }, SCAN_RESET_DELAY_MS);
    },
    [addBarcode],
  );

  const handleManualSubmit = useCallback(
    (barcode: string) => {
      setIsManualEntryVisible(false);
      addBarcode(barcode);
    },
    [addBarcode],
  );

  if (isSubscriptionLoading) {
    return (
      <View style={[styles.container, styles.centeredLoading]}>
        <ActivityIndicator size="large" color="#1A1A1A" />
      </View>
    );
  }

  if (!isPremium) {
    return (
      <View style={styles.premiumContainer}>
        <Text style={styles.premiumTitle}>{t('compare.premiumTitle')}</Text>
        <Text style={styles.premiumMessage}>{t('compare.premiumMessage')}</Text>
        <Pressable style={styles.primaryButton} onPress={() => router.push('/paywall')}>
          <Text style={styles.primaryButtonText}>{t('compare.upgrade')}</Text>
        </Pressable>
        <Pressable style={styles.secondaryButton} onPress={() => router.back()}>
          <Text style={styles.secondaryButtonText}>{t('result.backToScanner')}</Text>
        </Pressable>
      </View>
    );
  }

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

  const items = barcodes.map((barcode, index) => ({ barcode, result: results[index] }));
  const sortedItems = [...items].sort((a, b) => {
    const scoreA = a.result.data?.found ? a.result.data.score : null;
    const scoreB = b.result.data?.found ? b.result.data.score : null;
    if (scoreA === null && scoreB === null) return 0;
    if (scoreA === null) return 1;
    if (scoreB === null) return -1;
    return scoreB - scoreA;
  });

  // "Best for you": highest score among products with no allergen conflict for anyone on the
  // account. Unlike a single-profile app, we never fall back to "just pick the highest score
  // anyway" when every scanned item conflicts with someone — recommending a fish product as
  // "best" when it's exactly what one household member is allergic to would be actively
  // misleading, not just imprecise. No safe candidate means no recommendation, not a bad one.
  const scorable = items
    .map((item) => (item.result.data?.found && item.result.data.score !== null ? { barcode: item.barcode, data: item.result.data } : null))
    .filter((item): item is { barcode: string; data: ProductFound } => item !== null);
  const withoutAllergenConflict = scorable.filter((item) => !hasAnyProfileAllergenConflict(item.data));
  const best = withoutAllergenConflict.reduce<(typeof withoutAllergenConflict)[number] | null>((acc, item) => {
    if (!acc || (item.data.score ?? -1) > (acc.data.score ?? -1)) {
      return item;
    }
    return acc;
  }, null);
  const noSafeChoice = scorable.length >= 2 && withoutAllergenConflict.length === 0;

  const foundItems: FoundItem[] = items
    .map((item) => (item.result.data?.found ? { barcode: item.barcode, data: item.result.data } : null))
    .filter((item): item is FoundItem => item !== null);
  const summaryRows = buildSummaryRows(foundItems);

  return (
    <View style={styles.container}>
      <CameraView
        key={cameraKey}
        style={StyleSheet.absoluteFill}
        facing="back"
        barcodeScannerSettings={{ barcodeTypes: ['ean13', 'ean8', 'upc_a'] }}
        onBarcodeScanned={isScanningPaused ? undefined : handleBarcodeScanned}
      />

      <View style={styles.overlay} pointerEvents="box-none">
        <View style={[styles.topBar, { top: insets.top + 12 }]}>
          <Pressable style={styles.topBarButton} onPress={() => router.back()}>
            <Text style={styles.topBarButtonText}>{t('result.backToScanner')}</Text>
          </Pressable>
          <Pressable style={styles.topBarButton} onPress={() => setIsManualEntryVisible(true)}>
            <Text style={styles.topBarButtonText}>{t('scanner.manualEntryButton')}</Text>
          </Pressable>
          {barcodes.length > 0 && (
            <Pressable style={styles.topBarButton} onPress={clearAll}>
              <Text style={styles.topBarButtonText}>{t('compare.clearAll')}</Text>
            </Pressable>
          )}
          {/* Same direct-user-gesture requirement as app/index.tsx's identical button. */}
          <Pressable style={styles.topBarButton} onPress={() => setCameraKey((key) => key + 1)}>
            <Text style={styles.topBarButtonText}>{t('scanner.restartCameraButton')}</Text>
          </Pressable>
        </View>

        <View style={styles.targetFrame} />
        <Text style={styles.hintText}>{barcodes.length === 0 ? t('compare.hint') : t('scanner.targetHint')}</Text>
      </View>

      {barcodes.length > 0 && (
        <View style={styles.listPanel}>
          {best && <Text style={styles.bestForYouBanner}>{t('compare.bestForYou', { name: best.data.name })}</Text>}
          {noSafeChoice && <Text style={styles.noSafeChoiceBanner}>{t('compare.noSafeChoice')}</Text>}
          {barcodes.length >= MAX_COMPARE_ITEMS && <Text style={styles.maxReachedText}>{t('compare.maxReached')}</Text>}
          {summaryRows.length > 0 && (
            <View style={styles.summaryBox}>
              <Text style={styles.summaryTitle}>{t('compare.summaryTitle')}</Text>
              {summaryRows.map((row) => (
                <Text key={row.key} style={styles.summaryRow}>
                  {t(`compare.categoryLabels.${row.key}`)}:{' '}
                  {row.allIncompatible
                    ? t('compare.summaryAllIncompatible')
                    : row.winnerNames
                      ? t('compare.summaryWinner', { names: row.winnerNames.join(', ') })
                      : t('compare.summarySimilar')}
                </Text>
              ))}
            </View>
          )}
          <ScrollView contentContainerStyle={[styles.listContent, { paddingBottom: 24 + insets.bottom }]}>
            {sortedItems.map(({ barcode, result }) => (
              <ComparisonCard
                key={barcode}
                barcode={barcode}
                result={result}
                isBest={barcode === best?.barcode}
                onRemove={() => removeBarcode(barcode)}
                onPress={() => router.push(`/product/${barcode}`)}
              />
            ))}
            {foundItems.length > 0 && <Text style={styles.disclaimerText}>{t('compare.formulationDisclaimer')}</Text>}
          </ScrollView>
        </View>
      )}

      <ManualBarcodeEntryModal
        visible={isManualEntryVisible}
        onClose={() => setIsManualEntryVisible(false)}
        onSubmit={handleManualSubmit}
      />
    </View>
  );
}

interface ComparisonCardProps {
  barcode: string;
  result: ProductQueryResult;
  isBest: boolean;
  onRemove: () => void;
  onPress: () => void;
}

function ComparisonCard({ barcode, result, isBest, onRemove, onPress }: ComparisonCardProps) {
  const { t } = useTranslation();
  const { data, isLoading, isError } = result;

  if (isLoading) {
    return (
      <View style={styles.card}>
        <Text style={styles.cardStatusText}>{t('compare.loadingItem')}</Text>
      </View>
    );
  }

  if (isError || !data || !data.found) {
    return (
      <View style={styles.card}>
        <View style={styles.cardHeader}>
          <Text style={styles.cardName} numberOfLines={1}>
            {barcode}
          </Text>
          <Text style={styles.cardStatusText}>{t('compare.notFoundItem')}</Text>
          <Pressable style={styles.removeButton} onPress={onRemove} hitSlop={8}>
            <Text style={styles.removeButtonText}>✕</Text>
          </Pressable>
        </View>
      </View>
    );
  }

  const hasAllergenConflict = hasAnyProfileAllergenConflict(data);
  const categories: Array<{ key: CategoryKey; level: ComparisonLevel | null; detail: string | null }> = [
    { key: 'sugar', level: data.comparisonBands.sugar, detail: formatGrams(data.nutriments?.sugars100g) },
    { key: 'saturatedFat', level: data.comparisonBands.saturatedFat, detail: formatGrams(data.nutriments?.saturatedFat100g) },
    { key: 'salt', level: data.comparisonBands.salt, detail: formatGrams(data.nutriments?.salt100g) },
    { key: 'additives', level: data.comparisonBands.additives, detail: data.additives.length > 0 ? String(data.additives.length) : null },
  ];

  return (
    <Pressable style={[styles.card, isBest && styles.cardBest]} onPress={onPress}>
      <View style={styles.cardHeader}>
        <View style={styles.cardHeaderText}>
          <Text style={styles.cardName} numberOfLines={1}>
            {data.name}
          </Text>
          {isBest && <Text style={styles.bestBadge}>{t('compare.bestChoice')}</Text>}
        </View>
        <Text style={[styles.cardScore, { color: getScoreColor(data.score) }]}>{data.score ?? '—'}</Text>
        <Pressable style={styles.removeButton} onPress={onRemove} hitSlop={8}>
          <Text style={styles.removeButtonText}>✕</Text>
        </Pressable>
      </View>

      {categories.map(({ key, level, detail }) =>
        level === null ? null : (
          <View key={key} style={styles.categoryRow}>
            <View style={[styles.categoryDot, { backgroundColor: getComparisonLevelColor(level) }]} />
            <Text style={styles.categoryLabel}>{t(`compare.categoryLabels.${key}`)}</Text>
            <Text style={styles.categoryValue}>
              {t(`compare.levels.${key}.${level}`)}
              {detail ? ` (${detail})` : ''}
            </Text>
          </View>
        ),
      )}

      <View style={styles.categoryRow}>
        <View style={[styles.categoryDot, { backgroundColor: getComparisonLevelColor(hasAllergenConflict ? 'Bad' : 'Good') }]} />
        <Text style={styles.categoryLabel}>{t('compare.categoryLabels.allergen')}</Text>
        <Text style={styles.categoryValue}>
          {hasAllergenConflict ? t('compare.allergenConflict') : t('compare.allergenCompatible')}
        </Text>
      </View>
    </Pressable>
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
  },
  topBar: {
    position: 'absolute',
    left: 20,
    right: 20,
    flexDirection: 'row',
    justifyContent: 'flex-end',
    flexWrap: 'wrap',
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
  targetFrame: {
    position: 'absolute',
    top: '14%',
    alignSelf: 'center',
    width: 260,
    height: 140,
    borderWidth: 3,
    borderColor: '#FFFFFF',
    borderRadius: 16,
    backgroundColor: 'transparent',
  },
  hintText: {
    position: 'absolute',
    top: '31%',
    left: 40,
    right: 40,
    color: '#FFFFFF',
    fontSize: 15,
    textAlign: 'center',
  },
  listPanel: {
    position: 'absolute',
    left: 0,
    right: 0,
    bottom: 0,
    maxHeight: '62%',
    backgroundColor: '#FFFFFF',
    borderTopLeftRadius: 20,
    borderTopRightRadius: 20,
    paddingTop: 16,
  },
  bestForYouBanner: {
    fontSize: 13,
    fontWeight: '700',
    color: '#2E7D32',
    textAlign: 'center',
    paddingHorizontal: 20,
    marginBottom: 10,
  },
  noSafeChoiceBanner: {
    fontSize: 13,
    fontWeight: '700',
    color: '#C62828',
    textAlign: 'center',
    paddingHorizontal: 20,
    marginBottom: 10,
  },
  maxReachedText: {
    fontSize: 12,
    color: '#9E9E9E',
    textAlign: 'center',
    paddingHorizontal: 20,
    marginBottom: 8,
  },
  summaryBox: {
    marginHorizontal: 16,
    marginBottom: 12,
    padding: 12,
    borderRadius: 12,
    backgroundColor: '#F5F5F5',
  },
  summaryTitle: {
    fontSize: 12,
    fontWeight: '700',
    color: '#1A1A1A',
    marginBottom: 6,
  },
  summaryRow: {
    fontSize: 12,
    color: '#4A4A4A',
    marginTop: 2,
  },
  disclaimerText: {
    fontSize: 11,
    color: '#9E9E9E',
    fontStyle: 'italic',
    marginTop: 12,
    lineHeight: 15,
  },
  listContent: {
    paddingHorizontal: 16,
    paddingBottom: 24,
  },
  card: {
    paddingVertical: 12,
    paddingHorizontal: 4,
    borderBottomWidth: 1,
    borderBottomColor: '#EEEEEE',
  },
  cardBest: {
    backgroundColor: '#F1F8F1',
    borderRadius: 12,
    borderBottomWidth: 0,
    paddingHorizontal: 10,
  },
  cardHeader: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 6,
  },
  cardHeaderText: {
    flex: 1,
  },
  cardName: {
    fontSize: 14,
    fontWeight: '600',
    color: '#1A1A1A',
    flex: 1,
  },
  cardStatusText: {
    fontSize: 13,
    color: '#9E9E9E',
    marginLeft: 8,
  },
  bestBadge: {
    fontSize: 11,
    color: '#2E7D32',
    fontWeight: '700',
    marginTop: 2,
  },
  cardScore: {
    fontSize: 18,
    fontWeight: '700',
    marginRight: 10,
  },
  categoryRow: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingVertical: 3,
  },
  categoryDot: {
    width: 8,
    height: 8,
    borderRadius: 4,
    marginRight: 10,
  },
  categoryLabel: {
    fontSize: 12,
    color: '#4A4A4A',
    flex: 1,
  },
  categoryValue: {
    fontSize: 12,
    fontWeight: '600',
    color: '#1A1A1A',
    flexShrink: 1,
    textAlign: 'right',
  },
  removeButton: {
    paddingHorizontal: 6,
    paddingVertical: 4,
  },
  removeButtonText: {
    fontSize: 14,
    color: '#9E9E9E',
  },
  premiumContainer: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#FFFFFF',
    padding: 24,
  },
  premiumTitle: {
    fontSize: 20,
    fontWeight: '700',
    color: '#1A1A1A',
    textAlign: 'center',
  },
  premiumMessage: {
    fontSize: 14,
    color: '#6B6B6B',
    textAlign: 'center',
    marginTop: 12,
    lineHeight: 20,
  },
  primaryButton: {
    marginTop: 24,
    backgroundColor: '#1A1A1A',
    borderRadius: 12,
    paddingVertical: 14,
    paddingHorizontal: 24,
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
