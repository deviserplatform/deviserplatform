import { ModuleView } from './module-view';
import { Property } from './property';
import { ModulePermission } from './module-permission';
import { Module } from './module';

export interface PageModule {
    id: string;
    title: string;
    containerId: string;
    isActive: boolean;
    moduleId: string;
    moduleViewId: string;
    sortOrder: number;
    pageId: string;
    moduleView: ModuleView;
    module: Module;
    // export virtual Page Page { get; set; }
    inheritViewPermissions: boolean;
    inheritEditPermissions: boolean;
    hasEditPermission: boolean;
    properties: Property[];
    modulePermissions: ModulePermission[];
}