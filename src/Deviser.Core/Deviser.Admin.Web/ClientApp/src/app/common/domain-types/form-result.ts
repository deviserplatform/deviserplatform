import { FormBehaviour } from './form-behaviour';

export interface FormResult {
    successMessage: string;
    errorMessage: string;
    formBehaviour: FormBehaviour;
    result: any;
    isSucceeded: boolean;
}