import { LookUpField } from "./look-up-field";

export interface LookUpDictionary {
    lookUpData: { [key: string]: LookUpField[] };
}