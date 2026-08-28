import { getLocales } from 'expo-localization';
import i18next from 'i18next';
import { initReactI18next } from 'react-i18next';

import de from './locales/de.json';
import en from './locales/en.json';
import es from './locales/es.json';
import fr from './locales/fr.json';
import tr from './locales/tr.json';

const SUPPORTED_LANGUAGES = ['en', 'tr', 'de', 'fr', 'es'] as const;

const deviceLanguage = getLocales()[0]?.languageCode ?? 'en';
const supportedLanguage = (SUPPORTED_LANGUAGES as readonly string[]).includes(deviceLanguage)
  ? deviceLanguage
  : 'en';

void i18next.use(initReactI18next).init({
  resources: {
    en: { translation: en },
    tr: { translation: tr },
    de: { translation: de },
    fr: { translation: fr },
    es: { translation: es },
  },
  lng: supportedLanguage,
  fallbackLng: 'en',
  interpolation: {
    escapeValue: false,
  },
});

export default i18next;
