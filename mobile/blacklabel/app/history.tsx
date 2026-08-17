import { useFocusEffect, useRouter } from 'expo-router';
import { useCallback, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import {
  Pressable,
  RefreshControl,
  SectionList,
  StyleSheet,
  Text,
  TextInput,
  View,
} from 'react-native';

import { getCachedProductName } from '../src/db/productCache';
import { getAllScans, type LocalScan } from '../src/db/scanHistory';
import { syncPendingScans } from '../src/sync/syncScans';
import { getScoreColor } from '../src/utils/score';

interface ScanWithName extends LocalScan {
  productName: string | null;
}

interface Section {
  title: string;
  data: ScanWithName[];
}

function formatSectionTitle(dateKey: string, todayKey: string, yesterdayKey: string, todayLabel: string, yesterdayLabel: string): string {
  if (dateKey === todayKey) return todayLabel;
  if (dateKey === yesterdayKey) return yesterdayLabel;
  return dateKey;
}

function toDateKey(iso: string): string {
  return iso.slice(0, 10);
}

export default function HistoryScreen() {
  const { t } = useTranslation();
  const router = useRouter();
  const [scans, setScans] = useState<ScanWithName[]>([]);
  const [query, setQuery] = useState('');
  const [isRefreshing, setIsRefreshing] = useState(false);

  const loadScans = useCallback(async () => {
    const localScans = await getAllScans();
    const namesByBarcode = new Map<string, string | null>();
    for (const scan of localScans) {
      if (!namesByBarcode.has(scan.barcode)) {
        namesByBarcode.set(scan.barcode, await getCachedProductName(scan.barcode));
      }
    }
    setScans(localScans.map((scan) => ({ ...scan, productName: namesByBarcode.get(scan.barcode) ?? null })));
  }, []);

  useFocusEffect(
    useCallback(() => {
      void loadScans();
    }, [loadScans]),
  );

  const handleRefresh = async () => {
    setIsRefreshing(true);
    await syncPendingScans();
    await loadScans();
    setIsRefreshing(false);
  };

  const sections = useMemo<Section[]>(() => {
    const normalizedQuery = query.trim().toLowerCase();
    const filtered = normalizedQuery
      ? scans.filter(
          (scan) =>
            scan.barcode.toLowerCase().includes(normalizedQuery) ||
            (scan.productName?.toLowerCase().includes(normalizedQuery) ?? false),
        )
      : scans;

    const now = new Date();
    const todayKey = toDateKey(now.toISOString());
    const yesterdayKey = toDateKey(new Date(now.getTime() - 24 * 60 * 60 * 1000).toISOString());

    const grouped = new Map<string, ScanWithName[]>();
    for (const scan of filtered) {
      const key = toDateKey(scan.scannedAt);
      const bucket = grouped.get(key) ?? [];
      bucket.push(scan);
      grouped.set(key, bucket);
    }

    return Array.from(grouped.entries())
      .sort(([a], [b]) => (a < b ? 1 : -1))
      .map(([dateKey, data]) => ({
        title: formatSectionTitle(dateKey, todayKey, yesterdayKey, t('history.today'), t('history.yesterday')),
        data,
      }));
  }, [scans, query, t]);

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Text style={styles.headerTitle}>{t('history.title')}</Text>
        <TextInput
          style={styles.searchInput}
          placeholder={t('history.searchPlaceholder')}
          value={query}
          onChangeText={setQuery}
          autoCapitalize="none"
          autoCorrect={false}
        />
      </View>

      <SectionList
        sections={sections}
        keyExtractor={(item) => item.id}
        refreshControl={<RefreshControl refreshing={isRefreshing} onRefresh={handleRefresh} />}
        ListEmptyComponent={
          <Text style={styles.emptyText}>{query.trim() ? t('history.noResults') : t('history.empty')}</Text>
        }
        renderSectionHeader={({ section }) => <Text style={styles.sectionHeader}>{section.title}</Text>}
        renderItem={({ item }) => (
          <Pressable style={styles.row} onPress={() => router.push(`/product/${item.barcode}`)}>
            <View style={[styles.scoreDot, { backgroundColor: getScoreColor(item.scoreAtScanTime) }]} />
            <View style={styles.rowText}>
              <Text style={styles.rowTitle}>{item.productName ?? item.barcode}</Text>
              <Text style={styles.rowSubtitle}>
                {new Date(item.scannedAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                {!item.synced ? ` · ${t('history.syncPending')}` : ''}
              </Text>
            </View>
          </Pressable>
        )}
        contentContainerStyle={sections.length === 0 && styles.emptyContainer}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#FFFFFF',
  },
  header: {
    paddingHorizontal: 20,
    paddingTop: 60,
    paddingBottom: 16,
  },
  headerTitle: {
    fontSize: 24,
    fontWeight: '700',
    color: '#1A1A1A',
    marginBottom: 16,
  },
  searchInput: {
    backgroundColor: '#F5F5F5',
    borderRadius: 12,
    paddingHorizontal: 16,
    paddingVertical: 10,
    fontSize: 14,
    color: '#1A1A1A',
  },
  sectionHeader: {
    fontSize: 13,
    fontWeight: '700',
    color: '#6B6B6B',
    backgroundColor: '#FFFFFF',
    paddingHorizontal: 20,
    paddingTop: 16,
    paddingBottom: 8,
  },
  row: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 20,
    paddingVertical: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#EEEEEE',
  },
  scoreDot: {
    width: 10,
    height: 10,
    borderRadius: 5,
    marginRight: 14,
  },
  rowText: {
    flex: 1,
  },
  rowTitle: {
    fontSize: 15,
    color: '#1A1A1A',
    fontWeight: '500',
  },
  rowSubtitle: {
    fontSize: 12,
    color: '#9E9E9E',
    marginTop: 2,
  },
  emptyContainer: {
    flexGrow: 1,
    justifyContent: 'center',
  },
  emptyText: {
    textAlign: 'center',
    color: '#6B6B6B',
    fontSize: 14,
    paddingHorizontal: 40,
  },
});
