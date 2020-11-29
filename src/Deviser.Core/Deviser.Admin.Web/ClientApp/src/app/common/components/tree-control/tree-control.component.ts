import { Component, OnInit, Input, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { FlatTreeControl } from '@angular/cdk/tree';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { SelectionModel } from '@angular/cdk/collections';

export interface Node {
  id: string;
  children: Node[];
  name: string;
  sourceNode: any;
}

export class FlatNode {
  expandable: boolean;
  name: string;
  level: number;
  id: string;
  sourceNode: any;
}

const TREE_DATA: any[] = [
  {
    id: '1',
    name: 'Fruit',
    children: [{
      id: '1.1',
      name: 'Apple'
    }, {
      id: '1.2',
      name: 'Banana',
    }, {
      id: '1.3',
      name: 'Fruit loops',
    }, {
      id: '1.4',
      name: 'Orange'
    }],
  },
  {
    id: '2',
    name: 'Vegetables',
    children: [{
      id: '2.1',
      name: 'Green',
      children: [{
        id: '2.1.1',
        name: 'Broccoli',
      }, {
        id: '2.1.2',
        name: 'Brussels sprouts'
      }],
    },
    {
      id: '2.2',
      name: 'Pumpkins'
    }, {
      id: '2.3',
      name: 'Carrots',
    }]
  },
];

@Component({
  selector: 'app-tree-control',
  templateUrl: './tree-control.component.html',
  styleUrls: ['./tree-control.component.scss']
})
export class TreeControlComponent implements OnInit {

  @Input() treeData: Observable<any[]>;
  @Input() keyField: string;
  @Input() childrenField: string;
  @Input() displayField: string;
  @Input() parentField: string;
  @Input() sortField: string;

  @Output() nodeDrop = new EventEmitter<any>();
  @Output() nodeAdd = new EventEmitter<any>();
  @Output() nodeDelete = new EventEmitter<any>();
  @Output() nodeSelect = new EventEmitter<any>();

  dataSource: MatTreeFlatDataSource<Node, FlatNode>;
  dataChange = new BehaviorSubject<Node[]>([]);
  expandedNodeSet = new Set<string>();
  /** Map from flat node to nested node. This helps us finding the nested node to be modified */
  flatNodeMap = new Map<FlatNode, Node>();
  /** Map from nested node to flattened node. This helps us to keep the same object for selection */
  nestedNodeMap = new Map<Node, FlatNode>();
  /** The new item's name */
  newItemName = '';
  /** A selected parent node to be inserted */
  selectedNode: FlatNode | null = null;
  treeControl: FlatTreeControl<FlatNode>;
  treeFlattener: MatTreeFlattener<Node, FlatNode>;

  get data(): Node[] { return this.dataChange.value; }

  /** The selection for checklist */
  checklistSelection = new SelectionModel<FlatNode>(true /* multiple */);

  /* Drag and drop */
  dragNode: any;
  dragNodeExpandOverWaitTimeMs = 300;
  dragNodeExpandOverNode: any;
  dragNodeExpandOverTime: number;
  dragNodeExpandOverArea: number;

  @ViewChild('emptyItem') emptyItem: ElementRef;

  constructor(/*private database: ChecklistDatabase*/) {
    this.treeFlattener = new MatTreeFlattener(this.transformer, this.getLevel, this.isExpandable, this.getChildren);
    this.treeControl = new FlatTreeControl<FlatNode>(this.getLevel, this.isExpandable);
    this.dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

    this.dataChange.subscribe(data => {
      this.dataSource.data = [];
      this.dataSource.data = data;
    });
  }
  ngOnInit(): void {
    if (!this.treeData) return;
    this.treeData.subscribe(treeData =>{
      // this.dataChange.next(this.parseInputTree(treeData));
      this.rebuildTreeForData(treeData);
    });
  }

  
  rebuildTreeForData(treeData: any) {
    this.rememberExpandedTreeNodes(this.treeControl, this.expandedNodeSet);    
    this.dataChange.next(this.parseInputTree(treeData));
    this.expandNodesById(this.treeControl.dataNodes, Array.from(this.expandedNodeSet));
  }

  parseInputTree(treeData: any[]): Node[] {
    const nodes: Node[] = [];
    treeData.forEach(sourceNode => {
      const children = sourceNode[this.childrenField];
      sourceNode[this.childrenField] = null;
      const node = {
        id: sourceNode[this.keyField],
        name: sourceNode[this.displayField],
        children: [],
        sourceNode
      };

      if (children &&
        Array.isArray(children) &&
        (children as any[]).length > 0) {
        node.children = this.parseInputTree(children);
      }
      nodes.push(node);
    });
    return nodes;
  }

  toOutputTree(nodes: Node[], parent: Node = null): any[] {
    const sourceNodes = [];
    nodes.forEach((node, index) => {
      const sourceNode = node.sourceNode;
      let children = [];
      if (node.children && node.children.length > 0) {
        children = this.toOutputTree(node.children, node);
      }
      sourceNode[this.childrenField] = children;
      sourceNode[this.sortField] = index + 1;
      if (parent) {
        const parentSourceNode = JSON.parse(JSON.stringify(parent.sourceNode));
        parentSourceNode[this.childrenField] = null;
        sourceNode[this.parentField] = (parent) ? parentSourceNode : null;
      }
      sourceNodes.push(sourceNode);
    });
    return sourceNodes;
  }

  getLevel = (node: FlatNode) => node.level;

  isExpandable = (node: FlatNode) => node.expandable;

  getChildren = (node: Node): Node[] => node.children;

  hasChild = (_: number, _nodeData: FlatNode) => _nodeData.expandable;

  hasNoContent = (_: number, _nodeData: FlatNode) => _nodeData.name === '';

  /**
   * Transformer to convert nested node to flat node. Record the nodes in maps for later use.
   */
  transformer = (node: Node, level: number) => {
    const existingNode = this.nestedNodeMap.get(node);
    const flatNode = existingNode && existingNode.id === node.id
      ? existingNode
      : new FlatNode();
    flatNode.id = node.id;
    flatNode.name = node.name;
    flatNode.sourceNode = flatNode.sourceNode;
    flatNode.level = level;
    flatNode.expandable = (node.children && node.children.length > 0);
    this.flatNodeMap.set(flatNode, node);
    this.nestedNodeMap.set(node, flatNode);
    return flatNode;
  }

  /** Whether all the descendants of the node are selected */
  descendantsAllSelected(node: FlatNode): boolean {
    const descendants = this.treeControl.getDescendants(node);
    return descendants.every(child => this.checklistSelection.isSelected(child));
  }

  /** Whether part of the descendants are selected */
  descendantsPartiallySelected(node: FlatNode): boolean {
    const descendants = this.treeControl.getDescendants(node);
    const result = descendants.some(child => this.checklistSelection.isSelected(child));
    return result && !this.descendantsAllSelected(node);
  }

  /** Toggle the to-do item selection. Select/deselect all the descendants node */
  todoItemSelectionToggle(node: FlatNode): void {
    this.checklistSelection.toggle(node);
    const descendants = this.treeControl.getDescendants(node);
    this.checklistSelection.isSelected(node)
      ? this.checklistSelection.select(...descendants)
      : this.checklistSelection.deselect(...descendants);
  }

  /** Select the category so we can insert the new item. */
  addNewItem(flatNode: FlatNode) {
    const parentNode = this.flatNodeMap.get(flatNode);
    const node: Node = this.flatNodeMap.get(flatNode);
    // this.database.insertItem(parentNode, '');
    const sourceNode = {};
    sourceNode[this.parentField] = parentNode.sourceNode;
    const newItem: Node = { id: '', name: '', children: [], sourceNode };
    this.insertItem(parentNode, newItem);
    this.treeControl.expand(flatNode);
    this.dataChange.next(this.data);
    this.nodeAdd.emit({ parent: node.sourceNode, newItem });
  }

  /** Save the node to database */
  saveNode(node: FlatNode, itemValue: string) {
    const nestedNode = this.flatNodeMap.get(node);
    // this.database.updateItem(nestedNode, itemValue);
    this.updateItem(nestedNode, itemValue);
  }

  handleDragStart(event, node) {
    // Required by Firefox (https://stackoverflow.com/questions/19055264/why-doesnt-html5-drag-and-drop-work-in-firefox)
    event.dataTransfer.setData('foo', 'bar');
    //event.dataTransfer.setDragImage(this.emptyItem.nativeElement, 0, 0);
    this.dragNode = node;
    this.treeControl.collapse(node);
  }

  handleDragOver(event, node) {
    event.preventDefault();
    // Handle node expand
    if (this.dragNodeExpandOverNode && node === this.dragNodeExpandOverNode) {
      if ((Date.now() - this.dragNodeExpandOverTime) > this.dragNodeExpandOverWaitTimeMs) {
        if (!this.treeControl.isExpanded(node)) {
          this.treeControl.expand(node);
          //this.cd.detectChanges();
        }
      }
    } else {
      this.dragNodeExpandOverNode = node;
      this.dragNodeExpandOverTime = new Date().getTime();
    }

    // Handle drag area
    const percentageY = event.offsetY / event.target.clientHeight;
    if (0 <= percentageY && percentageY <= 0.25) {
      this.dragNodeExpandOverArea = 1;
    } else if (1 >= percentageY && percentageY >= 0.75) {
      this.dragNodeExpandOverArea = -1;
    } else {
      this.dragNodeExpandOverArea = 0;
    }
  }

  handleDrop(event, node) {
    if (node !== this.dragNode) {
      let newItem: Node;
      if (this.dragNodeExpandOverArea === 1) {
        // newItem = this.database.copyPasteItemAbove(this.flatNodeMap.get(this.dragNode), this.flatNodeMap.get(node));
        newItem = this.copyPasteItemAbove(this.flatNodeMap.get(this.dragNode), this.flatNodeMap.get(node));
      } else if (this.dragNodeExpandOverArea === -1) {
        // newItem = this.database.copyPasteItemBelow(this.flatNodeMap.get(this.dragNode), this.flatNodeMap.get(node));
        newItem = this.copyPasteItemBelow(this.flatNodeMap.get(this.dragNode), this.flatNodeMap.get(node));
      } else {
        // newItem = this.database.copyPasteItem(this.flatNodeMap.get(this.dragNode), this.flatNodeMap.get(node));
        newItem = this.copyPasteItem(this.flatNodeMap.get(this.dragNode), this.flatNodeMap.get(node));
      }
      // this.database.deleteItem(this.flatNodeMap.get(this.dragNode));
      this.deleteNode(this.data, this.flatNodeMap.get(this.dragNode));
      this.dataChange.next(this.data);
      this.treeControl.expandDescendants(this.nestedNodeMap.get(newItem));
      this.nodeDrop.emit(this.toOutputTree(this.data));
    }
    this.handleDragEnd(event);
  }

  handleDragEnd(event) {
    this.dragNode = null;
    this.dragNodeExpandOverNode = null;
    this.dragNodeExpandOverTime = 0;
    this.dragNodeExpandOverArea = NaN;
    event.preventDefault();
  }

  getStyle(node: FlatNode) {
    // const cssClass: string = this.selectedNode === node ? 'selected' : '';
    if (this.dragNode === node) {
      return `drag-start`;
    } else if (this.dragNodeExpandOverNode === node) {
      switch (this.dragNodeExpandOverArea) {
        case 1:
          return `drop-above`;
        case -1:
          return `drop-below`;
        default:
          return `drop-center`;
      }
    }
    return '';
  }

  deleteItem(flatNode: FlatNode) {
    const node: Node = this.flatNodeMap.get(flatNode);
    this.deleteNode(this.data, node);
    this.dataChange.next(this.data);
    this.nodeDelete.emit(node.sourceNode);
  }

  onNodeClick(flatNode: FlatNode) {
    this.selectedNode = flatNode;
    const node: Node = this.flatNodeMap.get(flatNode);
    this.nodeSelect.emit(node.sourceNode);
  }


  /** Add an item to to-do list */
  private cloneNode(source: Node): Node {
    return {
      id: source.id,
      children: source.children,
      name: source.name,
      sourceNode: source.sourceNode
    } as Node
  }

  private insertItem(parent: Node, dragNode: Node): Node {
    if (!parent.children) {
      parent.children = [];
    }
    const newItem = this.cloneNode(dragNode);
    parent.children.push(newItem);
    // this.dataChange.next(this.data);
    return newItem;
  }

  private insertItemAbove(node: Node, dragNode: Node): Node {
    const parentNode = this.getParentFromNodes(node);
    const newItem = this.cloneNode(dragNode);
    if (parentNode != null) {
      parentNode.children.splice(parentNode.children.indexOf(node), 0, newItem);
    } else {
      this.data.splice(this.data.indexOf(node), 0, newItem);
    }
    // this.dataChange.next(this.data);
    return newItem;
  }

  private insertItemBelow(node: Node, dragNode: Node): Node {
    const parentNode = this.getParentFromNodes(node);
    const newItem = this.cloneNode(dragNode);
    if (parentNode != null) {
      parentNode.children.splice(parentNode.children.indexOf(node) + 1, 0, newItem);
    } else {
      this.data.splice(this.data.indexOf(node) + 1, 0, newItem);
    }
    // this.dataChange.next(this.data);
    return newItem;
  }

  private getParentFromNodes(node: Node): Node {
    for (let i = 0; i < this.data.length; ++i) {
      const currentRoot = this.data[i];
      const parent = this.getParent(currentRoot, node);
      if (parent != null) {
        return parent;
      }
    }
    return null;
  }

  private getParent(currentRoot: Node, node: Node): Node {
    if (currentRoot.children && currentRoot.children.length > 0) {
      for (let i = 0; i < currentRoot.children.length; ++i) {
        const child = currentRoot.children[i];
        if (child === node) {
          return currentRoot;
        } else if (child.children && child.children.length > 0) {
          const parent = this.getParent(child, node);
          if (parent != null) {
            return parent;
          }
        }
      }
    }
    return null;
  }

  private updateItem(node: Node, name: string) {
    node.name = name;
    this.dataChange.next(this.data);
  }

  private deleteNode(nodes: Node[], nodeToDelete: Node) {
    const index = nodes.indexOf(nodeToDelete, 0);
    if (index > -1) {
      nodes.splice(index, 1);
    } else {
      nodes.forEach(node => {
        if (node.children && node.children.length > 0) {
          this.deleteNode(node.children, nodeToDelete);
        }
      });
    }
  }

  private copyPasteItem(from: Node, to: Node): Node {
    const newItem = this.insertItem(to, from);
    if (from.children) {
      from.children.forEach(child => {
        this.copyPasteItem(child, newItem);
      });
    }
    return newItem;
  }

  private copyPasteItemAbove(from: Node, to: Node): Node {
    const newItem = this.insertItemAbove(to, from);
    if (from.children) {
      from.children.forEach(child => {
        this.copyPasteItem(child, newItem);
      });
    }
    return newItem;
  }

  private copyPasteItemBelow(from: Node, to: Node): Node {
    const newItem = this.insertItemBelow(to, from);
    if (from.children) {
      from.children.forEach(child => {
        this.copyPasteItem(child, newItem);
      });
    }
    return newItem;
  }

  private rememberExpandedTreeNodes(
    treeControl: FlatTreeControl<FlatNode>,
    expandedNodeSet: Set<string>
  ) {
    if (treeControl.dataNodes) {
      treeControl.dataNodes.forEach((node) => {
        if (treeControl.isExpandable(node) && treeControl.isExpanded(node)) {
          // capture latest expanded state
          expandedNodeSet.add(node.id);
        }
      });
    }
  }

  private expandNodesById(flatNodes: FlatNode[], ids: string[]) {
    if (!flatNodes || flatNodes.length === 0) return;
    const idSet = new Set(ids);
    return flatNodes.forEach((node) => {
      if (idSet.has(node.id)) {
        this.treeControl.expand(node);
        let parent = this.getParentNode(node);
        while (parent) {
          this.treeControl.expand(parent);
          parent = this.getParentNode(parent);
        }
      }
    });
  }

  private getParentNode(node: FlatNode): FlatNode | null {
    const currentLevel = node.level;
    if (currentLevel < 1) {
      return null;
    }
    const startIndex = this.treeControl.dataNodes.indexOf(node) - 1;
    for (let i = startIndex; i >= 0; i--) {
      const currentNode = this.treeControl.dataNodes[i];
      if (currentNode.level < currentLevel) {
        return currentNode;
      }
    }
    return null;
  }
}