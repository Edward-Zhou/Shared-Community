import { Component } from '@angular/core';
import { AuthService, emitter } from '../../auth/auth.service'
import { Router,NavigationExtras } from '@angular/router';
import { UserInfo } from "../../model/user.model";
declare var jQuery: any;

@Component({
    selector: 'hearderNav',
    templateUrl: 'topnavbar.template.html'
})
export class TopnavbarComponent {
    userInfo: UserInfo;
    constructor(public authSvc: AuthService, private router: Router ) {
        this.userInfo = authSvc.userInfo;
    }
    toggleNavigation(): void {
        jQuery("body").toggleClass("mini-navbar");
    }

    logout(): void {
        this.authSvc.accountLogout();
        // Set our navigation extras object
        // that passes on our global query params and fragment
        let navigationExtras: NavigationExtras = {
            preserveQueryParams: true,
            preserveFragment: true
        };
        this.router.navigate(['/login']);
    }
}