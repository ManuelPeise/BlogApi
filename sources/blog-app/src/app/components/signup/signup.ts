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
import { passwordMatchValidator } from '../../lib/validation';

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
        password: ['', [Validators.required, Validators.minLength(8)]],
        passwordValidation: ['', [Validators.required, Validators.minLength(8)]],
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
