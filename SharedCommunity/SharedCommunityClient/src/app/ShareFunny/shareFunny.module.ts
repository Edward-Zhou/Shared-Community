import { NgModule } from '@angular/core';
import { ShareFunnyRoutingModule } from './shareFunny-routing.module';
import { ShareFunnyComponent } from './shareFunny/shareFunny.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
    imports: [
        ShareFunnyRoutingModule,
        SharedModule
    ],
    declarations: [
        ShareFunnyComponent
    ]
})

export class ShareFunnyModule{

}