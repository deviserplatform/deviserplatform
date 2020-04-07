import { FormBehaviour } from './form-behaviour';
import { AdminResult } from './admin-result';

export interface FormResult extends AdminResult {    
    formBehaviour: FormBehaviour;
}