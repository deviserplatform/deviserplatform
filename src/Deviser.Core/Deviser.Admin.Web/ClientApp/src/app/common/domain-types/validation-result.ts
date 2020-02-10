import { ValidationError } from "./validation-error";

export interface ValidationResult {
    succeeded: boolean;
    errors:ValidationError[];
}
