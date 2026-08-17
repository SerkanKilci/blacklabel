import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Modal, Pressable, StyleSheet, Text, TextInput, View } from 'react-native';

import { normalizeBarcode } from '../utils/barcode';

interface ManualBarcodeEntryModalProps {
  visible: boolean;
  onClose: () => void;
  onSubmit: (normalizedBarcode: string) => void;
}

/** Shared by the Scanner and Compare screens so both offer the same fallback when the camera
 * can't read a barcode (damaged label, poor lighting, or — on web — no working scanner at all). */
export function ManualBarcodeEntryModal({ visible, onClose, onSubmit }: ManualBarcodeEntryModalProps) {
  const { t } = useTranslation();
  const [input, setInput] = useState('');

  const normalized = normalizeBarcode(input);

  const handleClose = () => {
    setInput('');
    onClose();
  };

  const handleSubmit = () => {
    if (!normalized) {
      return;
    }
    setInput('');
    onSubmit(normalized);
  };

  return (
    <Modal visible={visible} transparent animationType="fade" onRequestClose={handleClose}>
      <View style={styles.modalBackdrop}>
        <View style={styles.modalCard}>
          <Text style={styles.modalTitle}>{t('scanner.manualEntryTitle')}</Text>
          <Text style={styles.modalHint}>{t('scanner.manualEntryHint')}</Text>

          <TextInput
            style={styles.modalInput}
            value={input}
            onChangeText={setInput}
            keyboardType="number-pad"
            maxLength={14}
            autoFocus
            placeholder={t('scanner.manualEntryPlaceholder')}
            placeholderTextColor="#9E9E9E"
            onSubmitEditing={handleSubmit}
            returnKeyType="done"
          />
          {input.length > 0 && !normalized && <Text style={styles.modalErrorText}>{t('scanner.manualEntryInvalid')}</Text>}

          <View style={styles.modalButtonRow}>
            <Pressable style={styles.modalCancelButton} onPress={handleClose}>
              <Text style={styles.modalCancelButtonText}>{t('settings.cancel')}</Text>
            </Pressable>
            <Pressable
              style={[styles.modalSubmitButton, !normalized && styles.modalSubmitButtonDisabled]}
              onPress={handleSubmit}
              disabled={!normalized}
            >
              <Text style={styles.modalSubmitButtonText}>{t('scanner.manualEntrySubmit')}</Text>
            </Pressable>
          </View>
        </View>
      </View>
    </Modal>
  );
}

const styles = StyleSheet.create({
  modalBackdrop: {
    flex: 1,
    backgroundColor: 'rgba(0, 0, 0, 0.6)',
    alignItems: 'center',
    justifyContent: 'center',
    padding: 24,
  },
  modalCard: {
    width: '100%',
    maxWidth: 360,
    backgroundColor: '#FFFFFF',
    borderRadius: 16,
    padding: 24,
  },
  modalTitle: {
    fontSize: 18,
    fontWeight: '700',
    color: '#1A1A1A',
  },
  modalHint: {
    fontSize: 13,
    color: '#6B6B6B',
    marginTop: 6,
    marginBottom: 16,
  },
  modalInput: {
    borderWidth: 1,
    borderColor: '#CCCCCC',
    borderRadius: 12,
    paddingHorizontal: 14,
    paddingVertical: 12,
    fontSize: 16,
    color: '#1A1A1A',
    letterSpacing: 1,
  },
  modalErrorText: {
    fontSize: 12,
    color: '#C62828',
    marginTop: 8,
  },
  modalButtonRow: {
    flexDirection: 'row',
    justifyContent: 'flex-end',
    gap: 12,
    marginTop: 20,
  },
  modalCancelButton: {
    paddingVertical: 10,
    paddingHorizontal: 16,
  },
  modalCancelButtonText: {
    fontSize: 14,
    fontWeight: '500',
    color: '#6B6B6B',
  },
  modalSubmitButton: {
    backgroundColor: '#1A1A1A',
    borderRadius: 10,
    paddingVertical: 10,
    paddingHorizontal: 20,
  },
  modalSubmitButtonDisabled: {
    backgroundColor: '#CCCCCC',
  },
  modalSubmitButtonText: {
    fontSize: 14,
    fontWeight: '600',
    color: '#FFFFFF',
  },
});
