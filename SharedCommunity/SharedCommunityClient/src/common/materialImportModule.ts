import { MatCardModule, MatIconModule, MatPaginator, MatTableDataSource} from '@angular/material';
import { NgModule} from '@angular/core';

@NgModule({
    imports:[
        MatCardModule,
        MatIconModule,
        MatPaginator, 
        MatTableDataSource],
    declarations:[

        ],
    exports:[
        MatCardModule,
        MatIconModule,
        MatPaginator, 
        MatTableDataSource],
})

export class MaterialImportModule{}