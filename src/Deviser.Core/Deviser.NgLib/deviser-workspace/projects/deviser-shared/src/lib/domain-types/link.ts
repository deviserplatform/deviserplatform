import { LinkType } from './link-type';

export interface Link {
    linkType: LinkType;
    linkText: string;
    isNewWindow: boolean;
    url: string;
    pageId: string;    
}