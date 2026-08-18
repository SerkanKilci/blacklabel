import { useLocalSearchParams, useRouter } from 'expo-router';
import { useEffect, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import {
  ActivityIndicator,
  Alert,
  FlatList,
  Pressable,
  StyleSheet,
  Switch,
  Text,
  TextInput,
  View,
} from 'react-native';

import { ALLERGEN_CODES } from '../../src/constants/allergens';
import { useAdditives } from '../../src/hooks/useAdditives';
import { useDeleteHouseholdProfile, useHouseholdProfiles, useUpdateHouseholdProfile } from '../../src/hooks/useHouseholdProfiles';
import { EMPTY_PROFILE_FORM, type DietFlags, type ProfileFormValues } from '../../src/types/preferences';

const DIET_FLAG_KEYS: Array<keyof DietFlags> = [
  'vegan',
  'vegetarian',
  'glutenFree',
  'lactoseFree',
  'noPalmOil',
  'lowSugar',
  'lowSalt',
];

export default function EditHouseholdProfileScreen() {
  const { profileId } = useLocalSearchParams<{ profileId: string }>();
  const router = useRouter();
  const { t, i18n } = useTranslation();
  const isTurkish = i18n.language === 'tr';
  const { data: profiles, isLoading, isError } = useHouseholdProfiles();
  const { data: additives } = useAdditives();
  const updateMutation = useUpdateHouseholdProfile();
  const deleteMutation = useDeleteHouseholdProfile();

  const profile = profiles?.find((p) => p.id === profileId);

  const [draft, setDraft] = useState<ProfileFormValues>(EMPTY_PROFILE_FORM);
  const [additiveQuery, setAdditiveQuery] = useState('');
  const [hasLoadedDraft, setHasLoadedDraft] = useState(false);

  useEffect(() => {
    if (profile && !hasLoadedDraft) {
      setDraft({
        name: profile.name,
        avoidedAdditiveCodes: profile.avoidedAdditiveCodes,
        allergenCodes: profile.allergenCodes,
        dietFlags: profile.dietFlags,
      });
      setHasLoadedDraft(true);
    }
  }, [profile, hasLoadedDraft]);

  const toggleAllergen = (code: string) => {
    setDraft((prev) => ({
      ...prev,
      allergenCodes: prev.allergenCodes.includes(code)
        ? prev.allergenCodes.filter((c) => c !== code)
        : [...prev.allergenCodes, code],
    }));
  };

  const toggleDietFlag = (key: keyof DietFlags) => {
    setDraft((prev) => ({ ...prev, dietFlags: { ...prev.dietFlags, [key]: !prev.dietFlags[key] } }));
  };

  const toggleAvoidedAdditive = (code: string) => {
    setDraft((prev) => ({
      ...prev,
      avoidedAdditiveCodes: prev.avoidedAdditiveCodes.includes(code)
        ? prev.avoidedAdditiveCodes.filter((c) => c !== code)
        : [...prev.avoidedAdditiveCodes, code],
    }));
  };

  const handleDelete = () => {
    if (!profile) return;
    Alert.alert(
      t('preferences.profiles.deleteConfirmTitle'),
      t('preferences.profiles.deleteConfirmMessage', { name: profile.name }),
      [
        { text: t('preferences.profiles.cancel'), style: 'cancel' },
        {
          text: t('preferences.profiles.delete'),
          style: 'destructive',
          onPress: () => deleteMutation.mutate(profile.id, { onSuccess: () => router.back() }),
        },
      ],
    );
  };

  const filteredAdditives = useMemo(() => {
    if (!additives) return [];
    const normalizedQuery = additiveQuery.trim().toLowerCase();
    if (!normalizedQuery) return additives;
    return additives.filter(
      (additive) =>
        additive.code.toLowerCase().includes(normalizedQuery) ||
        additive.nameTr.toLowerCase().includes(normalizedQuery) ||
        additive.nameEn.toLowerCase().includes(normalizedQuery),
    );
  }, [additives, additiveQuery]);

  if (isLoading) {
    return (
      <View style={styles.centered}>
        <ActivityIndicator size="large" color="#1A1A1A" />
      </View>
    );
  }

  if (isError || !profile) {
    return (
      <View style={styles.centered}>
        <Text style={styles.centeredText}>{t('preferences.loadError')}</Text>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <FlatList
        data={filteredAdditives}
        keyExtractor={(item) => item.code}
        ListHeaderComponent={
          <View>
            <TextInput
              style={styles.nameInput}
              value={draft.name}
              onChangeText={(name) => setDraft((prev) => ({ ...prev, name }))}
              placeholder={t('preferences.profiles.addPlaceholder')}
            />

            <Text style={styles.sectionTitle}>{t('preferences.allergensSection')}</Text>
            {ALLERGEN_CODES.map((code) => (
              <Pressable key={code} style={styles.checkboxRow} onPress={() => toggleAllergen(code)}>
                <View style={[styles.checkbox, draft.allergenCodes.includes(code) && styles.checkboxChecked]} />
                <Text style={styles.checkboxLabel}>{t(`allergens.${code}`)}</Text>
              </Pressable>
            ))}

            <Text style={styles.sectionTitle}>{t('preferences.dietSection')}</Text>
            {DIET_FLAG_KEYS.map((key) => (
              <View key={key} style={styles.switchRow}>
                <Text style={styles.checkboxLabel}>{t(`dietFlags.${key}`)}</Text>
                <Switch value={draft.dietFlags[key]} onValueChange={() => toggleDietFlag(key)} />
              </View>
            ))}

            <Text style={styles.sectionTitle}>{t('preferences.avoidedAdditivesSection')}</Text>
            <TextInput
              style={styles.searchInput}
              placeholder={t('preferences.searchAdditivesPlaceholder')}
              value={additiveQuery}
              onChangeText={setAdditiveQuery}
              autoCapitalize="none"
              autoCorrect={false}
            />
          </View>
        }
        renderItem={({ item }) => (
          <Pressable style={styles.checkboxRow} onPress={() => toggleAvoidedAdditive(item.code)}>
            <View style={[styles.checkbox, draft.avoidedAdditiveCodes.includes(item.code) && styles.checkboxChecked]} />
            <Text style={styles.checkboxLabel}>
              {item.code} — {isTurkish ? item.nameTr : item.nameEn}
            </Text>
          </Pressable>
        )}
        ListEmptyComponent={
          additiveQuery.trim() ? <Text style={styles.noResultsText}>{t('preferences.noAdditivesFound')}</Text> : null
        }
        ListFooterComponent={
          <Pressable style={styles.deleteProfileButton} onPress={handleDelete}>
            <Text style={styles.deleteProfileButtonText}>{t('preferences.profiles.delete')}</Text>
          </Pressable>
        }
        contentContainerStyle={styles.listContent}
      />

      <View style={styles.footer}>
        {updateMutation.isError && <Text style={styles.errorText}>{t('preferences.saveError')}</Text>}
        {updateMutation.isSuccess && <Text style={styles.successText}>{t('preferences.saved')}</Text>}
        <Pressable
          style={styles.saveButton}
          onPress={() => updateMutation.mutate({ id: profile.id, values: draft })}
          disabled={updateMutation.isPending}
        >
          <Text style={styles.saveButtonText}>
            {updateMutation.isPending ? t('preferences.saving') : t('preferences.save')}
          </Text>
        </Pressable>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#FFFFFF',
  },
  centered: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#FFFFFF',
  },
  centeredText: {
    fontSize: 14,
    color: '#6B6B6B',
  },
  listContent: {
    paddingHorizontal: 20,
    paddingTop: 60,
    paddingBottom: 20,
  },
  nameInput: {
    fontSize: 24,
    fontWeight: '700',
    color: '#1A1A1A',
    marginBottom: 20,
    paddingVertical: 4,
    borderBottomWidth: 1,
    borderBottomColor: '#EEEEEE',
  },
  sectionTitle: {
    fontSize: 14,
    fontWeight: '700',
    color: '#1A1A1A',
    marginTop: 24,
    marginBottom: 8,
  },
  checkboxRow: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingVertical: 8,
  },
  checkbox: {
    width: 20,
    height: 20,
    borderRadius: 6,
    borderWidth: 2,
    borderColor: '#CCCCCC',
    marginRight: 12,
  },
  checkboxChecked: {
    backgroundColor: '#1A1A1A',
    borderColor: '#1A1A1A',
  },
  checkboxLabel: {
    fontSize: 14,
    color: '#1A1A1A',
    flex: 1,
  },
  switchRow: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingVertical: 8,
  },
  searchInput: {
    backgroundColor: '#F5F5F5',
    borderRadius: 12,
    paddingHorizontal: 16,
    paddingVertical: 10,
    fontSize: 14,
    color: '#1A1A1A',
    marginBottom: 8,
  },
  noResultsText: {
    fontSize: 13,
    color: '#9E9E9E',
    textAlign: 'center',
    marginTop: 12,
  },
  deleteProfileButton: {
    marginTop: 32,
    alignItems: 'center',
    paddingVertical: 12,
  },
  deleteProfileButtonText: {
    color: '#C62828',
    fontSize: 14,
    fontWeight: '600',
  },
  footer: {
    borderTopWidth: 1,
    borderTopColor: '#EEEEEE',
    paddingHorizontal: 20,
    paddingVertical: 16,
  },
  saveButton: {
    backgroundColor: '#1A1A1A',
    borderRadius: 12,
    paddingVertical: 14,
    alignItems: 'center',
  },
  saveButtonText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: '600',
  },
  errorText: {
    color: '#C62828',
    fontSize: 12,
    marginBottom: 8,
    textAlign: 'center',
  },
  successText: {
    color: '#2E7D32',
    fontSize: 12,
    marginBottom: 8,
    textAlign: 'center',
  },
});
