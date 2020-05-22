import { AdminConfigType } from './admin-confit-type';

export interface DAConfig {
    basePath: string;
    currentPageId: string;
    isEmbedded: boolean; //true for embedded in admin module, false for standalone
    debugBaseUrl: string;
    module: string;
    model: string;
    adminConfigType: AdminConfigType;
    assetPath: string;
}