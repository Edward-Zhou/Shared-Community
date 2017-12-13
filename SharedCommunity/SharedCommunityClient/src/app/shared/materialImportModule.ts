import { MatCardModule, MatIconModule} from '@angular/material';
import { MatTableModule } from '@angular/material/table';
import { NgModule} from '@angular/core';

@NgModule({
    imports:[
        MatCardModule,
        MatIconModule,
        MatTableModule],
    exports:[
        MatCardModule,
        MatIconModule,
        MatTableModule],
})

export class MaterialImportModule{}