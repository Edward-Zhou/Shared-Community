import { Injectable } from "@angular/core";
import {HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpHeaders} from '@angular/common/http';
import { Observable } from "rxjs/Rx";

@Injectable()

export class RequestBasicUrlInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>{
        const url = 'https://localhost:44317/';
        var token = this.readCookie('XSRF-TOKEN');
        
        const headers = new HttpHeaders({  
            'X-XSRF-TOKEN': token          
        });
        req = req.clone({
            url: url + req.url,
            headers:headers,
            withCredentials: true            
        });
        return next.handle(req);
    }
    readCookie(name) {
        name += '=';
        for (var ca = document.cookie.split(/;\s*/), i = ca.length - 1; i >= 0; i--)
            if (!ca[i].indexOf(name))
                return ca[i].replace(name, '');
    }
}