import i18n from 'i18next';
import HttpBackend from 'i18next-http-backend';
import commonDe from '../assets/i18n/de/common.de.json';
import commonEn from '../assets/i18n/en/common.en.json';
import { environment } from '../environments/environment';

if (!i18n.isInitialized) {
  i18n
    .use(HttpBackend)
    // .use(LanguageDetector)
    .init({
      fallbackLng: 'en',
      debug: environment.production ? false : true,
      ns: ['common'],
      defaultNS: 'common',
      interpolation: {
        escapeValue: false,
      },
      resources: {
        en: {
          common: commonEn,
        },
        de: {
          common: commonDe,
        },
      },
      backend: {
        loadPath: 'assets/i18n/{{lng}}/{{ns}}.json',
      },
    });
}
export const changeLanguage = (lng: string) => {
  i18n.changeLanguage(lng);
};

export default i18n;
