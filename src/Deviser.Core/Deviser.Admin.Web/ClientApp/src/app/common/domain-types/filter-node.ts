import { LogicalOperator } from './logical-operator';
import { Filter } from './filter';

export interface FilterNode {
    rootOperator: LogicalOperator;
    childOperator: LogicalOperator;
    filter?: Filter;
    parent?: FilterNode;
    childNodes?: FilterNode[];
}