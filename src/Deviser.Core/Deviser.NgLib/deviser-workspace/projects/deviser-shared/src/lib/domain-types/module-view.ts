import { Property } from './property';
import { ModuleViewType } from './module-view-type';

export interface ModuleView
    {
        id: string;
        actionName: string;
        controllerName: string;
        controllerNamespace: string;
        displayName: string;
        iconImage: string;
        iconClass: string;
        moduleViewTypeId: string;
        moduleId: string;
        isDefault: boolean;
        moduleViewType: ModuleViewType;
        properties: Property[];
    }