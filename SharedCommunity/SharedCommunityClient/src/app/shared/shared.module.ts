import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpModule } from "@angular/http";
import { AuthService } from "../auth/auth.service";
import { CommonModule } from "@angular/common";
import { MaterialImportModule } from './materialImportModule';
import { LayoutComponent } from './layout/layout.component';
import { TopnavbarComponent } from './topnavbar/topnavbar.component';
import { SlideMenuComponent} from './slidemenu/slidemenu.component';
import { RouterModule } from "@angular/router";

@NgModule({
    imports: [
        RouterModule,
        FormsModule, 
        ReactiveFormsModule,
        HttpModule,
        CommonModule,
        MaterialImportModule
    ],
    declarations: [
        LayoutComponent,
        TopnavbarComponent,
        SlideMenuComponent
    ],
    exports: [
        RouterModule,
        FormsModule, 
        ReactiveFormsModule,
        HttpModule,
        CommonModule,
        MaterialImportModule,
        LayoutComponent,
        TopnavbarComponent,
        SlideMenuComponent
    ],
    providers: [
        AuthService
    ]
})

export class SharedModule{

}