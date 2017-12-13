import { NgModule } from '@angular/core';
import { ThreadShareRoutingModule } from './threadShare-routing.module';
import { ThreadShareComponent } from './threadShare/threadShare.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
    imports: [
        ThreadShareRoutingModule,
        SharedModule
    ],
    declarations: [
        ThreadShareComponent
    ]
})

export class ThreadShareModule{

}