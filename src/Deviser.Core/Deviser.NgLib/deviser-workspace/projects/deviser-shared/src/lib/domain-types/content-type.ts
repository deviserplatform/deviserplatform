import { Property } from './property';
import { ContentTypeField } from './content-type-field';

export interface ContentType {
    id: string;
    name: string;
    label: string;
    isList: boolean;
    iconImage: string;
    iconClass: string;
    sortOrder: number;
    properties: Property[];
    contentTypeFields: ContentTypeField[];
    isActive: boolean;
    isActiveBadgeClass: string;
    createdDate: Date | string | null;
    lastModifiedDate: Date | string | null;
}