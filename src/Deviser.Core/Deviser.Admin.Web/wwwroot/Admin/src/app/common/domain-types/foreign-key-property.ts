export interface ForeignKeyProperty {    
    fieldName: string;
    fieldNameCamelCase: string;
    fKEntityType: string;
    fKFieldType: string;
    isPKProperty: boolean;
    pKEntityType: string;
    pKFieldType: string;
    principalFieldName: string;
    principalFieldNameCamelCase: string
}