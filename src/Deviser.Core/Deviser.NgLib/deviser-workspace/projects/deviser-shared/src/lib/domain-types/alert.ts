import { AlertType } from './alert-type';

export interface Alert {
    alertType: AlertType,
    message: string,
    timeout: number
}