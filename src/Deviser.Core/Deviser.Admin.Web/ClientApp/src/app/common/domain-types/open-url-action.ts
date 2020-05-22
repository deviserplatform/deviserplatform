import { ClientAction } from './client-action';

export class OpenUrlAction implements ClientAction {
    url: string;
    openAfterSec: number;
}