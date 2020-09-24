export interface Language {
    Id: string;

    //Only used in Language Admin UI
    selectedLanguage: Language;
    cultureCode: string;
    englishName: string;
    nativeName: string;
    fallbackCulture: string;
    isActive: boolean;
    //IsActiveText: string;
    isActiveBadgeClass: string;
    createdDate: Date | string | null;
    lastModifiedDate: Date | string | null;
}