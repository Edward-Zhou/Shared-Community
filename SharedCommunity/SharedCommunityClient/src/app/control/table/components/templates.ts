//#region  HEADER_TEMPLATE
export const HEADER_TEMPLATE = `
<div class="data-table-header">
<h4 class="title" [textContent]="dataTable.headerTitle"></h4>
<div class="button-panel">
    <button type="button" class="btn btn-default btn-sm refresh-button"
        (click)="dataTable.reloadItems()" 
        [attr.aria-label]="dataTable.translations.headerReload" [title]="dataTable.translations.headerReload">
        <span class="glyphicon glyphicon-refresh"></span>
    </button>
    <button type="button" class="btn btn-default btn-sm column-selector-button" [class.active]="columnSelectorOpen"
        (click)="columnSelectorOpen = !columnSelectorOpen; $event.stopPropagation()" 
        [attr.aria-label]="dataTable.translations.headerColumnSelector" [title]="dataTable.translations.headerColumnSelector">
        <span class="glyphicon glyphicon-list"></span>
    </button>
    <div class="column-selector-wrapper" (click)="$event.stopPropagation()">
        <div *ngIf="columnSelectorOpen" class="column-selector-box panel panel-default">
            <div *ngIf="dataTable.expandableRows" class="column-selector-fixed-column checkbox">
                <label>
                    <input type="checkbox" [(ngModel)]="dataTable.expandColumnVisible"/>
                    <span>{{dataTable.translations.expandColumn}}</span>
                </label>
            </div>
            <div *ngIf="dataTable.indexColumn" class="column-selector-fixed-column checkbox">
                <label>
                    <input type="checkbox" [(ngModel)]="dataTable.indexColumnVisible"/>
                    <span>{{dataTable.translations.indexColumn}}</span>
                </label>
            </div>
            <div *ngIf="dataTable.selectColumn" class="column-selector-fixed-column checkbox">
                <label>
                    <input type="checkbox" [(ngModel)]="dataTable.selectColumnVisible"/>
                    <span>{{dataTable.translations.selectColumn}}</span>
                </label>
            </div>
            <div *ngFor="let column of dataTable.columns" class="column-selector-column checkbox">
                <label>
                    <input type="checkbox" [(ngModel)]="column.visible"/>
                    <span [textContent]="column.header"></span>
                </label>
            </div>
        </div>
    </div>
</div>
</div>
`;
//#endregion

//#region  ROW_TEMPLATE 
export const ROW_TEMPLATE = `
<tr	class="data-table-row"
    [title]="toolTip"
    [style.background-color]="dataTable.getRowColor(item, index, _this)"
    [class.row-odd]="index % 2 === 0"
    [class.row-even]="index % 2 === 1"
    [class.selected]="selected"
    [class.clickable]="dataTable.selectOnRowClick"
    (dblclick)="dataTable.rowDoubleClicked(_this, $event)"
    (click)="dataTable.rowClicked(_this, $event)"
    >
    <td [hide]="!dataTable.expandColumnVisible">
        <div tabindex="0" role="button" (click)="expanded = !expanded; $event.stopPropagation()" class="row-expand-button"
            [attr.aria-expanded]="expanded" [title]="dataTable.translations.expandRow" [attr.aria-label]="dataTable.translations.expandRow">
            <span class="glyphicon" [ngClass]="{'glyphicon-triangle-right': !expanded, 'glyphicon-triangle-bottom': expanded}" aria-hidden="true"></span>
        </div>
    </td>
    <td [hide]="!dataTable.indexColumnVisible" class="index-column" [textContent]="displayIndex"></td>
    <td [hide]="!dataTable.selectColumnVisible" class="select-column">
        <input type="checkbox" [(ngModel)]="selected"/>
    </td>
    <td *ngFor="let column of dataTable.columns" [hide]="!column.visible" [ngClass]="column.styleClassObject" class="data-column"
        [style.background-color]="column.getCellColor(_this, index)">
        <div *ngIf="!column.cellTemplate" [textContent]="item[column.property]"></div>
        <div *ngIf="column.cellTemplate" [ngTemplateOutlet]="column.cellTemplate" [ngTemplateOutletContext]="{column: column, row: _this, item: item}"></div>
    </td>
</tr>
<tr *ngIf="dataTable.expandableRows" [hide]="!expanded" class="row-expansion">
    <td [attr.colspan]="dataTable.columnCount">
        <div [ngTemplateOutlet]="dataTable.expandTemplate" [ngTemplateOutletContext]="{row: _this, item: item}"></div>
    </td>
</tr>
`;
//#endregion

//#region  TABLE_TEMPLATE  
export const TABLE_TEMPLATE = `
<div class="data-table-wrapper">
    <data-table-header *ngIf="header"></data-table-header>

    <div class="data-table-box">
        <table class="table table-condensed table-bordered data-table">
            <thead>
                <tr>
                    <th scope="col" [hide]="!expandColumnVisible" class="expand-column-header">
                    <th scope="col" [hide]="!indexColumnVisible" class="index-column-header">
                        <span [textContent]="indexColumnHeader"></span>
                    </th>
                    <th scope="col" [hide]="!selectColumnVisible" class="select-column-header">
                        <input [hide]="!multiSelect" type="checkbox" [(ngModel)]="selectAllCheckbox" [attr.aria-label]="translations.selectAllRows" />
                    </th>
                    <th scope="col" *ngFor="let column of columns" #th [hide]="!column.visible" 
                    	  (click)="headerClicked(column, $event)" 
                    	  (keydown.enter)="headerClicked(column, $event)" (keydown.space)="headerClicked(column, $event)"
                        [class.sortable]="column.sortable" [class.resizable]="column.resizable"
                        [ngClass]="column.styleClassObject" class="column-header" [style.width]="column.width | px"
                        [attr.aria-sort]="column.sortable ? (column.property === sortBy ? (sortAsc ? 'ascending' : 'descending') : 'none') : null"
                        [attr.tabindex]="column.sortable ? '0' : null">
                        <span *ngIf="!column.headerTemplate" [textContent]="column.header"></span>
                        <span *ngIf="column.headerTemplate" [ngTemplateOutlet]="column.headerTemplate" [ngTemplateOutletContext]="{column: column}"></span>
                        <span class="column-sort-icon" *ngIf="column.sortable">
                            <span class="glyphicon glyphicon-sort column-sortable-icon" [hide]="column.property === sortBy"></span>
                            <span [hide]="column.property !== sortBy">
                                 <span class="glyphicon" [ngClass]="{'glyphicon-triangle-top': !sortAsc, 'glyphicon-triangle-bottom': sortAsc}"></span>
                            </span>
                        </span>
                        <span *ngIf="column.resizable" class="column-resize-handle" (mousedown)="resizeColumnStart($event, column, th)"></span>
                    </th>
                </tr>
            </thead>
            <tbody *ngFor="let item of items; let index=index" class="data-table-row-wrapper"
                   dataTableRow #row [item]="item" [index]="index" (selectedChange)="onRowSelectChanged(row)">
            </tbody>
            <tbody *ngIf="itemCount === 0 && noDataMessage">
                <tr>
                    <td [attr.colspan]="columnCount">{{ noDataMessage }}</td>
                </tr>
            </tbody>
            <tbody class="substitute-rows" *ngIf="pagination && substituteRows">
                <tr *ngFor="let item of substituteItems, let index = index"
                    [class.row-odd]="(index + items.length) % 2 === 0"
                    [class.row-even]="(index + items.length) % 2 === 1"
                    >
                    <td [hide]="!expandColumnVisible"></td>
                    <td [hide]="!indexColumnVisible">&nbsp;</td>
                    <td [hide]="!selectColumnVisible"></td>
                    <td *ngFor="let column of columns" [hide]="!column.visible">
                </tr>
            </tbody>
        </table>
        <div class="loading-cover" *ngIf="showReloading && reloading"></div>
    </div>

    <data-table-pagination
        *ngIf="pagination"
        [show_range]="pagination_range"
        [show_limit]="pagination_limit"
        [show_input]="pagination_input"
        [show_numbers]="pagination_numbers"></data-table-pagination>
</div>
`;
//#endregion    

//#region PAGINATION_TEMPLATE
export const PAGINATION_TEMPLATE = `
<div class="pagination-box">
    <div class="pagination-range" *ngIf="show_range">
        {{dataTable.translations.paginationRange}}:
        <span [textContent]="dataTable.offset + 1"></span>
        -
        <span [textContent]="[dataTable.offset + dataTable.limit , dataTable.itemCount] | min"></span>
        /
        <span [textContent]="dataTable.itemCount"></span>
    </div>
    <div class="pagination-controllers">
        <div class="pagination-limit" *ngIf="show_limit">
            <div class="input-group">
                <span class="input-group-addon">{{dataTable.translations.paginationLimit}}:</span>
                <input #limitInput type="number" class="form-control" min="1" step="1"
                       [ngModel]="limit" (blur)="limit = limitInput.value"
                       (keyup.enter)="limit = limitInput.value" (keyup.esc)="limitInput.value = limit"/>
            </div>
        </div>
        <div class=" pagination-pages">
            <button [disabled]="dataTable.offset <= 0" (click)="pageFirst()" class="btn btn-default pagination-firstpage">&laquo;</button>
            <button [disabled]="dataTable.offset <= 0" (click)="pageBack()" class="btn btn-default pagination-prevpage">&lsaquo;</button>
            <div class="pagination-page" *ngIf="show_input">
                <div class="input-group">
                    <input #pageInput type="number" class="form-control" min="1" step="1" max="{{maxPage}}"
                           [ngModel]="page" (blur)="page = pageInput.value"
                           (keyup.enter)="page = pageInput.value" (keyup.esc)="pageInput.value = page"/>
                    <div class="input-group-addon">
                        <span>/</span>
                        <span [textContent]="dataTable.lastPage"></span>
                    </div>
                </div>
            </div>
            <button *ngIf="hasPrevious(maxPage,page)" [disabled]="true" (click)="false" class="btn btn-default hasPrevious">...</button>
            <div class="pagination-page" *ngIf="show_numbers">
                <button *ngFor="let i of createPageRange(maxPage,page)"
                    [disabled]="i == page"
                    (click)="page = i"
                    class="btn btn-default">{{ i }}</button>
            </div>
            <button *ngIf="hasNext(maxPage,page)" [disabled]="true" (click)="false" class="btn btn-default hasNext">...</button>
            <button [disabled]="(dataTable.offset + dataTable.limit) >= dataTable.itemCount" (click)="pageForward()" class="btn btn-default pagination-nextpage">&rsaquo;</button>
            <button [disabled]="(dataTable.offset + dataTable.limit) >= dataTable.itemCount" (click)="pageLast()" class="btn btn-default pagination-lastpage">&raquo;</button>
        </div>
    </div>
</div>
`;
//#endregion