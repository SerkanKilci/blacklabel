import type { AdditiveInfo } from '../types/product';

/** The additive catalog only has translated name/description text for these 5 languages —
 * any other active app language (should never happen, i18n's supportedLngs is the same 5) falls
 * back to English rather than showing an empty string. */
export function getLocalizedAdditiveName(additive: AdditiveInfo, language: string): string {
  switch (language) {
    case 'tr':
      return additive.nameTr;
    case 'de':
      return additive.nameDe;
    case 'fr':
      return additive.nameFr;
    case 'es':
      return additive.nameEs;
    default:
      return additive.nameEn;
  }
}

export function getLocalizedAdditiveDescription(additive: AdditiveInfo, language: string): string {
  switch (language) {
    case 'tr':
      return additive.descriptionTr;
    case 'de':
      return additive.descriptionDe;
    case 'fr':
      return additive.descriptionFr;
    case 'es':
      return additive.descriptionEs;
    default:
      return additive.descriptionEn;
  }
}
