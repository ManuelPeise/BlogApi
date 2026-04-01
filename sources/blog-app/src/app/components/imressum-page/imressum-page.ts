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
      captionImpressum: i18n.t('captionImpressum', { ns: 'common' }),
      captionContact: i18n.t('captionContact', { ns: 'common' }),
      captionResponsibility: i18n.t('captionResponsibility', { ns: 'common' }),
      captionLiability: i18n.t('captionLiability', { ns: 'common' }),
      liabilityText1: i18n.t('liabilityText1', { ns: 'common' }),
      liabilityText2: i18n.t('liabilityText2', { ns: 'common' }),
      liabilityText3: i18n.t('liabilityText3', { ns: 'common' }),
      liabilityText4: i18n.t('liabilityText4', { ns: 'common' }),
      liabilityText5: i18n.t('liabilityText5', { ns: 'common' }),
      captionLiabilityLinks: i18n.t('captionLiabilityLinks', { ns: 'common' }),
      liabilityLinkText: i18n.t('liabilityLinkText', { ns: 'common' }),
    };
  }
}
