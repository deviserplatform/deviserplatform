import { Property } from './property';

export interface LayoutType {
    id: string;
    name: string; //old property - Type
    label: string;
    iconImage: string;
    iconClass: string;
    layoutTypeIds: string;
    properties: Property[];
    allowedLayoutTypes: LayoutType[];
    isActive: boolean;
    //IsActiveText: string;
    isActiveBadgeClass: string;
    createdDate: Date | string | null;
    lastModifiedDate: Date | string | null;
}