import { LookUpDictionary } from "./look-up-dictionary";
import { ModelConfig } from "./model-config";
import { ChildConfig } from "./child-config";
import { AdminConfigType } from './admin-confit-type';
import { FilterOperator } from './filter-operator';

export interface AdminConfig {
    adminConfigType: AdminConfigType;
    adminTitle: string;
    childConfigs: ChildConfig[];
    filterOperator: FilterOperator;
    modelType: string;
    modelConfig: ModelConfig;
    lookUps: LookUpDictionary;
}
