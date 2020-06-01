import { Property } from './property';

export interface PlaceHolder {
    id: string;
    type: string;
    layoutTemplate: string;
    sortOrder?: number;
    //Module: Module;
    layoutTypeId?: string;
    properties?: Property[];
    placeHolders?: PlaceHolder[];
}