import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthenticationService } from '../../services/AuthenticationService';
import { Formbutton } from '../customComponents/formbutton/formbutton';
import { Textfield } from '../customComponents/textfield/textfield';

@Component({
  selector: 'app-signin',
  imports: [CommonModule, ReactiveFormsModule, Formbutton, Textfield],
  templateUrl: './signin.html',
  styleUrls: ['./signin.scss'],
})
export class Signin {
  authService = inject(AuthenticationService);
  signInForm: FormGroup;
  errorMessage: string | null = null;

  constructor(private formBuilder: FormBuilder) {
    this.signInForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }

  handleSignIn() {
    if (this.signInForm.valid) {
      this.authService.signIn(this.signInForm.value);

      if (this.authService.authenticationError) {
        this.errorMessage = 'Error, please check your credentials.';
        return;
      }
    }
  }
}
