export interface PageContentTranslation {
    id?: string;
    contentData: string | any;
    createdDate?: Date | string | null;
    cultureCode: string;
    isActive?: boolean;
    lastModifiedDate?: Date | string | null;
    pageContentId: string;
}