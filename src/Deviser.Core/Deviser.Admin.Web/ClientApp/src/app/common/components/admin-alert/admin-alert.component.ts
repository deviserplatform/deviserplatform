import { Component, OnInit, Input } from '@angular/core';
import { AlertComponent } from 'ngx-bootstrap/alert/alert.component';

import { Alert } from '../../domain-types/alert';
import { AlertService } from '../../services/alert.service';

@Component({
  selector: 'app-admin-alert',
  templateUrl: './admin-alert.component.html',
  styleUrls: ['./admin-alert.component.scss']
})
export class AdminAlertComponent implements OnInit {

  alerts: Alert[] = [];

  constructor(private _alertService: AlertService) {
    this._alertService.alerts.subscribe(alert => {
      if (alert) {
        this.alerts.push(alert)
      }
    });
  }

  ngOnInit() {
  }

  // onClosed(dismissedAlert: any): void {
  //   this.alerts = this.alerts.filter(alert => alert !== dismissedAlert);
  // }

}
