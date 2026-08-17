import { useTranslation } from 'react-i18next';
import { ScrollView, StyleSheet, Text, View } from 'react-native';

export default function TermsScreen() {
  const { t } = useTranslation();

  return (
    <ScrollView style={styles.container} contentContainerStyle={styles.content}>
      <Text style={styles.title}>{t('terms.title')}</Text>
      <Text style={styles.body}>{t('terms.body')}</Text>

      <View style={styles.contactSection}>
        <Text style={styles.contactLabel}>{t('terms.contactLabel')}</Text>
        <Text style={styles.contactValue}>legal@blacklabel.app</Text>
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
    marginBottom: 20,
  },
  body: {
    fontSize: 14,
    color: '#4A4A4A',
    lineHeight: 22,
  },
  contactSection: {
    marginTop: 28,
    paddingTop: 20,
    borderTopWidth: 1,
    borderTopColor: '#EEEEEE',
  },
  contactLabel: {
    fontSize: 13,
    fontWeight: '700',
    color: '#1A1A1A',
    marginBottom: 6,
  },
  contactValue: {
    fontSize: 13,
    color: '#6B6B6B',
    marginTop: 2,
  },
});
