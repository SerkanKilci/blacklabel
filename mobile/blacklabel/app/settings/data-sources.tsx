import { useTranslation } from 'react-i18next';
import { ScrollView, StyleSheet, Text, View } from 'react-native';

export default function DataSourcesScreen() {
  const { t } = useTranslation();

  return (
    <ScrollView style={styles.container} contentContainerStyle={styles.content}>
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
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#FFFFFF',
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
