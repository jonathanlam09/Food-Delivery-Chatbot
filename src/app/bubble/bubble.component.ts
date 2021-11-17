import { Component, OnInit } from '@angular/core';
import { CdkDrag, CdkDragEnd, CdkDragStart } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-bubble',
  templateUrl: './bubble.component.html',
  styleUrls: ['./bubble.component.css']
})
export class BubbleComponent implements OnInit {
  i : number = 0;
  elementPosition! : {x: number; y: number }

  constructor() { }

  ngOnInit(): void {
  }

  DragStart(event: CdkDragStart){
    var bubble = document.getElementById("bubble");
    var chatbot = document.getElementById("chatbot");
    bubble!.classList.add("padding0");
    //if function to check if chatbot is open when attempt to drag
    if (bubble!.classList.contains("chatbotopen")){
      bubble!.classList.remove("chatbotopen")
      bubble!.classList.add("draggingwhenopen")
      chatbot!.style.transform = "scale(0)";
      chatbot!.style.transitionDuration = "";
    }
  }

  DragEnd(event: CdkDragEnd){
    var bubble = document.getElementById("bubble");
    var chatbot = document.getElementById("chatbot");
    var bubblepos = bubble!.getBoundingClientRect();
    var bodypos = document.body.getBoundingClientRect();
    var total;

    //bubble 4 sides pos
    let bubble_top = bubblepos.top;
    let bubble_bot = bubblepos.bottom;
    let bubble_left = bubblepos.left;
    let bubble_right = bubblepos.right;

    //body 4 sides pos
    let body_top = bodypos.top;
    let body_bot = bodypos.bottom;
    let body_left = bodypos.left;
    let body_right = bodypos.right;

    //bubble 4 sides pos away from body in %
    let top_offset_percentage = (bubble_top/body_bot)*100;
    let bottom_offset_percentage = ((body_bot-bubble_bot)/body_bot)*100;
    let left_offset_percentage = (bubble_left/body_right)*100;
    let right_offset_percentage = ((body_right-bubble_right)/body_right)*100;
    chatbot!.style.top = "";
    chatbot!.style.bottom = "";
    chatbot!.style.left = "";
    chatbot!.style.right = "";

    //Top left body container
    //1 -
    //- - 
    if (top_offset_percentage < 50 && left_offset_percentage < 50){
      if(top_offset_percentage>left_offset_percentage){
        //Left offset = 0%, Top remain offset %
        chatbot!.style.left = "10%";
        this.elementPosition = {x: -1 * window.innerWidth + window.innerWidth * 0.06, y: bubble_top}
        //if function to readjust the chatbot overlapping
        if (top_offset_percentage >= 35){
          total = top_offset_percentage - 15;
          chatbot!.style.top = total.toString() + "%";
        }
        else{
          chatbot!.style.top = top_offset_percentage.toString() + "%";
        }
      }
      else{
        //Top offset = 0%, Left remain offset %
        chatbot!.style.top = "10%";
        chatbot!.style.left = left_offset_percentage.toString() + "%";
        this.elementPosition = {x: -1*window.innerWidth+bubble_right, y: 0}
      }
    }
    //Bottom left body container
    //- - 
    //1 -
    else if (bottom_offset_percentage < 50 && left_offset_percentage < 50 ){
      if(bottom_offset_percentage>left_offset_percentage){
        //Left offset = 0%, Bottom remain offset %
        chatbot!.style.left = "10%";
        //if function to readjust the chatbot overlapping
        if (bottom_offset_percentage > 35){
          total = bottom_offset_percentage - 15;
          chatbot!.style.bottom = total.toString() + "%";
        }
        else{
          total = bottom_offset_percentage + 5;
          chatbot!.style.bottom = total.toString() + "%";
          console.log("left = 0, bottom left side.");
        }
        this.elementPosition = {x: -1 * window.innerWidth + window.innerWidth * 0.06, y: bubble_top}
      }
      else{
        //Bottom offset = 0%, Left remain offset%
        chatbot!.style.bottom = "15%";
        chatbot!.style.left = left_offset_percentage.toString() + "%";
        this.elementPosition = {x: -1 * window.innerWidth + bubble_right, y: window.innerHeight - window.innerHeight * 0.08}
      }
    }
    //Top right body container
    //- 1
    //- -
    else if (top_offset_percentage < 50 && right_offset_percentage < 50 ){
      if(top_offset_percentage>right_offset_percentage){
        //Right offset = 0%, Top remain offset %
        chatbot!.style.right = "10%";
        //if function to readjust chatbot overlapping
        if ( top_offset_percentage > 35){
          total = top_offset_percentage - 15
          chatbot!.style.top = total.toString() + "%";
        }
        else{
          chatbot!.style.top = top_offset_percentage.toString() + "%";
        }
        this.elementPosition = {x: 0, y: bubble_top}
      }
      else{
        //Top offset = 0%, Right remain offset %
        chatbot!.style.top = "10%";
        chatbot!.style.right = right_offset_percentage.toString() + "%";
        this.elementPosition = {x: -1 * window.innerWidth + bubble_right, y: 0}
      }
    }
    //Bottom right body container
    //- -
    //- 1
    else if (bottom_offset_percentage < 50 && right_offset_percentage < 50 ){
      if(bottom_offset_percentage>right_offset_percentage){
        //Right offset = 0%, Bottom remain offset %
        chatbot!.style.right = "10%";
        //if function to readjust chatbot overlapping
        if ( bottom_offset_percentage >= 35){
          total= bottom_offset_percentage - 15;
          chatbot!.style.bottom = total.toString() + "%";
          this.elementPosition = {x: 0, y: bubble_bot}
        }
        else{
          total = bottom_offset_percentage + 5;
          chatbot!.style.bottom = total.toString() + "%";
          console.log("right = 0, bottom right side." + chatbot!.style.bottom);
        }
        this.elementPosition = {x: 0, y: bubble_top}
      }
      else{
        //Bottom offset = 0%, Right remain offset %
        chatbot!.style.bottom = "15%";
        chatbot!.style.right = right_offset_percentage.toString() + "%";
        this.elementPosition = {x: bubble_right - window.innerWidth, y: window.innerHeight - window.innerHeight * 0.08}
      }
    }
    //To reset this.i value as DragEnd triggers click function
    this.i = -1;
    //To check if chatbot is open while dragging, reopen when drag ends
    if (bubble!.classList.contains("draggingwhenopen")){
      chatbot!.classList.remove("draggingwhenopen")
      chatbot!.style.transform = "scale(1)";
      chatbot!.style.transitionDuration = "0.5s";
      chatbot!.style.paddingRight = "";
      this.i = -2
    }
  } 

  open(){
    var bubble = document.getElementById("bubble");
    var chatbot = document.getElementById("chatbot");
    this.i = this.i + 1;
    console.log(this.i)
    if (this.i % 2 == 0){
        bubble!.classList.remove("chatbotopen")
        chatbot!.style.transform = "scale(0)";
        chatbot!.style.transitionDuration = "0.5s";
        chatbot!.style.paddingRight = "";
    }
    else{
      if (bubble!.classList.contains("padding0")){
        bubble!.classList.add("chatbotopen");
        chatbot!.style.transform = "scale(1)";
        chatbot!.style.transitionDuration = "0.5s";
      }
      else{
        bubble!.classList.add("chatbotopen");
        chatbot!.style.transform = "scale(1)";
        chatbot!.style.transitionDuration = "0.5s";
        chatbot!.style.paddingRight = "10%"
      }
    }
  }
}
