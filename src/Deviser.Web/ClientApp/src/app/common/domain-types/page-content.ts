import { Property } from './property';
import { ContentType } from './content-type';
import { PageContentTranslation } from './page-content-translation';
import { ContentPermission } from './content-permission';

export interface PageContent {
    id: string;
    title?: string;
    containerId: string;
    properties: Property[];
    sortOrder: number;
    createdDate?: Date | string | null;
    isActive?: boolean;
    lastModifiedDate?: Date | string | null;
    pageId: string;
    contentTypeId: string;
    // export virtual Page Page { get; set; }
    contentType?: ContentType
    inheritViewPermissions?: boolean;
    inheritEditPermissions?: boolean;
    hasEditPermission: boolean;
    pageContentTranslation?: PageContentTranslation;
    contentPermissions?: ContentPermission;
}