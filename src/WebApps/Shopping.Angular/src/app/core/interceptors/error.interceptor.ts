import { Injectable } from '@angular/core';
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from '../../shared/services/toastr.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private readonly toastrService: ToastrService) {}

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(req).pipe(
      catchError((error: unknown) => {
        if (error instanceof HttpErrorResponse) {
          const message = this.extractMessage(error);

          if (message) {
            this.toastrService.showDanger(message);
          }
        }

        return throwError(() => error);
      })
    );
  }

  private extractMessage(error: HttpErrorResponse): string {
    const { error: response, statusText } = error;

    if (!response) {
      return statusText || 'Ocorreu um erro inesperado.';
    }

    if (typeof response === 'string') {
      return response;
    }

    if (typeof response === 'object') {
      if (typeof response.message === 'string') {
        return response.message;
      }

      if (Array.isArray(response.errors)) {
        return response.errors.join(' ');
      }

      if (response.errors && typeof response.errors === 'object') {
        const values = Object.values(response.errors).flat();
        if (values.length > 0) {
          return values.join(' ');
        }
      }
    }

    return statusText || 'Ocorreu um erro inesperado.';
  }
}
