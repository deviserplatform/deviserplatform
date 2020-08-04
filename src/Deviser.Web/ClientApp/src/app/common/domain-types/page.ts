import { PageContent } from './page-content';
import { PageModule } from './page-module';
import { PageTranslation } from './page-translation';
import { PagePermission } from './page-permission';
import { Layout } from './layout';
import { PageType } from './page-type';
import { AdminPage } from './admin-page';
import { PageState } from './page-state';

export interface Page
    {
        id: string;
        createdDate: Date | string | null;
        endDate: Date | string | null;
        isActive: boolean;
        isSystem: boolean;
        isIncludedInMenu: boolean;
        lastModifiedDate: Date | string | null;
        layoutId: string | null;
        pageLevel: number | null;
        pageOrder: number | null;
        parentId: string | null;
        pageTypeId: string | null;
        themeSrc: string;
        startDate: Date | string | null;
        siteMapPriority: number;
        pageContent: PageContent[];
        pageModule: PageModule[];
        pageTranslation: PageTranslation[];
        pagePermissions: PagePermission[];
        layout: Layout;
        parent: Page;
        pageType: PageType;
        childPage: Page[];
        adminPage: AdminPage;
        //Non DB Properties
        isCurrentPage: boolean;
        isBreadCrumb: boolean;
    }