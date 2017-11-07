import { NgModule } from '@angular/core';
import { ImageShareRoutingModule } from './imageShare-routing.module';
import { ImageShareComponent } from './imageShare/imageShare.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
    imports: [
        ImageShareRoutingModule,
        SharedModule
    ],
    declarations: [
        ImageShareComponent
    ]
})

export class ImageShareModule{

}