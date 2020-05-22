import { LabelType } from "./label-type"

export interface LabelOption {
    labelType: LabelType;
    parameters: { [key: string]: string };
}