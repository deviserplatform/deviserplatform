import { ContentType } from './content-type';
import { Property } from './property';
import { ModuleView } from './module-view';

export interface PageElement {
    type: string;
    contentType?: ContentType;
    moduleView?: ModuleView;
    properties: Property[];
}