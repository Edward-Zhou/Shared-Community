import { Injectable, Injector } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor} from '@angular/common/http';
import { AuthService } from '../auth/auth.service';
import { Observable } from 'rxjs/Observable';

@Injectable()

export class TokenInterceptor implements HttpInterceptor{
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>{
        request = request.clone({
            setHeaders: {
                Authorization: `Bearer ${ localStorage.getItem('token')}`
            }
        });

        return next.handle(request);
    }
}