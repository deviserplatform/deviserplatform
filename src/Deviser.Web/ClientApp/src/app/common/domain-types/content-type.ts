import { Property } from './property';

export interface ContentType {
    id: string;
    name: string;
    label: string;
    iconImage: string;
    iconClass: string;
    sortOrder: number;
    properties: Property[];
    isActive: boolean;
    isActiveBadgeClass: string;
    createdDate: Date | string | null;
    lastModifiedDate: Date | string | null;
}