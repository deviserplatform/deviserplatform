import { PropertyOption } from './property-option';

export interface OptionList
{
    id: string;
    name: string;
    label: string;
    list: PropertyOption[];
    isActive: boolean;
    createdDate: Date | string | null;
    lastModifiedDate: Date | string | null;
    //IsActiveText: string;
    isActiveBadgeClass: string;
}