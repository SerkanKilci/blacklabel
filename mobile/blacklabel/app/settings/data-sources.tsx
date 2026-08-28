import { useTranslation } from 'react-i18next';
import { ScrollView, StyleSheet, Text, View } from 'react-native';

import { BackButton } from '../../src/components/BackButton';
import { Screen } from '../../src/components/Screen';

export default function DataSourcesScreen() {
  const { t } = useTranslation();

  return (
    <Screen style={styles.container}>
      <ScrollView contentContainerStyle={styles.content}>
        <BackButton />
        <Text style={styles.title}>{t('dataSources.title')}</Text>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>{t('dataSources.offTitle')}</Text>
          <Text style={styles.body}>{t('dataSources.offBody')}</Text>
          <Text style={styles.attribution}>{t('dataSources.offAttribution')}</Text>
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>{t('dataSources.visionTitle')}</Text>
          <Text style={styles.body}>{t('dataSources.visionBody')}</Text>
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>{t('dataSources.revenueCatTitle')}</Text>
          <Text style={styles.body}>{t('dataSources.revenueCatBody')}</Text>
        </View>
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
    marginBottom: 24,
  },
  section: {
    marginBottom: 28,
  },
  sectionTitle: {
    fontSize: 15,
    fontWeight: '700',
    color: '#1A1A1A',
    marginBottom: 8,
  },
  body: {
    fontSize: 14,
    color: '#4A4A4A',
    lineHeight: 21,
  },
  attribution: {
    fontSize: 12,
    color: '#9E9E9E',
    marginTop: 8,
  },
});
