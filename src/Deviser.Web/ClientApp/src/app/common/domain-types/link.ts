export interface Link {
    linkType: 'URL' | 'PAGE';
    linkText: string;
    isNewWindow: boolean;
    url: string;
    pageId: string;    
}