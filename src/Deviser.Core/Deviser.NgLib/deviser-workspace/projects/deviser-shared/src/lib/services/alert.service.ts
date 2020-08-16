import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Alert } from '../domain-types/alert';
import { AlertType } from '../domain-types/alert-type';

@Injectable({
  providedIn: 'root'
})
export class AlertService {

  private _alertSubject: BehaviorSubject<Alert> = new BehaviorSubject<Alert>(null);
  alerts: Observable<Alert> = this._alertSubject.asObservable();

  constructor() { }

  addAlert(alert: Alert) {
    this._alertSubject.next(alert);
  }

  showMessage(alertType: AlertType, message: string, timeout: number = 50000) {
    this._alertSubject.next({
      alertType,
      message,
      timeout
    });
  }
}
