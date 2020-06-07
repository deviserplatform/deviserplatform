export interface PageContentTranslation {
    id: string;
    contentData: string;
    createdDate: Date | string | null;
    cultureCode: string;
    isActive: boolean;
    lastModifiedDate: Date | string | null;
    pageContentId: string;
}