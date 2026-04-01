import { ChangeDetectionStrategy, Component } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import i18n from '../../I18nService';

@Component({
  selector: 'app-imressum-page',
  imports: [],
  templateUrl: './imressum-page.html',
  styleUrl: './imressum-page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ImressumPage {
  env = environment;
  labels: any;

  constructor() {
    this.loadLabels();
  }

  loadLabels() {
    this.labels = {
      captionImpressum: i18n.t('captionImpressum', { ns: 'impressum' }),
      captionContact: i18n.t('captionContact', { ns: 'impressum' }),
      captionResponsibility: i18n.t('captionResponsibility', { ns: 'impressum' }),
      captionLiability: i18n.t('captionLiability', { ns: 'impressum' }),
      liabilityText1: i18n.t('liabilityText1', { ns: 'impressum' }),
      liabilityText2: i18n.t('liabilityText2', { ns: 'impressum' }),
      liabilityText3: i18n.t('liabilityText3', { ns: 'impressum' }),
      liabilityText4: i18n.t('liabilityText4', { ns: 'impressum' }),
      liabilityText5: i18n.t('liabilityText5', { ns: 'impressum' }),
      captionLiabilityLinks: i18n.t('captionLiabilityLinks', { ns: 'impressum' }),
      liabilityLinkText: i18n.t('liabilityLinkText', { ns: 'impressum' }),
    };
  }
}
