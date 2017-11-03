import { Injectable, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { Http, Headers, Response, RequestOptionsArgs, RequestOptions } from '@angular/http';
import { tokenNotExpired, AuthHttp } from 'angular2-jwt';
import { Observable } from 'rxjs';
import { APIUrl } from '../model/apiUrl.model';
import { UserInfo } from '../model/user.model';

@Injectable()

export class AuthService{
    public token: string;
    redirectUrl: string;
    requestArgs: RequestOptionsArgs;
    apiUrl: APIUrl = new APIUrl();
    public userUpdated: EventEmitter<boolean>;

    constructor(private http: Http, private router:Router){
        this.userUpdated = new EventEmitter();
    }

    public accountLogin(account: string, password: string): Observable<boolean>{
        let formData = new FormData();
        formData.append('account', account);
        formData.append('password', password);
        return this.http.post(this.apiUrl.accountLogin(), formData)
            .map((response: Response) =>{
                let responseJson = response.json();
                let token = responseJson && responseJson.access_token;
                let userInfo = responseJson && responseJson.user_info as UserInfo
                if(token){
                    this.token = token;
                    localStorage.setItem('currentUser', JSON.stringify(userInfo));
                    localStorage.setItem('token', token);
                    return true;
                }
                else{
                    return false;
                }
            })
    }

    public accountLogout():void{
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