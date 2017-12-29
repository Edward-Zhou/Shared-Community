import { Component, Inject, forwardRef, Input } from "@angular/core";
import { DataTable } from "./table.component";
import { PAGINATION_TEMPLATE } from "./templates";
import { PAGINATION_STYLE } from "./styles";

@Component({
    moduleId: module.id,
    selector: 'data-table-pagination',
    template: PAGINATION_TEMPLATE,
    styles: [PAGINATION_STYLE]
})

export class DataTablePagination{
    @Input() show_range = false;
    @Input() show_limit = false;
    @Input() show_input = false;
    @Input() show_numbers = true;

    constructor(@Inject(forwardRef(() => DataTable)) public dataTable: DataTable){}

    pageBack(){
        this.dataTable.offset -= Math.min(this.dataTable.limit, this.dataTable.offset);
    }
}