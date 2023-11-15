import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatTabsModule } from '@angular/material/tabs';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SignalRTestComponent } from './Signlar-Test/signalr-test.component';
import { RabbitMQTestComponent } from './rabbit-mq-test/rabbit-mq-test.component';


@NgModule({
  declarations: [
    AppComponent,
    SignalRTestComponent,
    RabbitMQTestComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatTabsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
