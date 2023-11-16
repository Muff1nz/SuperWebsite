import { Component } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";

@Component({
  selector: 'app-rabbit-mq-test',
  templateUrl: './rabbit-mq-test.component.html',
  styleUrls: ['./rabbit-mq-test.component.scss']
})
export class RabbitMQTestComponent {
  constructor(private httpClient: HttpClient){}

  sendRabbitMQAck(message: string) {
    this.httpClient.post('https://localhost:7777/sendToQueue1', {message: message}).subscribe();
  }

  sendRabbitMQNoAck(message: string) {
    this.httpClient.post('https://localhost:7777/sendToQueue2', {message: message}).subscribe();
  }

  sendRabbitMQVoid(message: string) {
    this.httpClient.post('https://localhost:7777/sendToVoid', {message: message}).subscribe();
  }

  sendRabbitMQTopic(topic: string, message: string) {
    this.httpClient.post('https://localhost:7777/sendToTopic', {topic: topic, message: message}).subscribe();
    }
}
