import { PageTranslation } from './page-translation';

export interface PageContext {
    assetPath: string; //Property available only for angular client!
    currentPageId: string;
    currentUrl: string;
    currentTranslation: PageTranslation;
    currentLocale: string;
    debugBaseUrl: string;
    hasPageViewPermission: boolean;
    hasPageEditPermission: boolean;
    isEmbedded: boolean; //Property available only for angular client!
    layoutId: string;
    homePageUrl: string;
    homePageFullUrl: string;
    isMultilingual: boolean;
    siteRoot: string;
    siteLanguage: string;
    selectedTheme: string;
}