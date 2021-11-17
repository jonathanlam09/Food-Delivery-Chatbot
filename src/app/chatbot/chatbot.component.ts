import { Component, Input, OnInit } from '@angular/core';
import { ChatboxComponent } from '../chatbox/chatbox.component';

@Component({
  selector: 'app-chatbot',
  templateUrl: './chatbot.component.html',
  styleUrls: ['./chatbot.component.css']
})
export class ChatbotComponent implements OnInit {

  colorBackRight: string = '#6495ED';
  colorFontRight: string = '#ffffff';
  colorBackLeft: string = '#eeeeee';
  colorFontLeft: string = '#343a40';
  disabled: boolean = false;
  messages = [];
  

  constructor() { }

  ngOnInit(): void {
  }

  endchat(){
    var confirmation = "Are you sure you want to leave this session?"
    if (confirm(confirmation)){
      this.disabled = true;
    }
  }
}
