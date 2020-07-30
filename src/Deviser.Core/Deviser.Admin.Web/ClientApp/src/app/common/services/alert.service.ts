import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Alert, AlertType } from '../domain-types/alert';

@Injectable({
  providedIn: 'root'
})
export class AlertService {

  private _alertSubject: BehaviorSubject<Alert> = new BehaviorSubject<Alert>(null);
  alerts: Observable<Alert> = this._alertSubject.asObservable();

  constructor() { 
    
  }

  addAlert(alert: Alert) {
    this._alertSubject.next(alert);
  }

  showMessage(alertType: AlertType, message: string, timeout: number = 5000) {
    this._alertSubject.next({
      alertType,
      message,
      timeout
    });
  }
}
