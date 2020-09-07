import { FileItemType } from './file-item-type';

export interface FileItem
{
    name: string;
    path?: string;
    extension?: string;
    type?: FileItemType;
}