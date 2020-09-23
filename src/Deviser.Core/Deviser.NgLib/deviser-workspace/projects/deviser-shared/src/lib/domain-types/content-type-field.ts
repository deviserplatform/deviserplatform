import { ContentType } from './content-type';
import { ContentFieldType } from './content-field-type';

export interface ContentTypeField
{
    id: string;
    fieldName: string;
    fieldLabel: string;
    fieldDescription: string;
    contentTypeId: string;
    contentFieldTypeId: string;
    sortOrder: number;
    isShownOnList: boolean;
    isShownOnPreview: boolean;
    contentType: ContentType;
    contentFieldType: ContentFieldType;
}