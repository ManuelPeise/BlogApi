import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { LoadingService } from './LoadingService ';
import { finalize } from 'rxjs';

let requests = 0; // shared across calls

export const LoadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);

  requests++;
  loadingService.show();

  return next(req).pipe(
    finalize(() => {
      requests--;

      if (requests === 0) {
        loadingService.hide();
      }
    }),
  );
};
