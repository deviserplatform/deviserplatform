import { LookUpDictionary } from "./look-up-dictionary";
import { ModelConfig } from "./model-config";
import { ChildConfig } from "./child-config";
import { AdminConfigType } from './admin-confit-type';

export interface AdminConfig {
    adminConfigType: AdminConfigType;
    adminTitle: string;
    childConfigs: ChildConfig[];
    modelType: string;
    modelConfig: ModelConfig;
    lookUps: LookUpDictionary;
}
