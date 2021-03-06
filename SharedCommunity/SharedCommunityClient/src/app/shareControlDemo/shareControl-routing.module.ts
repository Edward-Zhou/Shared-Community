import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from '../shared/layout/layout.component'
import { ShareControlComponent } from './shareControlDemo/shareControl.component';
import { AuthGuard } from "app/auth/authGuard.service";

const routes: Routes = [
    { 
      path: '', 
      component : LayoutComponent, 
      children:[{
          path: "sharecontrol",
          component: ShareControlComponent,
          canActivate: [AuthGuard]
        }]
    }
]

@NgModule({
    imports:[
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule]
})

export class ShareControlRoutingModule{}