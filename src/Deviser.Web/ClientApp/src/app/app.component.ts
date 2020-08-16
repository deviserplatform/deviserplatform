import { Component } from '@angular/core';
import { Alert } from 'deviser-shared';
import { AlertService } from 'deviser-shared';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  alerts: Alert[] = [];
  title = 'ClientApp';

  constructor(private _alertService: AlertService){
    this._alertService.alerts.subscribe(alert => {
      if (alert) {
        this.alerts.push(alert)
      }
    });
  }
}
