import { LookUpDictionary } from "./look-up-dictionary";
import { ModelConfig } from "./model-config";
import { ChildConfig } from "./child-config";

export interface AdminConfig {
    adminTitle: string;
    childConfigs: ChildConfig[];
    modelType: string;
    modelConfig: ModelConfig;
    lookUps: LookUpDictionary;
}
