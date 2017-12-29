import { Component, ViewChild } from '@angular/core';
import { DataTable, DataTableResource } from '../../table/table-index';
import { cars } from './data-table-data';


@Component({
  selector: 'data-table-demo',
  templateUrl: './data-table.html'
})
export class DataTableDemo {

    yearLimit = 1999;
    carResource = new DataTableResource(cars);
    cars = [];
    carCount = 0;

    @ViewChild(DataTable) carsTable: DataTable;

    constructor() {
        this.rowColors = this.rowColors.bind(this);

        this.carResource.count().then(count => this.carCount = count);
    }

    reloadCars(params) {
        this.carResource.query(params).then(cars => this.cars = cars);
    }

    // custom features:

    carClicked(car) {
        alert(car.model);
    }


    rowColors(car) {
        if (car.year >= this.yearLimit) {return 'rgb(255, 255, 197)'; };
    }
}
