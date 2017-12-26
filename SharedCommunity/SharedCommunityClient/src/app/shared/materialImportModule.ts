import {MatSortModule,MatCheckboxModule,MatCardModule, MatIconModule,MatTableModule, MatPaginatorModule,MatTableDataSource, MatPaginator, MatInputModule} from '@angular/material';
import { NgModule} from '@angular/core';

@NgModule({
    imports:[
        MatCardModule,
        MatIconModule,
        MatTableModule,
        MatPaginatorModule,      
        MatInputModule,
        MatCheckboxModule,
        MatSortModule
    ],
    exports:[
        MatCardModule,
        MatIconModule,
        MatTableModule,
        MatPaginatorModule,   
        MatInputModule,
        MatCheckboxModule,
        MatSortModule
    ],
})

export class MaterialImportModule{}