import { Field } from "./field";
import { ModelConfig } from "./model-config";

export interface ChildConfig {
    field: Field
    modelConfig: ModelConfig
}