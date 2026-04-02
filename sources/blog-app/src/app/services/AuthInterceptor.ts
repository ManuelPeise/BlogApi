import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);

  return next(req.clone({ withCredentials: true })).pipe(
    tap({
      error: (err) => {
        if (err instanceof HttpErrorResponse && err.status === 403) {
          // router.navigate(['/signin']);
        }
      },
    }),
  );
};
