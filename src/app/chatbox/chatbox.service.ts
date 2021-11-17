import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, Input } from '@angular/core';
import { environment } from 'src/environments/environment';
import { TextMessage } from '../Models/TextMessage';
import {io} from 'socket.io-client'
import { Observable, Subscriber } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatboxService {
  enable: boolean = false;
  socket: any;
  constructor(private http: HttpClient) {this.socket = io('ws://localhost:8000', { transports : ['websocket'] });}

  httpOptions = {
    headers: new HttpHeaders({ 
      'Access-Control-Allow-Origin':'*',
    })
  };
  
  listen(){
    return new Observable((subscriber)=>{
      this.socket.on("toAdmin", (client: any)=>{
        subscriber.next(client);
        // this.emit(client);
      })
    })
  }

  emit(client:string){
    this.socket.emit("toAdmin", client);
  }

  sendMessage(text: TextMessage){
    return this.http.post<any>(environment.backend.requestTextUrl, text, this.httpOptions)
  }
  getPrice(){
    return this.http.get<any>(environment.backend2.requestTextUrl)
  }
}
