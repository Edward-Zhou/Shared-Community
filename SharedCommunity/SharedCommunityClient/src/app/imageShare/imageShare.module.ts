import { NgModule } from '@angular/core';
import { ImageShareRoutingModule } from './imageShare-routing.module';
import { ImageShareComponent } from './imageShare/imageShare.component';
import { SharedModule } from '../shared/shared.module';
import { CardImageControl } from '../control/cardImage/cardImage.control';

@NgModule({
    imports: [
        ImageShareRoutingModule,
        SharedModule
    ],
    declarations: [
        ImageShareComponent,
        CardImageControl
    ]
})

export class ImageShareModule{

}