import { Component, ViewChild} from "@angular/core";
import{  SharedModule } from '../../shared/shared.module';
import { MatTableDataSource, MatPaginator, MatSort } from "@angular/material";
import { ThreadService } from "../../services/thread.service";
import { Thread } from "../../model/thread.model";

@Component({
    selector:'threadShare',
    templateUrl:'threadShare.component.html',
})

export class ThreadShareComponent{
    //displayedColumns = ['Title', 'Creator', 'Created Time', 'Forum', "State", "Uri", "LastActiveOn"];
    displayedColumns = ['title']
    threads: Thread[];
    dataSource= new MatTableDataSource();
  
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    constructor(private threadService: ThreadService)
    {
      const that = this;
      this.threadService.getThreads().subscribe(
        threads => {
          that.threads = threads;
          that.dataSource = new MatTableDataSource(that.threads);
          that.dataSource.paginator = that.paginator;
          that.dataSource.sort = that.sort;
          //console.log(that.dataSource);
        }
      );     
    }
  
    /**
     * Set the paginator after the view init since this component will
     * be able to query its view for the initialized paginator.
     */
    ngAfterViewInit() {
      // this.dataSource.paginator = this.paginator;
      // this.dataSource.sort = this.sort;
    }
}
