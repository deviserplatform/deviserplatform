export interface Alert {
    alterType: AlertType,
    message: string,
    timeout: number
}

export enum AlertType {
    Info = 'info',
    Success = 'success',
    Error = 'danger',
    Warning = 'warning'
} 