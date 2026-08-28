import { useRouter } from 'expo-router';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import {
  ActivityIndicator,
  Alert,
  KeyboardAvoidingView,
  Platform,
  Pressable,
  ScrollView,
  StyleSheet,
  Text,
  TextInput,
  View,
} from 'react-native';

import { BackButton } from '../../src/components/BackButton';
import { Screen } from '../../src/components/Screen';
import { useCreateHouseholdProfile, useDeleteHouseholdProfile, useHouseholdProfiles } from '../../src/hooks/useHouseholdProfiles';
import { useSubscription } from '../../src/hooks/useSubscription';
import type { HouseholdProfile } from '../../src/types/preferences';

export default function HouseholdProfilesScreen() {
  const { t } = useTranslation();
  const router = useRouter();
  const { data: subscription } = useSubscription();
  const isPremium = subscription?.isPremium ?? false;
  const { data: profiles, isLoading, isError } = useHouseholdProfiles();
  const createMutation = useCreateHouseholdProfile();
  const deleteMutation = useDeleteHouseholdProfile();
  const [newName, setNewName] = useState('');

  const handleAdd = () => {
    const name = newName.trim();
    if (!name) {
      return;
    }
    createMutation.mutate(name, {
      onSuccess: (profile) => {
        setNewName('');
        router.push(`/preferences/${profile.id}`);
      },
    });
  };

  const handleDelete = (profile: HouseholdProfile) => {
    Alert.alert(
      t('preferences.profiles.deleteConfirmTitle'),
      t('preferences.profiles.deleteConfirmMessage', { name: profile.name }),
      [
        { text: t('preferences.profiles.cancel'), style: 'cancel' },
        { text: t('preferences.profiles.delete'), style: 'destructive', onPress: () => deleteMutation.mutate(profile.id) },
      ],
    );
  };

  if (isLoading) {
    return (
      <View style={styles.centered}>
        <ActivityIndicator size="large" color="#1A1A1A" />
      </View>
    );
  }

  if (isError) {
    return (
      <View style={styles.centered}>
        <Text style={styles.centeredText}>{t('preferences.profiles.loadError')}</Text>
      </View>
    );
  }

  return (
    <Screen style={styles.container}>
    <KeyboardAvoidingView style={styles.keyboardAvoider} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
    <ScrollView contentContainerStyle={styles.content} keyboardShouldPersistTaps="handled">
      <BackButton />
      <Text style={styles.title}>{t('preferences.profiles.title')}</Text>
      <Text style={styles.subtitle}>{t('preferences.profiles.subtitle')}</Text>

      {profiles && profiles.length === 0 && <Text style={styles.emptyText}>{t('preferences.profiles.empty')}</Text>}

      {profiles?.map((profile) => (
        <View key={profile.id} style={styles.profileRow}>
          <Pressable style={styles.profileRowMain} onPress={() => router.push(`/preferences/${profile.id}`)}>
            <Text style={styles.profileName}>{profile.name}</Text>
            <Text style={styles.chevron}>›</Text>
          </Pressable>
          <Pressable style={styles.deleteButton} onPress={() => handleDelete(profile)} hitSlop={8}>
            <Text style={styles.deleteButtonText}>✕</Text>
          </Pressable>
        </View>
      ))}

      {isPremium ? (
        <View style={styles.addRow}>
          <TextInput
            style={styles.addInput}
            placeholder={t('preferences.profiles.addPlaceholder')}
            value={newName}
            onChangeText={setNewName}
            onSubmitEditing={handleAdd}
          />
          <Pressable style={styles.addButton} onPress={handleAdd} disabled={createMutation.isPending}>
            <Text style={styles.addButtonText}>{t('preferences.profiles.add')}</Text>
          </Pressable>
        </View>
      ) : (
        <View style={styles.premiumCard}>
          <Text style={styles.premiumCardTitle}>{t('preferences.profiles.premiumTitle')}</Text>
          <Text style={styles.premiumCardMessage}>{t('preferences.profiles.premiumMessage')}</Text>
          <Pressable style={styles.premiumCardButton} onPress={() => router.push('/paywall')}>
            <Text style={styles.premiumCardButtonText}>{t('preferences.profiles.upgrade')}</Text>
          </Pressable>
        </View>
      )}
    </ScrollView>
    </KeyboardAvoidingView>
    </Screen>
  );
}

const styles = StyleSheet.create({
  container: {
    backgroundColor: '#FFFFFF',
  },
  keyboardAvoider: {
    flex: 1,
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
  content: {
    paddingHorizontal: 20,
    paddingTop: 16,
    paddingBottom: 40,
  },
  title: {
    fontSize: 24,
    fontWeight: '700',
    color: '#1A1A1A',
  },
  subtitle: {
    fontSize: 14,
    color: '#6B6B6B',
    marginTop: 8,
    lineHeight: 20,
  },
  emptyText: {
    fontSize: 14,
    color: '#9E9E9E',
    textAlign: 'center',
    marginTop: 32,
  },
  profileRow: {
    flexDirection: 'row',
    alignItems: 'center',
    marginTop: 20,
    borderBottomWidth: 1,
    borderBottomColor: '#EEEEEE',
    paddingBottom: 12,
  },
  profileRowMain: {
    flex: 1,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
  },
  profileName: {
    fontSize: 16,
    fontWeight: '600',
    color: '#1A1A1A',
  },
  chevron: {
    fontSize: 20,
    color: '#CCCCCC',
  },
  deleteButton: {
    paddingLeft: 16,
    paddingVertical: 4,
  },
  deleteButtonText: {
    fontSize: 14,
    color: '#9E9E9E',
  },
  addRow: {
    flexDirection: 'row',
    marginTop: 28,
    gap: 8,
  },
  addInput: {
    flex: 1,
    backgroundColor: '#F5F5F5',
    borderRadius: 12,
    paddingHorizontal: 16,
    paddingVertical: 10,
    fontSize: 14,
    color: '#1A1A1A',
  },
  addButton: {
    backgroundColor: '#1A1A1A',
    borderRadius: 12,
    paddingHorizontal: 18,
    justifyContent: 'center',
  },
  addButtonText: {
    color: '#FFFFFF',
    fontSize: 14,
    fontWeight: '600',
  },
  premiumCard: {
    marginTop: 28,
    padding: 16,
    borderRadius: 12,
    backgroundColor: '#F5F5F5',
  },
  premiumCardTitle: {
    fontSize: 14,
    fontWeight: '700',
    color: '#1A1A1A',
  },
  premiumCardMessage: {
    fontSize: 13,
    color: '#6B6B6B',
    marginTop: 4,
    lineHeight: 19,
  },
  premiumCardButton: {
    marginTop: 12,
    alignSelf: 'flex-start',
    backgroundColor: '#1A1A1A',
    borderRadius: 12,
    paddingVertical: 10,
    paddingHorizontal: 18,
  },
  premiumCardButtonText: {
    color: '#FFFFFF',
    fontSize: 13,
    fontWeight: '600',
  },
});
