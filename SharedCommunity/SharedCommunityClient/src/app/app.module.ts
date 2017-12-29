import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { MainComponent} from './main/main.component';
import { AppComponent } from "./app.component";
import { LoginModule } from "./login/login.module";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { RequestBasicUrlInterceptor } from "./helpers/requestBasicUrlInterceptor";
import { UrlSerializer } from "@angular/router";
import { LowerCaseUrlSerializer } from "./helpers/lowerCaseUrlSerializer";
import { ImageShareModule } from "./imageShare/imageShare.module";
import { AuthGuard } from "./auth/authGuard.service";
import { TokenInterceptor } from "./helpers/tokenInterceptor";
import { ThreadShareModule } from 'app/Thread/threadShare.module';
import { ShareFunnyModule } from 'app/ShareFunny/shareFunny.module';
import { ShareControlModule } from "app/shareControlDemo/shareControl.module";

@NgModule({
    declarations: [
        MainComponent,
        AppComponent
    ],
    imports:[
        HttpClientModule,
        BrowserModule,  
        AppRoutingModule,      
        LoginModule,
        ImageShareModule,
        ThreadShareModule,
        ShareFunnyModule,
        ShareControlModule              
    ],
    providers: [
        { 
            provide: HTTP_INTERCEPTORS, 
            useClass: RequestBasicUrlInterceptor, 
            multi: true },
        { 
            provide: HTTP_INTERCEPTORS, 
            useClass: TokenInterceptor, 
            multi: true },
        { 
            provide: UrlSerializer, 
            useClass: LowerCaseUrlSerializer
        },
        [AuthGuard]
    ],
    bootstrap: [MainComponent]
})
export class AppModule{}