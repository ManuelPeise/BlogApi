import { inject, Injectable, signal } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { ISignInRequest } from '../lib/models/ISignInRequest';
import { LocalStorageService } from './LocalStorageService';
import { LocalStorageEnum } from '../lib/enums/LocalStorageEnum';
import { ITokenData } from '../lib/models/ITokenData';
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

  private localStorageService = new LocalStorageService<ITokenData>(LocalStorageEnum.AuthState);

  signIn(request: ISignInRequest) {
    this.http.post<IResponseModel<string>>(this.authUrl, request).subscribe(async (response) => {
      if (response.success) {
        const tokenData: ITokenData = { jwt: response.data };
        this.localStorageService.setItem(tokenData);
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

  getToken(): ITokenData | null {
    const tokenData = this.localStorageService.getItem();
    return tokenData;
  }

  signOut() {
    this.localStorageService.deleteItem();
    this.currentUserSignal.set(null);
  }
}
