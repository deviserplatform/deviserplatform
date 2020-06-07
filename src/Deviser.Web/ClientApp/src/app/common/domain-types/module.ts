import { ModuleView } from './module-view';

export interface Module {
    id: string;
    createdDate: Date | string | null;
    description: string;
    isActive: boolean;
    label: string;
    lastModifiedDate: Date | string | null;
    name: string;
    version: string;
    moduleView: ModuleView;
    //IsActiveText: string;
    isActiveBadgeClass: string;
}