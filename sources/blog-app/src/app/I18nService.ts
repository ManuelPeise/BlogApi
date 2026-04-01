import i18n from 'i18next';
import HttpBackend from 'i18next-http-backend';
import commonDe from '../assets/i18n/de/common.de.json';
import impressumDe from '../assets/i18n/de/impressum.de.json';
import dataProtectionDe from '../assets/i18n/de/dataProtection.de.json';
import commonEn from '../assets/i18n/en/common.en.json';
import impressumEn from '../assets/i18n/en/impressum.en.json';
import dataProtectionEn from '../assets/i18n/en/dataProtection.en.json';
import { environment } from '../environments/environment';

if (!i18n.isInitialized) {
  i18n.use(HttpBackend).init({
    fallbackLng: 'de',
    debug: environment.production ? false : true,
    ns: ['common', 'impressum', 'dataProtection'],
    defaultNS: 'common',
    interpolation: {
      escapeValue: false,
    },
    resources: {
      en: {
        common: commonEn,
        impressum: impressumEn,
        dataProtection: dataProtectionEn,
      },
      de: {
        common: commonDe,
        impressum: impressumDe,
        dataProtection: dataProtectionDe,
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
