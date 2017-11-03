import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { MainComponent} from './main/main.component';
import { AppComponent } from "./app.component";
import { LoginModule } from "./login/login.module";

@NgModule({
    declarations: [
        MainComponent,
        AppComponent
    ],
    imports:[
        BrowserModule,        
        LoginModule,
        AppRoutingModule        
    ],
    bootstrap: [MainComponent]
})
export class AppModule{}