import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Thread } from "../model/thread.model";
import { APIUrl } from "../model/apiUrl.model";
import { Observable } from 'rxjs/Observable';

@Injectable()
export class ThreadService {
  private apiUrl: APIUrl = new APIUrl();

  startTime: string;
  endTime: string;
  constructor(private http: HttpClient) {
    var date = new Date();
    var day = ("0" + date.getDate()).slice(-2);
    var month = ("0" +(date.getMonth()+1)).slice(-2);
    this.startTime = date.getFullYear() + "-" + month + "-" + "01";
    this.endTime = date.getFullYear() + "-" + month + "-" + day;
  }

  getThreads(startTime= this.startTime, endTime= this.endTime):Observable<Thread[]>{
    return this.http.get<Thread[]>(this.apiUrl.getThreads(startTime, endTime));
  }

}
