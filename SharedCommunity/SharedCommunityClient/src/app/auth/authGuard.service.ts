import { Injectable } from '@angular/core';
import { CanActivate, Router, CanActivateChild, CanLoad, Route, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable()
export class AuthGuard implements CanActivate, CanActivateChild, CanLoad{
    constructor(private authService: AuthService, private router: Router) { }

    canLoad(route: Route): boolean {
        let url: string = `/${route.path}`;
        return this.isLogin(url);
    }
    canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        return this.canActivate(childRoute, state);
    }
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        let url: string = state.url;
        return this.isLogin(url);
    }

    isLogin(url: string): boolean{
        if(this.authService.isAccountlogin()){ 
            return true;
        }
        alert('please log in below access security information');
        // Navigate to the login page with extras
        this.router.navigate(['/login']);
        return false;
    }
    
}