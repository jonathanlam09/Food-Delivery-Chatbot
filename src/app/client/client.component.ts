import { Component, OnInit } from '@angular/core';
import { ClientService } from './client.service';
@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css']
})
export class ClientComponent implements OnInit {

  textInput: string = "";
  sockets: string[] = [];

  constructor(private clientService: ClientService) { 
    this.clientService.listen("toAdmin").subscribe((data:any)=>{
      this.sockets.push(data);
      console.log(data)
    })
  }

  ngOnInit(): void {
  }

  send(){
    this.clientService.emit("toAdmin", this.textInput);
    this.textInput = "";
  }

  enter(event: any){
    if (event.keyCode == 13){
      this.send();
    }
  }
}
