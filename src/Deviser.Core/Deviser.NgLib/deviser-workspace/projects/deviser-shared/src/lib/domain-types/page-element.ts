import { ContentType } from './content-type';
import { Property } from './property';
import { ModuleView } from './module-view';
import { PageContent } from './page-content';
import { PageModule } from './page-module';

export interface PageElement {
    contentType?: ContentType;    
    moduleView?: ModuleView;
    id?: string;
    pageContent?: PageContent;
    pageModule?: PageModule;
    properties: Property[];
    sortOrder?: number;
    title?: string;
    type: string;
}