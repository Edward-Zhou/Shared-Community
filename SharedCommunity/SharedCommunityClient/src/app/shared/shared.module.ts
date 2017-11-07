import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpModule } from "@angular/http";
import { AuthService } from "../auth/auth.service";
import { CommonModule } from "@angular/common";

@NgModule({
    imports: [
        FormsModule, 
        ReactiveFormsModule,
        HttpModule,
        CommonModule
    ],
    declarations: [

    ],
    exports: [
        FormsModule, 
        ReactiveFormsModule,
        HttpModule,
        CommonModule
    ],
    providers: [
        AuthService
    ]
})

export class SharedModule{

}