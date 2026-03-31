import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthenticationService } from '../../services/AuthenticationService';
import { Formbutton } from '../customComponents/formbutton/formbutton';
import { Textfield } from '../customComponents/textfield/textfield';

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

  readonly signInForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  readonly errorMessage = signal<string | null>(null);

  handleSignIn(): void {
    if (this.signInForm.valid) {
      this.authService.signIn({
        email: this.signInForm.value.email!,
        password: this.signInForm.value.password!,
      });

      if (this.authService.authenticationError) {
        this.errorMessage.set('Error, please check your credentials.');
        return;
      }

      this.errorMessage.set(null);
    }
  }
}
