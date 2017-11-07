import { Injectable, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { Http, Headers, Response, RequestOptionsArgs, RequestOptions } from '@angular/http';
import { tokenNotExpired, AuthHttp } from 'angular2-jwt';
import { Observable } from 'rxjs';
import 'rxjs/Rx';
import { APIUrl } from '../model/apiUrl.model';
import { UserInfo, UserLoginResponse } from '../model/user.model';
import { HttpClient } from "@angular/common/http";

@Injectable()

export class AuthService{
    public token: string;
    redirectUrl: string;
    requestArgs: RequestOptionsArgs;
    apiUrl: APIUrl = new APIUrl();
    public userUpdated: EventEmitter<boolean>;

    constructor(private http: HttpClient, private router:Router){
        this.userUpdated = new EventEmitter();
    }

    public accountLogin(account: string, password: string): Observable<boolean>{
        let formData = new FormData();
        formData.append('account', account);
        formData.append('password', password);
        return this.http.post<UserLoginResponse>(this.apiUrl.accountLogin(), formData)
               .map( 
                   response => {
                        let token = response.access_token;
                        localStorage.setItem('token', token);
                        return true;
                    },
                    error =>{
                        console.log('error');
                        return false;
                    }
                );
    }
    public isAccountlogin(){
        return tokenNotExpired('token');
    }
    public accountLogout():void {
        localStorage.removeItem('token');
        this.router.navigate(['/']);
    }

    public getUserInfo(): UserInfo{
        if(tokenNotExpired('token')){
            return JSON.parse(localStorage.getItem('currentUser')) as UserInfo;
        }
        return null;
    }
}