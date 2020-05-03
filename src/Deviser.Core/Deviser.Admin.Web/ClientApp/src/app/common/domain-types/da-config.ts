import { AdminConfigType } from './admin-confit-type';

export interface DAConfig {
    isEmbedded: boolean; //true for embedded in admin module, false for standalone
    debugBaseUrl: string;
    module: string;
    model: string;
    adminConfigType: AdminConfigType;
    assetPath: string;
}