import { DataTableRow } from "./row.component";
import { DataTableColumn } from "./column.component";

//#region   DataTableParams
export interface DataTableParams {
    offset?: number;
    limit?: number;
    sortBy?: string;
    sortAsc?: boolean;
  }
//#endregion

//#region RowCallback
export type RowCallback = (item: any, row: DataTableRow, index: number) => string;
//#endregion

//#region CellCallback
export type CellCallback = (item: any, row: DataTableRow, column: DataTableColumn, index: number) => string;
//#endregion

//#region   DataTableTranslations
export interface DataTableTranslations {
	headerReload: string,
	headerColumnSelector: string,
	indexColumn: string;
	selectColumn: string;
	selectRow: string;
	selectAllRows: string,
	expandColumn: string;
	expandRow: string;
	paginationLimit: string;
	paginationRange: string;
}
//#endregion

//#region defaultTranslations
export const defaultTranslations: DataTableTranslations = {
	headerReload: 'reload',
	headerColumnSelector: 'column selector',
	indexColumn: 'index',
	selectColumn: 'select',
	selectRow: 'select',
	selectAllRows: 'select',
	expandColumn: 'expand',
	expandRow: 'expand',
	paginationLimit: 'Limit',
	paginationRange: 'Results'
};
//#endregion


