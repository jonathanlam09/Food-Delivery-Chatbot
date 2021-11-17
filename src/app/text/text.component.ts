import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-text',
  templateUrl: './text.component.html',
  styleUrls: ['./text.component.css']
})
export class TextComponent implements OnInit {

  @Input() text!: string;
  @Input() date!: any;
  @Input() owner!: boolean;
  @Input('colorBackRight') colorBackRight!: string;
  @Input('colorFontRight') colorFontRight!: string;
  @Input('colorBackLeft') colorBackLeft!: string;
  @Input('colorFontLeft') colorFontLeft!: string;
  
  constructor() { }

  ngOnInit(): void {
  }

}
