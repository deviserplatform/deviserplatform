import { PlaceHolder } from './place-holder';

export interface PageLayout {
    id: string;
    name: string;
    placeHolders: PlaceHolder[];
    pageId: string;
    isChanged: boolean;
    isActive: boolean;
}