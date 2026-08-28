import { useTranslation } from 'react-i18next';
import { ScrollView, StyleSheet, Text, View } from 'react-native';

import { BackButton } from '../../src/components/BackButton';
import { Screen } from '../../src/components/Screen';

export default function TermsScreen() {
  const { t } = useTranslation();

  return (
    <Screen style={styles.container}>
      <ScrollView contentContainerStyle={styles.content}>
        <BackButton />
        <Text style={styles.title}>{t('terms.title')}</Text>
        <Text style={styles.body}>{t('terms.body')}</Text>

        <View style={styles.contactSection}>
          <Text style={styles.contactLabel}>{t('terms.contactLabel')}</Text>
          <Text style={styles.contactValue}>legal@blacklabel.app</Text>
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
