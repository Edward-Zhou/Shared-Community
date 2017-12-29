//#region HEADER_STYLE
export const HEADER_STYLE = `
.data-table-header {
    min-height: 25px;
    margin-bottom: 10px;
}
.title {
    display: inline-block;
    margin: 5px 0 0 5px;
}
.button-panel {
    float: right;
}
.button-panel button {
    outline: none !important;
}

.column-selector-wrapper {
    position: relative;
}
.column-selector-box {
    box-shadow: 0 0 10px lightgray;
    width: 150px;
    padding: 10px;
    position: absolute;
    right: 0;
    top: 1px;
    z-index: 1060;
}
.column-selector-box .checkbox {
    margin-bottom: 4px;
}
.column-selector-fixed-column {
    font-style: italic;
}
`;
//#endregion

//#region ROW_STYLE
export const ROW_STYLE = `
.select-column {
    text-align: center;
}

.row-expand-button {
    cursor: pointer;
    text-align: center;
}

.clickable {
    cursor: pointer;
}
`;
//#endregion

//#region TABLE_STYLE
export const TABLE_STYLE = `
/* bootstrap override: */

:host /deep/ .data-table.table > tbody+tbody {
    border-top: none;
}
:host /deep/ .data-table.table td {
    vertical-align: middle;
}

:host /deep/ .data-table > thead > tr > th,
:host /deep/ .data-table > tbody > tr > td {
	overflow: hidden;
}

/* I can't use the bootstrap striped table, because of the expandable rows */
:host /deep/ .row-odd {
    background-color: #F6F6F6;
}
:host /deep/ .row-even {
}

.data-table .substitute-rows > tr:hover,
:host /deep/ .data-table .data-table-row:hover {
    background-color: #ECECEC;
}
/* table itself: */

.data-table {
    box-shadow: 0 0 15px rgb(236, 236, 236);
    table-layout: fixed;
}

/* header cells: */

.column-header {
    position: relative;
}
.expand-column-header {
	width: 50px;
}
.select-column-header {
	width: 50px;
	text-align: center;
}
.index-column-header {
	width: 40px;
}
.column-header.sortable {
    cursor: pointer;
}
.column-header .column-sort-icon {
	float: right;
}
.column-header.resizable .column-sort-icon {
    margin-right: 8px;
}
.column-header .column-sort-icon .column-sortable-icon {
    color: lightgray;
}
.column-header .column-resize-handle {
    position: absolute;
    top: 0;
    right: 0;
    margin: 0;
    padding: 0;
    width: 8px;
    height: 100%;
    cursor: col-resize;
}

/* cover: */

.data-table-box {
    position: relative;
}

.loading-cover {
   position: absolute;
   width: 100%;
   height: 100%;
   background-color: rgba(255, 255, 255, 0.3);
   top: 0;
}
`;
//#endregion    

//#region PAGINATION_STYLE
export const PAGINATION_STYLE = `
.pagination-box {
    position: relative;
    margin-top: -10px;
}
.pagination-range {
    margin-top: 7px;
    margin-left: 3px;
    display: inline-block;
}
.pagination-controllers {
    float: right;
}
.pagination-controllers input {
    min-width: 60px;
    /*padding: 1px 0px 0px 5px;*/
    text-align: right;
}

.pagination-limit {
    margin-right: 25px;
    display: inline-table;
    width: 150px;
}
.pagination-pages {
    display: inline-block;
}
.pagination-page {
    width: 110px;
    display: inline-table;
}
.pagination-box button {
    outline: none !important;
}
.pagination-prevpage,
.pagination-nextpage,
.pagination-firstpage,
.pagination-lastpage {
    vertical-align: top;
}
.pagination-reload {
    color: gray;
    font-size: 12px;
}
`;
//#endregion  
