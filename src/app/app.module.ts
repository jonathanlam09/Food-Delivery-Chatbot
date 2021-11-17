import { Injector, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TextComponent } from './text/text.component';
import { ChatboxComponent } from './chatbox/chatbox.component';
import { ChatbotComponent } from './chatbot/chatbot.component';
import { BubbleComponent } from './bubble/bubble.component';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ChatboxService } from './chatbox/chatbox.service';
import { DragDropModule } from '@angular/cdk/drag-drop'
import {createCustomElement} from '@angular/elements';

@NgModule({
  declarations: [
    AppComponent,
    TextComponent,
    ChatboxComponent,
    ChatbotComponent,
    BubbleComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    CommonModule,
    DragDropModule
  ],
  providers: [ChatboxService],
  bootstrap: [AppComponent]
})
export class AppModule {

  constructor(private injector: Injector) {
  }

  ngDoBootstrap() {
    const appElement = createCustomElement(AppComponent, {injector : this.injector});
    customElements.define('app-notacode', appElement)
  }
}
