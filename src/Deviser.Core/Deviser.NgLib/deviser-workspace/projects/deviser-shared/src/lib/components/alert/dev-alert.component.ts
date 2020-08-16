import { Component, OnInit, Input } from '@angular/core';
import { Alert } from '../../domain-types/alert';
import { AlertService } from '../../services/alert.service';

@Component({
  selector: 'dev-alert',
  templateUrl: './dev-alert.component.html',
  styleUrls: ['./dev-alert.component.scss']
})
export class DevAlertComponent implements OnInit {

  alerts: Alert[] = [];

  constructor(private _alertService: AlertService) {
    this._alertService.alerts.subscribe(alert => {
      if (alert) {
        this.alerts.push(alert)
      }
    });
  }

  ngOnInit(): void {
  }

}
