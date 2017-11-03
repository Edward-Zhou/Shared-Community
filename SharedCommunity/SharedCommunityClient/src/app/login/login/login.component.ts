import { Component, OnInit } from '@angular/core';
import { Router, NavigationExtras } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from  '@angular/forms';
import { AuthService } from '../../auth/auth.service';

@Component({
    selector: 'login-Form',
    templateUrl: './login.component.html'
})

export class LoginComponent implements OnInit{
    isLogin: boolean = false;
    error: boolean = false;
    loginForm: FormGroup;
    constructor(private fb: FormBuilder, private authSerivce: AuthService, private router: Router){
        this.loginForm = this.fb.group({
            account: ["", Validators.required],
            password: ["", Validators.required]
        });
    }

    ngOnInit(): void {
        
    }

    accountLogin(event): void{
        this.authSerivce.accountLogin(this.loginForm.get('account').value, this.loginForm.get('password').value)
            .subscribe(successed => {
                if(successed){
                    this.router.navigate(['/']);
                    
                }
            });
    }

}