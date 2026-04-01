import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { environment } from '../../../environments/environment';
import i18n from '../../I18nService';

@Component({
  selector: 'app-footer',
  imports: [RouterLink],
  templateUrl: './footer.html',
  styleUrl: './footer.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Footer {
  readonly year = new Date().getFullYear();
  readonly appName = environment.appName;

  readonly labels = {
    impressum: i18n.t('labelImpressum', { ns: 'common' }),
    privacyPolicy: i18n.t('labelPrivacyPolicy', { ns: 'common' }),
  };
}
