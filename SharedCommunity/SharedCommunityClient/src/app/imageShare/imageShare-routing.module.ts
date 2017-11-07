import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ImageShareComponent } from './imageShare/imageShare.component';
import { AuthGuard } from "app/auth/authGuard.service";

const routes: Routes = [
    { 
      path: 'imageshare', 
      component : ImageShareComponent, 
      canActivate: [AuthGuard] 
    }
]

@NgModule({
    imports:[
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})

export class ImageShareRoutingModule{}