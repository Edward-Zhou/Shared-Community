import { Component, Input, Inject, forwardRef, Output, EventEmitter, OnDestroy } from "@angular/core";
import { DataTable } from "./table.component";
import { ROW_TEMPLATE } from "./templates";
import { ROW_STYLE } from "./styles";

@Component({
    moduleId: module.id,
    selector: '[dataTableRow]',
    template: ROW_TEMPLATE,
    styles: [ROW_STYLE]
})

export class DataTableRow implements OnDestroy{

    @Input() item: any;
    @Input() index: number;

    expanded: boolean;

    //#region row selection:
    private _selected: boolean;

    @Output() selectedChange = new EventEmitter();

    get selected(){
        return this._selected;
    }

    set selected(selected){
        this._selected = selected;
        this.selectedChange.emit(selected);
    }
    //#endregion
    
    get displayIndex(){
        if(this.dataTable.pagination){
            return this.dataTable.displayParams.offset + this.index + 1;
        } else {
            return this.index + 1;
        }
    }

    get toolTip(){
        if (this.dataTable.rowTooltip) {
            return this.dataTable.rowTooltip(this.item, this, this.index);
        }
        return '';
    }

    constructor(@Inject(forwardRef(() => DataTable)) public dataTable: DataTable) {}

    ngOnDestroy() {
        this.selected = false;
    }
}