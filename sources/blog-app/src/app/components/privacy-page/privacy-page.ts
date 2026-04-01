import { ChangeDetectionStrategy, Component } from '@angular/core';
import { environment } from '../../../environments/environment';
import i18n from '../../I18nService';

@Component({
  selector: 'app-privacy-page',
  imports: [],
  templateUrl: './privacy-page.html',
  styleUrl: './privacy-page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PrivacyPage {
  readonly env = environment;
  readonly labels = {
    captionPrivacyPolicy: i18n.t('captionPrivacyPolicy', { ns: 'dataProtection' }),
    captionGeneralInfo: i18n.t('captionGeneralInfo', { ns: 'dataProtection' }),
    textGeneralInfo: i18n.t('textGeneralInfo', { ns: 'dataProtection' }),
    captionResponsibleParty: i18n.t('captionResponsibleParty', { ns: 'dataProtection' }),
    captionDataCollection: i18n.t('captionDataCollection', { ns: 'dataProtection' }),
    textDataCollectionIntro: i18n.t('textDataCollectionIntro', { ns: 'dataProtection' }),
    textDataCollectionListIntro: i18n.t('textDataCollectionListIntro', { ns: 'dataProtection' }),
    dataCollectionItemFirstLastName: i18n.t('dataCollectionItemFirstLastName', {
      ns: 'dataProtection',
    }),
    dataCollectionItemEmail: i18n.t('dataCollectionItemEmail', { ns: 'dataProtection' }),
    dataCollectionItemProfilePicture: i18n.t('dataCollectionItemProfilePicture', {
      ns: 'dataProtection',
    }),
    dataCollectionItemBirthdate: i18n.t('dataCollectionItemBirthdate', { ns: 'dataProtection' }),
    dataCollectionItemAddress: i18n.t('dataCollectionItemAddress', { ns: 'dataProtection' }),
    dataCollectionItemCredentials: i18n.t('dataCollectionItemCredentials', {
      ns: 'dataProtection',
    }),
    dataCollectionItemUsageInfo: i18n.t('dataCollectionItemUsageInfo', { ns: 'dataProtection' }),
    textDataCollectionRequired: i18n.t('textDataCollectionRequired', { ns: 'dataProtection' }),
    textDataCollectionOptional: i18n.t('textDataCollectionOptional', { ns: 'dataProtection' }),
    captionStorageDeletion: i18n.t('captionStorageDeletion', { ns: 'dataProtection' }),
    textStorageActive: i18n.t('textStorageActive', { ns: 'dataProtection' }),
    textStorageDeletionRequest: i18n.t('textStorageDeletionRequest', { ns: 'dataProtection' }),
    textStorageRecovery: i18n.t('textStorageRecovery', { ns: 'dataProtection' }),
    captionDataPurpose: i18n.t('captionDataPurpose', { ns: 'dataProtection' }),
    textDataPurposeIntro: i18n.t('textDataPurposeIntro', { ns: 'dataProtection' }),
    dataPurposeItemPlatform: i18n.t('dataPurposeItemPlatform', { ns: 'dataProtection' }),
    dataPurposeItemAccounts: i18n.t('dataPurposeItemAccounts', { ns: 'dataProtection' }),
    dataPurposeItemBlogContent: i18n.t('dataPurposeItemBlogContent', { ns: 'dataProtection' }),
    dataPurposeItemSecurity: i18n.t('dataPurposeItemSecurity', { ns: 'dataProtection' }),
    captionAuthentication: i18n.t('captionAuthentication', { ns: 'dataProtection' }),
    textAuthenticationJwt: i18n.t('textAuthenticationJwt', { ns: 'dataProtection' }),
    textAuthenticationStorage: i18n.t('textAuthenticationStorage', { ns: 'dataProtection' }),
    captionDataDisclosure: i18n.t('captionDataDisclosure', { ns: 'dataProtection' }),
    textDataDisclosureIntro: i18n.t('textDataDisclosureIntro', { ns: 'dataProtection' }),
    dataDisclosureItemLegal: i18n.t('dataDisclosureItemLegal', { ns: 'dataProtection' }),
    dataDisclosureItemConsent: i18n.t('dataDisclosureItemConsent', { ns: 'dataProtection' }),
  };
}
