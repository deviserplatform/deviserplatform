import { FormBehaviour } from './form-behaviour';
import { AdminResult } from './admin-result';
import { ClientAction } from './client-action';

export interface FormResult extends AdminResult {    
    formBehaviour: FormBehaviour;
    successAction: ClientAction;
}