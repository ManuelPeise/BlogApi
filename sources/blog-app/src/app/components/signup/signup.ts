import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthenticationService } from '../../services/AuthenticationService';
import { Textfield } from '../customComponents/textfield/textfield';
import { Formbutton } from '../customComponents/formbutton/formbutton';
import { passwordMatchValidator } from '../../lib/validation';
import i18n from '../../I18nService';

@Component({
  selector: 'app-signup',
  imports: [ReactiveFormsModule, RouterLink, Formbutton, Textfield],
  templateUrl: './signup.html',
  styleUrl: './signup.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Signup {
  private readonly authService = inject(AuthenticationService);
  private readonly formBuilder = inject(FormBuilder);
  private readonly router = inject(Router);
  labels: any;

  constructor() {
    this.loadLabels();
  }

  readonly signupForm = this.formBuilder.group(
    {
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      passwordValidation: ['', [Validators.required, Validators.minLength(8)]],
    },
    { validators: passwordMatchValidator },
  );

  readonly errorMessage = signal<string | null>(null);

  handleSignUp(): void {
    if (this.signupForm.valid) {
      this.authService.signUp({
        firstName: this.signupForm.value.firstName!,
        lastName: this.signupForm.value.lastName!,
        email: this.signupForm.value.email!,
        password: this.signupForm.value.password!,
      });

      if (this.authService.signupError) {
        this.errorMessage.set(this.labels.errorSignUp);
        return;
      }

      this.errorMessage.set(null);
      this.router.navigate(['/signin']);
    }
  }

  loadLabels() {
    this.labels = {
      captionSignUp: i18n.t('captionSignUp', { ns: 'common' }),
      subTitleSignUp: i18n.t('subTitleSignUp', { ns: 'common' }),
      captionGetStarted: i18n.t('captionGetStarted', { ns: 'common' }),
      subTitleGetStarted: i18n.t('subTitleGetStarted', { ns: 'common' }),
      labelFirstName: i18n.t('labelFirstName', { ns: 'common' }),
      labelLastName: i18n.t('labelLastName', { ns: 'common' }),
      labelEmail: i18n.t('labelEmail', { ns: 'common' }),
      labelPassword: i18n.t('labelPassword', { ns: 'common' }),
      labelConfirmPassword: i18n.t('labelConfirmPassword', { ns: 'common' }),
      labelSignIn: i18n.t('labelSignIn', { ns: 'common' }),
      labelAlreadyHaveAccount: i18n.t('labelAlreadyHaveAccount', { ns: 'common' }),
      errorPasswordMismatch: i18n.t('errorPasswordMismatch', { ns: 'common' }),
      errorSignUp: i18n.t('errorSignUp', { ns: 'common' }),
    };
  }
}
