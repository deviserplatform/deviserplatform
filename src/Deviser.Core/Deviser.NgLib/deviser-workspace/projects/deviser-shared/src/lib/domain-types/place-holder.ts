import { Property } from './property';
import { PageContent } from './page-content';
import { ContentType } from './content-type';
import { ModuleView } from './module-view';
import { PageModule } from './page-module';

export interface PlaceHolder {
    id?: string;
    type: string;
    label?: string;
    layoutTemplate?: string;
    sortOrder?: number;

    contentType?: ContentType;
    moduleView?: ModuleView;
    placeHolderId?: string;

    title?: string;
    pageContent?: PageContent;
    pageModule?: PageModule;
    isUnassigned?: boolean;
    isPropertyChanged?: boolean;
    
    layoutTypeId?: string;
    properties?: Property[];
    placeHolders?: PlaceHolder[];
}