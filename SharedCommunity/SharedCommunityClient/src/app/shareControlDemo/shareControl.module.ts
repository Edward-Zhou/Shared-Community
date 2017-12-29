import { NgModule } from '@angular/core';
import { ShareControlRoutingModule } from './shareControl-routing.module';
import { ShareControlComponent } from './shareControlDemo/shareControl.component';
import { SharedModule } from '../shared/shared.module';
import { DataTableDemo } from 'app/control/demos/table/data-table';

@NgModule({
    imports: [
        ShareControlRoutingModule,
        SharedModule
    ],
    declarations: [
        DataTableDemo,
        ShareControlComponent,
    ]
})

export class ShareControlModule{

}