// src/app/core/services/signalr.service.ts
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private hubConnection: signalR.HubConnection;
  
  public capacityUpdate$ = new Subject<{ sessionId: string, newCapacity: number }>();

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5000/hubs/scheduling')
      .withAutomaticReconnect()
      .build();
  }

  public startConnection() {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) return;

    this.hubConnection
      .start()
      .then(() => console.log('SignalR Connected'))
      .catch(err => console.error('Error while starting connection: ' + err));

    this.hubConnection.on('ReceiveCapacityUpdate', (sessionId: string, newCapacity: number) => {
      console.log(`SignalR Update: Session ${sessionId} -> ${newCapacity}`);
      this.capacityUpdate$.next({ sessionId, newCapacity });
    });
  }
}