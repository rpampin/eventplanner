import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastService } from './toast.service';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

    constructor(public toastService: ToastService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request)
            .pipe(
                catchError((error: HttpErrorResponse) => {
                    debugger;
                    let errorMsg = '';
                    if (error.error instanceof ErrorEvent) {
                        console.log('this is client side error');
                        errorMsg = `Error: ${error.error.message}`;
                    }
                    else {
                        console.log('this is server side error');
                        errorMsg = `Error Code: ${error.status}, Message: ${error.message}, Error: ${error.error.split('\n')[0]}`;
                    }
                    console.log(errorMsg);
                    this.toastService.show(errorMsg, { classname: 'bg-danger text-light' });
                    return throwError(errorMsg);
                })
            )
    }
}