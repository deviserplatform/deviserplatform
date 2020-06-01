import { OptionList } from './option-list';
import { PropertyOption } from './property-option';

export interface Property
    {
        id: string;
        name: string;
        label: string;
        //Value property is not in DB, It is here to maintain JSON strucuture
        nalue: string;
        defaultValue: string;
        description: string;
        optionListId: string | null;        
        optionList: OptionList;
        defaultValuePropertyOption: PropertyOption;
        isActive: boolean;
        createdDate: Date | string | null;
        lastModifiedDate: Date | string | null;
    }