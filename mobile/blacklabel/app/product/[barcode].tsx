import { useLocalSearchParams, useRouter } from 'expo-router';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import {
  ActivityIndicator,
  Image,
  Pressable,
  ScrollView,
  StyleSheet,
  Text,
  View,
} from 'react-native';

import { ApiError } from '../../src/api/client';
import { BackButton } from '../../src/components/BackButton';
import { Screen } from '../../src/components/Screen';
import { ScoreRing } from '../../src/components/ScoreRing';
import { cacheProduct } from '../../src/db/productCache';
import { insertScan } from '../../src/db/scanHistory';
import { useAlternatives } from '../../src/hooks/useAlternatives';
import { useProduct } from '../../src/hooks/useProduct';
import { useSubscription } from '../../src/hooks/useSubscription';
import { syncPendingScans } from '../../src/sync/syncScans';
import type { AdditiveInfo, PersonalWarning, ProductFound } from '../../src/types/product';
import { getLocalizedAdditiveName, getLocalizedAdditiveDescription } from '../../src/utils/additiveLocalization';
import { getRiskLevelColor, getScoreColor } from '../../src/utils/score';

type TabKey = 'additives' | 'nutrition' | 'ingredients';
type TFunction = (key: string, options?: Record<string, unknown>) => string;

function personalWarningText(warning: PersonalWarning, product: ProductFound, t: TFunction, language: string): string {
  if (warning.type === 'allergen') {
    return t('warning.allergenMessage', { allergen: t(`allergens.${warning.code}`) });
  }

  if (warning.type === 'additive') {
    const additive = product.additives.find((a) => a.code === warning.code);
    const name = additive ? getLocalizedAdditiveName(additive, language) : warning.code;
    return t('warning.additiveMessage', { additive: `${warning.code} (${name})` });
  }

  return t('warning.dietMessage', { flag: t(`dietFlags.${warning.code}`) });
}

export default function ProductResultScreen() {
  const { barcode } = useLocalSearchParams<{ barcode: string }>();
  const router = useRouter();
  const { t } = useTranslation();
  const { data, isLoading, isError, error, refetch } = useProduct(barcode);

  if (isLoading) {
    return (
      <Screen style={styles.centered}>
        <BackButton style={styles.loadingBackButton} />
        <ActivityIndicator size="large" color="#1A1A1A" />
        <Text style={styles.centeredText}>{t('result.loading')}</Text>
      </Screen>
    );
  }

  if (isError) {
    const isLimitReached = error instanceof ApiError && error.status === 429;

    return (
      <View style={styles.centered}>
        <Text style={styles.centeredTitle}>
          {isLimitReached ? t('limits.scanTitle') : t('result.errorTitle')}
        </Text>
        <Text style={styles.centeredText}>
          {isLimitReached ? t('limits.scanMessage') : t('result.errorMessage')}
        </Text>
        {isLimitReached ? (
          <Pressable style={styles.primaryButton} onPress={() => router.push('/paywall')}>
            <Text style={styles.primaryButtonText}>{t('limits.upgrade')}</Text>
          </Pressable>
        ) : (
          <Pressable style={styles.primaryButton} onPress={() => refetch()}>
            <Text style={styles.primaryButtonText}>{t('result.retry')}</Text>
          </Pressable>
        )}
        <Pressable style={styles.secondaryButton} onPress={() => router.back()}>
          <Text style={styles.secondaryButtonText}>{t('result.backToScanner')}</Text>
        </Pressable>
      </View>
    );
  }

  if (!data || !data.found) {
    return (
      <View style={styles.centered}>
        <Text style={styles.centeredTitle}>{t('result.notFoundTitle')}</Text>
        <Text style={styles.centeredText}>{t('result.notFoundMessage')}</Text>
        <Pressable style={styles.primaryButton} onPress={() => router.back()}>
          <Text style={styles.primaryButtonText}>{t('result.backToScanner')}</Text>
        </Pressable>
      </View>
    );
  }

  return <ProductFoundView product={data} />;
}

function ProductFoundView({ product }: { product: ProductFound }) {
  const { t, i18n } = useTranslation();
  const router = useRouter();
  const [activeTab, setActiveTab] = useState<TabKey>('additives');
  const profilesWithWarnings = product.profileWarnings.filter((pw) => pw.warnings.length > 0);

  useEffect(() => {
    void cacheProduct(product);
    // The backend already recorded this scan server-side (GET /products/{barcode} is the
    // authoritative recording point — see src/db/scanHistory.ts), so mark it synced here to
    // avoid syncPendingScans() posting a duplicate later.
    void insertScan(product.barcode, product.score, true);
    void syncPendingScans();
    // Re-run for every fresh scan of this barcode, not just the first mount.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [product.barcode, product.score]);

  return (
    <Screen style={styles.container}>
    <ScrollView contentContainerStyle={styles.contentContainer}>
      <View style={styles.header}>
        <BackButton style={styles.backButton} />
        <ScoreRing score={product.score} />
        <Text style={styles.productName}>{product.name}</Text>
        {product.brand && <Text style={styles.productBrand}>{product.brand}</Text>}
        {product.imageUrl && <Image source={{ uri: product.imageUrl }} style={styles.productImage} resizeMode="contain" />}
      </View>

      {product.dataQuality !== 'Complete' && (
        <View style={styles.incompleteDataCard}>
          <Text style={styles.incompleteDataTitle}>{t('warning.incompleteDataTitle')}</Text>
          <Text style={styles.incompleteDataMessage}>{t('warning.incompleteDataMessage')}</Text>
        </View>
      )}

      {profilesWithWarnings.length > 0 && (
        <View style={styles.warningCard}>
          {profilesWithWarnings.map((profileWarning) => (
            <View key={profileWarning.profileId} style={styles.warningProfileBlock}>
              <Text style={styles.warningProfileName}>{profileWarning.profileName}</Text>
              {profileWarning.warnings.map((warning) => (
                <Text key={`${warning.type}-${warning.code}`} style={styles.warningText}>
                  {personalWarningText(warning, product, t, i18n.language)}
                </Text>
              ))}
            </View>
          ))}
        </View>
      )}

      {product.hasLockedPersonalWarnings && (
        <View style={styles.lockedWarningCard}>
          <Text style={styles.lockedWarningTitle}>{t('warning.lockedTitle')}</Text>
          <Text style={styles.lockedWarningMessage}>{t('warning.lockedMessage')}</Text>
          <Pressable style={styles.lockedWarningButton} onPress={() => router.push('/paywall')}>
            <Text style={styles.lockedWarningButtonText}>{t('preferences.profiles.upgrade')}</Text>
          </Pressable>
        </View>
      )}

      <View style={styles.tabBar}>
        <TabButton label={t('result.tabs.additives')} active={activeTab === 'additives'} onPress={() => setActiveTab('additives')} />
        <TabButton label={t('result.tabs.nutrition')} active={activeTab === 'nutrition'} onPress={() => setActiveTab('nutrition')} />
        <TabButton label={t('result.tabs.ingredients')} active={activeTab === 'ingredients'} onPress={() => setActiveTab('ingredients')} />
      </View>

      {activeTab === 'additives' && <AdditivesTab additives={product.additives} allergens={product.allergens} />}
      {activeTab === 'nutrition' && <NutritionTab product={product} />}
      {activeTab === 'ingredients' && <IngredientsTab ingredientsText={product.ingredientsText} />}

      <AlternativesSection barcode={product.barcode} />

      <Text style={styles.disclaimer}>{t('common.medicalDisclaimer')}</Text>
      {product.source === 'OpenFoodFacts' && <Text style={styles.attribution}>{t('result.attribution')}</Text>}
    </ScrollView>
    </Screen>
  );
}

function TabButton({ label, active, onPress }: { label: string; active: boolean; onPress: () => void }) {
  return (
    <Pressable style={[styles.tabButton, active && styles.tabButtonActive]} onPress={onPress}>
      <Text style={[styles.tabButtonText, active && styles.tabButtonTextActive]}>{label}</Text>
    </Pressable>
  );
}

function AdditivesTab({ additives, allergens }: { additives: AdditiveInfo[]; allergens: string[] }) {
  const { t, i18n } = useTranslation();
  const [expandedCode, setExpandedCode] = useState<string | null>(null);

  return (
    <View style={styles.tabContent}>
      {allergens.length > 0 && (
        <View style={styles.allergensSection}>
          <Text style={styles.sectionTitle}>{t('result.allergensTitle')}</Text>
          <View style={styles.allergensRow}>
            {allergens.map((allergen) => (
              <View key={allergen} style={styles.allergenChip}>
                <Text style={styles.allergenChipText}>{t(`allergens.${allergen}`)}</Text>
              </View>
            ))}
          </View>
          <Text style={styles.disclaimerSmall}>{t('common.allergenDisclaimer')}</Text>
        </View>
      )}

      {additives.length === 0 ? (
        <Text style={styles.emptyText}>{t('result.noAdditives')}</Text>
      ) : (
        additives.map((additive) => {
          const isExpanded = expandedCode === additive.code;
          const name = getLocalizedAdditiveName(additive, i18n.language);
          const description = getLocalizedAdditiveDescription(additive, i18n.language);

          return (
            <Pressable
              key={additive.code}
              style={styles.additiveRow}
              onPress={() => setExpandedCode(isExpanded ? null : additive.code)}
            >
              <View style={styles.additiveRowHeader}>
                <View style={[styles.riskDot, { backgroundColor: getRiskLevelColor(additive.riskLevel) }]} />
                <View style={styles.additiveRowText}>
                  <Text style={styles.additiveCode}>{additive.code}</Text>
                  <Text style={styles.additiveName}>{name}</Text>
                </View>
                <Text style={styles.riskLabel}>{t(`result.riskLevel.${additive.riskLevel}`)}</Text>
              </View>
              {isExpanded && (
                <View style={styles.additiveDescriptionWrapper}>
                  <Text style={styles.additiveDescription}>{description}</Text>
                  {additive.sourceNote && <Text style={styles.additiveSourceNote}>{additive.sourceNote}</Text>}
                </View>
              )}
            </Pressable>
          );
        })
      )}
    </View>
  );
}

function NutritionTab({ product }: { product: ProductFound }) {
  const { t } = useTranslation();
  const nutriments = product.nutriments;

  if (!nutriments) {
    return (
      <View style={styles.tabContent}>
        <Text style={styles.emptyText}>{t('result.noNutriments')}</Text>
      </View>
    );
  }

  const rows: Array<{ key: keyof typeof nutriments; unit: string }> = [
    { key: 'energyKcal100g', unit: 'kcal' },
    { key: 'fat100g', unit: 'g' },
    { key: 'saturatedFat100g', unit: 'g' },
    { key: 'carbohydrates100g', unit: 'g' },
    { key: 'sugars100g', unit: 'g' },
    { key: 'fiber100g', unit: 'g' },
    { key: 'proteins100g', unit: 'g' },
    { key: 'salt100g', unit: 'g' },
  ];

  return (
    <View style={styles.tabContent}>
      <Text style={styles.sectionTitle}>
        {t('result.tabs.nutrition')} ({t('result.nutriments.per100g')})
      </Text>
      {rows.map(({ key, unit }) => {
        const value = nutriments[key];
        return (
          <View key={key} style={styles.nutrimentRow}>
            <Text style={styles.nutrimentLabel}>{t(`result.nutriments.${key}`)}</Text>
            <Text style={styles.nutrimentValue}>{value === null ? '—' : `${value} ${unit}`}</Text>
          </View>
        );
      })}
    </View>
  );
}

function AlternativesSection({ barcode }: { barcode: string }) {
  const { t } = useTranslation();
  const router = useRouter();
  const { data: subscription } = useSubscription();
  const isPremium = subscription?.isPremium ?? false;
  const { data: alternatives, isLoading } = useAlternatives(barcode, isPremium);

  if (!isPremium) {
    return (
      <View style={styles.alternativesSection}>
        <Text style={styles.sectionTitle}>{t('alternatives.premiumTitle')}</Text>
        <Text style={styles.emptyText}>{t('alternatives.premiumMessage')}</Text>
        <Pressable style={styles.upgradeButton} onPress={() => router.push('/paywall')}>
          <Text style={styles.upgradeButtonText}>{t('alternatives.upgrade')}</Text>
        </Pressable>
      </View>
    );
  }

  if (isLoading) {
    return null;
  }

  if (!alternatives || alternatives.length === 0) {
    return (
      <View style={styles.alternativesSection}>
        <Text style={styles.sectionTitle}>{t('alternatives.title')}</Text>
        <Text style={styles.emptyText}>{t('alternatives.empty')}</Text>
      </View>
    );
  }

  return (
    <View style={styles.alternativesSection}>
      <Text style={styles.sectionTitle}>{t('alternatives.title')}</Text>
      {alternatives.map((alt) => (
        <Pressable key={alt.barcode} style={styles.alternativeRow} onPress={() => router.push(`/product/${alt.barcode}`)}>
          <View style={[styles.scoreDot, { backgroundColor: getScoreColor(alt.score) }]} />
          <View style={styles.alternativeRowText}>
            <Text style={styles.additiveCode}>{alt.name}</Text>
            {alt.brand && <Text style={styles.additiveName}>{alt.brand}</Text>}
          </View>
          <Text style={styles.riskLabel}>{alt.score ?? '—'}</Text>
        </Pressable>
      ))}
    </View>
  );
}

function IngredientsTab({ ingredientsText }: { ingredientsText: string | null }) {
  const { t } = useTranslation();

  return (
    <View style={styles.tabContent}>
      {ingredientsText ? (
        <Text style={styles.ingredientsText}>{ingredientsText}</Text>
      ) : (
        <Text style={styles.emptyText}>{t('result.noIngredients')}</Text>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    backgroundColor: '#FFFFFF',
  },
  contentContainer: {
    paddingBottom: 40,
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
  header: {
    alignItems: 'center',
    paddingTop: 12,
    paddingHorizontal: 24,
  },
  backButton: {
    alignSelf: 'flex-start',
  },
  loadingBackButton: {
    position: 'absolute',
    top: 12,
    left: 24,
  },
  productName: {
    fontSize: 20,
    fontWeight: '700',
    color: '#1A1A1A',
    marginTop: 16,
    textAlign: 'center',
  },
  productBrand: {
    fontSize: 14,
    color: '#6B6B6B',
    marginTop: 4,
  },
  productImage: {
    width: 120,
    height: 120,
    marginTop: 16,
  },
  warningCard: {
    marginTop: 20,
    marginHorizontal: 20,
    backgroundColor: '#FDECEA',
    borderRadius: 12,
    padding: 16,
  },
  warningProfileBlock: {
    marginBottom: 8,
  },
  warningProfileName: {
    color: '#C62828',
    fontSize: 12,
    fontWeight: '700',
    marginBottom: 2,
  },
  warningText: {
    color: '#C62828',
    fontSize: 14,
    fontWeight: '500',
  },
  incompleteDataCard: {
    marginTop: 20,
    marginHorizontal: 20,
    backgroundColor: '#FFF4E5',
    borderRadius: 12,
    padding: 16,
  },
  incompleteDataTitle: {
    color: '#8A5A00',
    fontSize: 13,
    fontWeight: '700',
  },
  incompleteDataMessage: {
    color: '#8A5A00',
    fontSize: 13,
    marginTop: 4,
    lineHeight: 19,
  },
  lockedWarningCard: {
    marginTop: 20,
    marginHorizontal: 20,
    backgroundColor: '#FFF4E5',
    borderRadius: 12,
    padding: 16,
  },
  lockedWarningTitle: {
    color: '#8A5A00',
    fontSize: 13,
    fontWeight: '700',
  },
  lockedWarningMessage: {
    color: '#8A5A00',
    fontSize: 13,
    marginTop: 4,
    lineHeight: 19,
  },
  lockedWarningButton: {
    marginTop: 12,
    alignSelf: 'flex-start',
    backgroundColor: '#8A5A00',
    borderRadius: 10,
    paddingVertical: 9,
    paddingHorizontal: 16,
  },
  lockedWarningButtonText: {
    color: '#FFFFFF',
    fontSize: 13,
    fontWeight: '600',
  },
  tabBar: {
    flexDirection: 'row',
    marginTop: 28,
    marginHorizontal: 20,
    backgroundColor: '#F5F5F5',
    borderRadius: 12,
    padding: 4,
  },
  tabButton: {
    flex: 1,
    paddingVertical: 10,
    borderRadius: 10,
    alignItems: 'center',
  },
  tabButtonActive: {
    backgroundColor: '#FFFFFF',
  },
  tabButtonText: {
    fontSize: 13,
    color: '#6B6B6B',
    fontWeight: '500',
  },
  tabButtonTextActive: {
    color: '#1A1A1A',
    fontWeight: '700',
  },
  tabContent: {
    marginTop: 20,
    paddingHorizontal: 20,
  },
  sectionTitle: {
    fontSize: 14,
    fontWeight: '700',
    color: '#1A1A1A',
    marginBottom: 12,
  },
  emptyText: {
    fontSize: 14,
    color: '#6B6B6B',
    textAlign: 'center',
    marginTop: 12,
  },
  allergensSection: {
    marginBottom: 20,
    paddingBottom: 16,
    borderBottomWidth: 1,
    borderBottomColor: '#EEEEEE',
  },
  allergensRow: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 8,
  },
  allergenChip: {
    backgroundColor: '#F5F5F5',
    borderRadius: 12,
    paddingHorizontal: 12,
    paddingVertical: 6,
  },
  allergenChipText: {
    fontSize: 12,
    color: '#1A1A1A',
    fontWeight: '500',
  },
  disclaimerSmall: {
    fontSize: 11,
    color: '#9E9E9E',
    marginTop: 10,
  },
  additiveRow: {
    borderBottomWidth: 1,
    borderBottomColor: '#EEEEEE',
    paddingVertical: 14,
  },
  additiveRowHeader: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  riskDot: {
    width: 10,
    height: 10,
    borderRadius: 5,
    marginRight: 12,
  },
  additiveRowText: {
    flex: 1,
  },
  additiveCode: {
    fontSize: 14,
    fontWeight: '700',
    color: '#1A1A1A',
  },
  additiveName: {
    fontSize: 13,
    color: '#6B6B6B',
    marginTop: 2,
  },
  riskLabel: {
    fontSize: 11,
    color: '#6B6B6B',
  },
  additiveDescriptionWrapper: {
    marginTop: 10,
    marginLeft: 22,
  },
  additiveDescription: {
    fontSize: 13,
    color: '#4A4A4A',
    lineHeight: 19,
  },
  additiveSourceNote: {
    fontSize: 11,
    color: '#9E9E9E',
    marginTop: 6,
  },
  nutrimentRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingVertical: 10,
    borderBottomWidth: 1,
    borderBottomColor: '#EEEEEE',
  },
  nutrimentLabel: {
    fontSize: 14,
    color: '#1A1A1A',
  },
  nutrimentValue: {
    fontSize: 14,
    color: '#6B6B6B',
    fontWeight: '600',
  },
  ingredientsText: {
    fontSize: 14,
    color: '#1A1A1A',
    lineHeight: 21,
  },
  disclaimer: {
    fontSize: 11,
    color: '#9E9E9E',
    textAlign: 'center',
    marginTop: 32,
    marginHorizontal: 24,
    lineHeight: 16,
  },
  attribution: {
    fontSize: 11,
    color: '#9E9E9E',
    textAlign: 'center',
    marginTop: 8,
  },
  alternativesSection: {
    marginTop: 32,
    paddingHorizontal: 20,
  },
  upgradeButton: {
    marginTop: 12,
    alignSelf: 'flex-start',
    backgroundColor: '#1A1A1A',
    borderRadius: 12,
    paddingVertical: 10,
    paddingHorizontal: 18,
  },
  upgradeButtonText: {
    color: '#FFFFFF',
    fontSize: 13,
    fontWeight: '600',
  },
  alternativeRow: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingVertical: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#EEEEEE',
  },
  scoreDot: {
    width: 10,
    height: 10,
    borderRadius: 5,
    marginRight: 12,
  },
  alternativeRowText: {
    flex: 1,
  },
});
