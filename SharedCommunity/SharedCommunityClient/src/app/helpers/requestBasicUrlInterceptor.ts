import { Injectable } from "@angular/core";
import {HttpEvent, HttpInterceptor, HttpHandler, HttpRequest} from '@angular/common/http';
import { Observable } from "rxjs/Rx";

@Injectable()

export class RequestBasicUrlInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>{
        const url = 'https://localhost:44317/';
        req = req.clone({
            url: url + req.url
        });
        return next.handle(req);
    }
}