import { AbstractControl, ValidationErrors } from '@angular/forms';

export const passwordMatchValidator = (control: AbstractControl): ValidationErrors | null => {
  const password = control.get('password')?.value;
  const confirmPassword = control.get('confirmPassword')?.value;
  if (password !== confirmPassword) {
    return { passwordMismatch: true };
  }
  return null;
};

export const updatePasswordMatchValidator = (control: AbstractControl): ValidationErrors | null => {
  const password = control.get('currentPassword')?.value;
  const newPassword = control.get('newPassword')?.value;
  const confirmPassword = control.get('confirmPassword')?.value;
  if (
    password === newPassword ||
    newPassword.length < 8 ||
    confirmPassword.length < 8 ||
    newPassword !== confirmPassword
  ) {
    return { passwordMismatch: true };
  }
  return null;
};
