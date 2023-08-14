import { Component } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Super Website';
  status = 'Not connected!';

  connection: HubConnection | undefined;

  protected onConnect(): void{
    if (this.connection !== undefined){
      this.connection.stop();
    }   

    this.connection = new HubConnectionBuilder().withUrl("http://localhost:5254/hub", {
      skipNegotiation: true, 
      withCredentials: false, 
      transport: HttpTransportType.WebSockets}
      ).build();
    this.connection.start();
    console.log(this.connection);

    this.status = 'Connected!';
  }

  protected onDisconnect(): void{
    if (this.connection !== undefined){
      this.connection.stop();
    }    
    console.log(this.connection);

    this.status = 'Not connected!';
  }
}
