import { Component } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";

@Component({
  selector: 'app-rabbit-mq-test',
  templateUrl: './rabbit-mq-test.component.html',
  styleUrls: ['./rabbit-mq-test.component.scss']
})
export class RabbitMQTestComponent {

  constructor(private httpClient: HttpClient){}

  sendRabbitMQOne(message: string) {
    this.httpClient.post('https://localhost:7777/sendToQueue', {message: message}).subscribe();
  }

}
