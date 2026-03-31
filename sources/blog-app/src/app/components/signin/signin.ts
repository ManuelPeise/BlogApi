import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthenticationService } from '../../services/AuthenticationService';
import { Formbutton } from '../customComponents/formbutton/formbutton';
import { Textfield } from '../customComponents/textfield/textfield';
import i18n from '../../I18nService';

@Component({
  selector: 'app-signin',
  imports: [ReactiveFormsModule, RouterLink, Formbutton, Textfield],
  templateUrl: './signin.html',
  styleUrls: ['./signin.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Signin {
  private readonly authService = inject(AuthenticationService);
  private readonly formBuilder = inject(FormBuilder);
  labels: any;

  constructor() {
    this.loadLabels();
  }
  readonly signInForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  readonly errorMessage = signal<string | null>(null);

  loadLabels() {
    this.labels = {
      captionSignIn: i18n.t('captionSignIn', { ns: 'common' }),
      subTitleSignIn: i18n.t('subTitleSignIn', { ns: 'common' }),
      labelEmail: i18n.t('labelEmail', { ns: 'common' }),
      labelPassword: i18n.t('labelPassword', { ns: 'common' }),
      labelCancel: i18n.t('labelCancel', { ns: 'common' }),
      labelSignIn: i18n.t('labelSignIn', { ns: 'common' }),
      errorInvalidCredentials: i18n.t('errorInvalidCredentials', { ns: 'common' }),
      labelDontHaveAccount: i18n.t('labelDontHaveAccount', { ns: 'common' }),
      labelSignUp: i18n.t('labelSignUp', { ns: 'common' }),
      captionWelcomeBack: i18n.t('captionWelcome', { ns: 'common' }),
      subTitleWelcomeBack: i18n.t('subTitleWelcome', { ns: 'common' }),
    };
  }

  handleSignIn(): void {
    if (this.signInForm.valid) {
      this.authService.signIn({
        email: this.signInForm.value.email!,
        password: this.signInForm.value.password!,
      });

      if (this.authService.authenticationError) {
        this.errorMessage.set(this.labels.errorInvalidCredentials);
        return;
      }

      this.errorMessage.set(null);
    }
  }
}
