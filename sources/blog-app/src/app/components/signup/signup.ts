import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { AuthenticationService } from '../../services/AuthenticationService';
import { Textfield } from '../customComponents/textfield/textfield';
import { Formbutton } from '../customComponents/formbutton/formbutton';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signup',
  imports: [CommonModule, ReactiveFormsModule, Formbutton, Textfield],
  templateUrl: './signup.html',
  styleUrl: './signup.scss',
})
export class Signup {
  signupForm: FormGroup;
  errorMessage: string | null = null;
  authService = inject(AuthenticationService);
  route = inject(Router);

  constructor(private formBuilder: FormBuilder) {
    this.signupForm = this.formBuilder.group(
      {
        firstName: ['', [Validators.required]],
        lastName: ['', [Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required]],
        passwordValidation: ['', [Validators.required]],
      },
      { validators: passwordMatchValidator },
    );
  }

  handleSignUp() {
    if (this.signupForm.valid) {
      this.authService.signUp(this.signupForm.value);

      if (this.authService.signupError) {
        this.errorMessage = 'Error, please check your credentials.';
        return;
      } else {
        this.route.navigate(['/signin']);
      }
    }
  }
}

const passwordMatchValidator = (control: AbstractControl): ValidationErrors | null => {
  const password = control.get('password')?.value;
  const passwordValidation = control.get('passwordValidation')?.value;
  if (password !== passwordValidation) {
    return { passwordMismatch: true };
  }
  return null;
};
