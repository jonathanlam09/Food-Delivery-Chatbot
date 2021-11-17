import { Component, Input, OnInit } from '@angular/core';
import { Text } from '../Models/Text';
import { TextMessage } from '../Models/TextMessage';
import { ChatboxService } from './chatbox.service';
import {Response} from '../Models/Response';
import { waitForAsync } from '@angular/core/testing';

@Component({
  selector: 'app-chatbox',
  templateUrl: './chatbox.component.html',
  styleUrls: ['./chatbox.component.css']
})
export class ChatboxComponent implements OnInit {
  @Input('lists') lists: string[] = [];
  @Input('messages') messages!: Text[];
  @Input('colorBackRight') colorBackRight!: string;
  @Input('colorFontRight') colorFontRight!: string;
  @Input('colorBackLeft') colorBackLeft!: string;
  @Input('colorFontLeft') colorFontLeft!: string;
  @Input('disabled') disabled!: boolean;
  BACK_ENABLED: boolean = true;
  visible: boolean = false;
  checked: boolean[] = [];
  pricelist : string[] = [];
  textInput: string = "";
  i: number = 0;
  name: string = "Jonathan";

  constructor(private chatboxService: ChatboxService) {
    this.chatboxService.listen().subscribe((client: any)=>{
      let socket_server : Text = {text: client, userOwner: false, visible: false};
      this.messages.push(socket_server);
      console.log(client);  
    })
  }

  //Run when document ready
  ngOnInit() {
    if (this.BACK_ENABLED){
      this.chatboxService.getPrice().subscribe((res: Response) => {
        for ( var i = 0; i < res.result.length; i++){
          this.pricelist.push(res.result[i]);
        }
      })
    }
    let TextInput = "Greetings! My name is Dorcas and I am here at your service to satisfy to your food cravings. What do you fancy to have today?";
    let messageReturn: Text = {text: TextInput, userOwner: false, visible: false}
    this.messages.push(messageReturn);
    let TextInput2 = "If you would like to start order, please type ORDER.";
    let messageReturn2: Text = {text: TextInput2, userOwner: false, visible: false}
    this.messages.push(messageReturn2);
    let TextInput3 = "If you wish to speak to our live agent, please type LIVECHAT.";
    let messageReturn3: Text = {text: TextInput3, userOwner: false, visible: false}
    this.messages.push(messageReturn3);
    let TextInput4 = "If you wish to end this session, please click the red icon located at the top right side of the chatbot..";
    let messageReturn4: Text = {text: TextInput4, userOwner: false, visible: false}
    this.messages.push(messageReturn4);
  }

  //Send chatbot Input Text to backend
  sendMessage() {
    let TextInput = this.textInput.replace("\n","");
    let newMessage: Text = {text: TextInput, userOwner:true, visible: false};
    this.messages.push(newMessage);
    console.log("//")
    console.log(newMessage.text)
    console.log("//")

    
    let messageBack: TextMessage = {text: TextInput};

    if (TextInput.toUpperCase() == "LIVECHAT"){
      this.BACK_ENABLED = false;
    }
    if (this.BACK_ENABLED == true) {
        this.chatboxService.sendMessage(messageBack)
        .subscribe((res: Response) => {
          console.log(res);
          if (TextInput.toUpperCase() == "ORDER"){
            this.visible = true;
            console.log(res.result)
            this.messages.push({text: "Please tick the item desire", userOwner: false, visible: this.visible});
            for (var i = 0; i <res.result.length; i++){
              this.lists.push(res.result[i])
            }   
          }
          else{
            this.visible = false;
            let messageReturn: Text = {text: res.result.toString().split(",").join(""), userOwner: false, visible: this.visible}
            this.messages.push(messageReturn);
            console.log("//");
            console.log(messageReturn.text);
            console.log("//");
            // if (this.lists.length == 0 ){
            //   this.lists.push(res.result)
            //   console.log(this.lists);
            //   console.log(this.lists.length);
            // }
          }
      });
    }
    else {
      if (TextInput.toUpperCase()=="LIVECHAT"){
        this.chatboxService.emit(this.name + " has joined the chat room.")
        setTimeout(()=>{
          let promptMsg1 : Text = {text: "Hi there, I am Jonathan and I am a live agent. How many I assist you today?", userOwner:false, visible: false};
          this.messages.push(promptMsg1);
          let promptMsg2 : Text = {text: "If you wish to disconnect from this session, please type END.", userOwner:false, visible: false};
          this.messages.push(promptMsg2);
        },3000)
      }
      else if(TextInput.toUpperCase()=="END"){
        this.BACK_ENABLED = true;
        let promptMsg : Text = {text: "You are no longer connected to our live agent.", userOwner:false, visible: false};
        setTimeout(() => {
          this.messages.push(promptMsg);
        }, 1000);
      }
      else{
        this.chatboxService.emit(TextInput);
      }
    }
    this.textInput = '';  
  }

  //Trigger sendMessage function on enterkey
  onKey(event: any) {
    if (event.keyCode == 13) {
      this.sendMessage()
    }
  }

  order(): void{
    var totalcheckbox = document.querySelectorAll(".checkbox");
    var checkboxlength = totalcheckbox.length;
    var string = "";
    var price;
    var index = 1;
    var totalprice = 0;
    var menulisting = "";
    var totalprice_string = "";
    var routing = "You will be redirected to payment page. Please hold on.";
    for (var i = 0; i < checkboxlength; i++){
      var checkbox = document.getElementById("checkboxtext" + i + 1);
      if (this.checked[i] == true){
        string = string + index + ")" + this.lists[i] + "\n";
        price = parseInt(this.pricelist[i]);
        menulisting = menulisting + index + ")"+ this.lists[i] + "\nRM" + price + "\n\n";
        totalprice = totalprice + price;
        index = index + 1;
      }
    }
    totalprice_string = menulisting + "\n"+"The total price is RM" + totalprice.toString() + ".\nWould you like to proceed with payment?";
    //If confirm food name
    if (confirm(string)){
      //If confirm total price
      if (confirm(totalprice_string)){
        //If confirm to be redirected to payment page
        if (confirm(routing)){
          //Payment Page
          window.location.href = "https://www.maybank2u.com.my/home/m2u/common/login.do";
        }
        //cancel order
        else{
          window.alert("Please visit again.");
        }
      }
      //cancel order
      else{
        window.alert("Please visit again.");
      }
    }
    //Cancel Order
    else
    {
      window.alert("Please visit again.");
    }
  }

  endchat(){
    window.alert("ha");
    this.BACK_ENABLED = false;
    let promptMsg1 : Text = {text: "You are now disconnected from our live agent.", userOwner:false, visible: false};
    setTimeout(()=>{
      this.messages.push(promptMsg1)
    }, 3000);
  }
}
