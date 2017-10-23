import { MatCardModule, MatIconModule} from '@angular/material';
import { NgModule} from '@angular/core';

@NgModule({
    imports:[
        MatCardModule,
        MatIconModule],
    exports:[
        MatCardModule,
        MatIconModule],
})

export class MaterialImportModule{}