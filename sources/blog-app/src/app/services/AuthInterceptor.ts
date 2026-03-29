import { HttpInterceptorFn } from '@angular/common/http';
import { LocalStorageEnum } from '../lib/enums/LocalStorageEnum';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const tokenJson = localStorage.getItem(LocalStorageEnum.AuthState) ?? null;
  const token = tokenJson ? JSON.parse(tokenJson).jwt : null;

  if (token) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` },
    });
  }
  return next(req);
};
