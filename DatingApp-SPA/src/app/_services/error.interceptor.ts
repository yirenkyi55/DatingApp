import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpErrorResponse,
  HTTP_INTERCEPTORS
} from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(
    req: import('@angular/common/http').HttpRequest<any>,
    next: import('@angular/common/http').HttpHandler
  ): import('rxjs').Observable<import('@angular/common/http').HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((httpError: HttpErrorResponse) => {
        // Handle 401 unauthorize error
        if (httpError.status === 401) {
          return throwError(httpError.statusText);
        }

        // Internal server error has occurred
        if (httpError.status === 500) {
          return throwError(httpError.error.message);
        }

        // Handle unprocessable entity error
        if (httpError.status === 422) {
          const serverError = httpError.error;
          let modelStateError = '';
          if (serverError && typeof serverError === 'object') {
            for (const key in serverError) {
              if (serverError[key]) {
                modelStateError += serverError[key] + '\n';
              }
            }
          }

          return throwError(modelStateError);
        }

        // Handle 400 bad request
        if (httpError.status === 400) {
          const serverError = httpError.error;
          return throwError(serverError.message);
        }
      })
    );
  }
}

export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
};
