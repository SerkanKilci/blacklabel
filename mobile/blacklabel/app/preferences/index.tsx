import { useRouter } from 'expo-router';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { ActivityIndicator, Alert, Pressable, ScrollView, StyleSheet, Text, TextInput, View } from 'react-native';

import { useCreateHouseholdProfile, useDeleteHouseholdProfile, useHouseholdProfiles } from '../../src/hooks/useHouseholdProfiles';
import type { HouseholdProfile } from '../../src/types/preferences';

export default function HouseholdProfilesScreen() {
  const { t } = useTranslation();
  const router = useRouter();
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
    <ScrollView style={styles.container} contentContainerStyle={styles.content}>
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
    </ScrollView>
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
  content: {
    paddingHorizontal: 20,
    paddingTop: 60,
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
});
