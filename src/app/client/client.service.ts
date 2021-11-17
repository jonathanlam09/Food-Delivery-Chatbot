import { Injectable } from '@angular/core';
import { io } from 'socket.io-client';
import {HttpClient, HttpHeaders} from '@angular/common/http'
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClientService {
  socket: any;
  constructor(private http: HttpClient) {
    this.socket = io('ws://localhost:8000', { transports : ['websocket'] });
   }

  httpOptions = {
    headers: new HttpHeaders({ 
      'Access-Control-Allow-Origin':'*',
    })
  };
  
  listen(eventName: string){
    return new Observable((subscriber)=>{
      this.socket.on(eventName, (wstext: any)=>{
        subscriber.next(wstext);
      })
    })
  }

  emit(eventName: string, wstext: any){
    this.socket.emit(eventName, wstext);
  }
}
