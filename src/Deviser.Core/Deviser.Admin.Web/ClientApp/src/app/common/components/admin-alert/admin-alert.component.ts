import { Component, OnInit, Input } from '@angular/core';
import { AlertComponent } from 'ngx-bootstrap/alert/alert.component';

import { Alert } from '../../domain-types/alert';

@Component({
  selector: 'app-admin-alert',
  templateUrl: './admin-alert.component.html',
  styleUrls: ['./admin-alert.component.scss']
})
export class AdminAlertComponent implements OnInit {

  @Input() alerts: Alert[];

  constructor() { }

  ngOnInit() {
  }

  // onClosed(dismissedAlert: any): void {
  //   this.alerts = this.alerts.filter(alert => alert !== dismissedAlert);
  // }

}
