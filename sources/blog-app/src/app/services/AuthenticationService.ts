import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { ISignInRequest } from '../lib/models/ISignInRequest';
import { IResponseBase, IResponseModel } from '../lib/models/IResponse';
import { Router } from '@angular/router';
import { ISignupRequest } from '../lib/models/ISignupRequest';
import { IUserModel } from '../lib/models/IUserModel';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  authUrl = `${environment.apiBaseUrl}authentication/signin`;
  authenticationError: string | null = null;
  signupError: string | null = null;
  currentUserSignal = signal<IUserModel | undefined | null>(undefined);
  http = inject(HttpClient);
  router = inject(Router);

  signIn(request: ISignInRequest) {
    this.http.post<IResponseModel<string>>(this.authUrl, request).subscribe(async (response) => {
      if (response.success) {
        this.loadCurrentUser();
        this.router.navigate(['/']);
      } else {
        this.authenticationError = 'Error, please check your credentials.';
      }
    });
  }

  signUp(request: ISignupRequest) {
    this.http
      .post<IResponseBase>(`${environment.apiBaseUrl}authentication/signup`, request)
      .subscribe(async (response) => {
        if (response.success) {
          this.router.navigate(['/']);
        } else {
          this.signupError = 'Error while creating account.';
        }
      });
  }

  loadCurrentUser() {
    this.http
      .get<IUserModel>(`${environment.apiBaseUrl}user/getcurrentuser`)
      .subscribe((response) => {
        if (response) {
          this.currentUserSignal.set(response);
        }
      });
  }

  signOut() {
    this.currentUserSignal.set(null);
  }
}
