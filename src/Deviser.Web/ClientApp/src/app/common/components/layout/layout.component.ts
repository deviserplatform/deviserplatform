import { Component, Inject, ViewChild } from '@angular/core';
import { forkJoin } from 'rxjs';
import { CdkDragDrop, CdkDragEnter, CdkDragExit, moveItemInArray, transferArrayItem, DragRef, DropListRef } from '@angular/cdk/drag-drop';
import { Alert, AlertType, Guid, LayoutType, PageContext, PageLayout, PlaceHolder, WINDOW } from 'deviser-shared';
import { AlertService, LayoutService, LayoutTypeService, SharedService } from 'deviser-shared';
import { ConfirmDialogComponent } from 'deviser-shared';

export class Node {
  id?: string;
  children?: Node[];
  type: string;
}

const TREE_DATA: Node[] = [
  {
    type: 'container',
    id: 'r',
    children: [
      {
        type: 'container',
        id: '1',
        children: [{
          type: 'container',
          id: '4',
          children: [{
            type: 'item',
            id: '5',
            children: []
          }]
        },
        {
          type: 'item',
          id: '6',
          children: []
        }]
      },
      {
        type: 'item',
        id: '2',
        children: []
      },
      {
        type: 'item',
        id: '3',
        children: []
      }
    ]
  },
  {
    type: 'item',
    id: '7',
    children: []
  },
  {
    type: 'item',
    id: '8',
    children: []
  }
];

// const NEW_ELEMENTS: Node[] = [
//   {
//     type: 'item'
//   },
//   {
//     type: 'container'
//   }
// ];


@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent {

  isLayoutNameEditable: boolean;
  isNewMode: boolean;
  layoutName: string;
  layoutTypes: LayoutType[] = [];
  pageLayouts: PageLayout[] = [];
  pageContext: PageContext;
  root: PlaceHolder;
  selectedLayout: PageLayout;
  selectedPlaceHolder: PlaceHolder;

  get nestedContainersDropListIds(): string[] {
    // We reverse ids here to respect items nesting hierarchy
    const recursiveIds = this.getIdsRecursive(this.root).reverse();
    // recursiveIds.splice(0, 0, 'newElementList');
    return recursiveIds;
  }

  get trashDropListIds(): string[] {
    // We reverse ids here to respect items nesting hierarchy
    const recursiveIds = this.getIdsRecursive(this.root).reverse();
    recursiveIds.push('trashList');
    return recursiveIds;
  }

  @ViewChild(ConfirmDialogComponent)
  private _confirmDialogComponent: ConfirmDialogComponent;

  constructor(private _alertService: AlertService,
    private _layoutService: LayoutService,
    private _layoutTypeService: LayoutTypeService,
    private _sharedService: SharedService,
    @Inject(WINDOW) window: any
  ) {

    this.pageContext = window.pageContext;
    // this.root = {
    //   id: 'root',
    //   type: 'container',
    //   children: TREE_DATA
    // };
    this.init();
    // this.newElements = NEW_ELEMENTS;
  }

  init() {
    this.isNewMode = false;
    const layoutTypes$ = this._layoutTypeService.getLayoutTypes();
    const pageLayout$ = this._layoutService.getLayouts();
    forkJoin([layoutTypes$, pageLayout$]).subscribe(results => {
      this.layoutTypes = results[0];
      this.pageLayouts = results[1];
      this.onGetLayouts();
    }, error => {
      this._alertService.showMessage(AlertType.Error, 'Unable to  get this item');
    });
  }

  onGetLayouts() {
    this.selectedLayout = this.pageLayouts.find(pl => pl.id === this.pageContext.layoutId);
    this.setPageLayout();
    console.log(this.selectedLayout);
  }

  onPageLayoutChange($event) {
    this.setPageLayout();
  }

  onDragDrop(event: CdkDragDrop<any, any | LayoutType[]>) {
    const containerData: PlaceHolder[] = event.container.data.placeHolders;
    const previousContainerData: PlaceHolder[] = event.previousContainer.data.placeHolders as PlaceHolder[];
    const previousContainerDataAsLayoutTypes: LayoutType[] = event.previousContainer.data as LayoutType[];
    if (previousContainerData === containerData) {
      moveItemInArray(containerData, event.previousIndex, event.currentIndex);
    } else if (this.layoutTypes === previousContainerDataAsLayoutTypes) {
      const item = previousContainerDataAsLayoutTypes[event.previousIndex];
      const itemToCopy: PlaceHolder = {
        id: Guid.newGuid(),
        // layoutTemplate: this.getLayoutTemplate(item.name),
        layoutTypeId: item.id,
        type: item.name,
        placeHolders: []
      };
      // copyArrayItem(previousContainerData, containerData, event.previousIndex, event.currentIndex);
      containerData.splice(event.currentIndex, 0, itemToCopy);
    } else {
      transferArrayItem(previousContainerData,
        containerData,
        event.previousIndex,
        event.currentIndex);
    }
  }

  onDropToTrash(event: CdkDragDrop<any>) {
    // const containerData: PlaceHolder[] = event.container.data;
    if (!event.previousContainer.data.placeHolders) {
      return;
    }
    const previousContainerData: PlaceHolder[] = event.previousContainer.data.placeHolders;
    previousContainerData.splice(event.previousIndex, 1);
  }

  elementDropPredicate = (drag: DragRef, drop: DropListRef): boolean => {
    const dragPlaceHolder = drag.data as PlaceHolder;
    const dragLayoutType = drag.data as LayoutType;
    const dropListPlaceHolder = drop.data as PlaceHolder
    if ((dragPlaceHolder || dragLayoutType) && dropListPlaceHolder) {
      let dragItemType: LayoutType;
      const dropItemType = this.getLayoutType(dropListPlaceHolder.layoutTypeId);
      if (dragLayoutType && dragLayoutType.name && dragLayoutType.layoutTypeIds) {
        dragItemType = dragLayoutType;
      }
      else {
        dragItemType = this.getLayoutType(dragPlaceHolder.layoutTypeId);
      }
      return dropItemType.layoutTypeIds.replace(/\s/g, '').split(',').indexOf(dragItemType.id) >= 0;
    }
    return false;
  }

  onPlaceHolderSelected($event: Event, node: PlaceHolder) {
    $event.stopPropagation();

    let propertiesValue = node.properties;
    this.syncPropertyForElement(node);
    //columnwidth update - hard coded behaviour only for property 'column_width'
    let columnWidthProp = this._sharedService.getColumnWidthProperty(propertiesValue);
    if (columnWidthProp && !columnWidthProp.value) {
      let columnWidth = columnWidthProp.optionList.list.find(item => item.name === this._sharedService.defaultWidth);
      columnWidthProp.value = columnWidth.id;
    }

    this.selectedPlaceHolder = node;
  }

  onEditLayoutName() {
    this.layoutName = this.selectedLayout.name;
    this.isLayoutNameEditable = true;
  }

  onSaveLayoutName() {
    this.selectedLayout.name = this.layoutName;
    this.isLayoutNameEditable = false;
  }

  onCancelEditLayoutName() {
    this.isLayoutNameEditable = false;
  }

  onNewLayout() {
    this.selectedLayout = {
      id: undefined,
      name: '',
      pageId: this.pageContext.currentPageId,
      placeHolders: [],
      isChanged: true,
      isActive: true
    };
    this.setPageLayout();
    this.layoutName = '';
    this.isLayoutNameEditable = true;
    this.isNewMode = true;
  }

  onCopyLayout() {
    this.selectedLayout.id = undefined;
    this.selectedLayout.name = "";
    this.selectedLayout.isChanged = true;
  }

  onSaveLayout() {
    this.parsePlaceHolders(this.selectedLayout.placeHolders, true);
    this.selectedLayout.pageId = this.pageContext.currentPageId;
    if (this.selectedLayout.id) {
      //Update layout
      this._layoutService.updateLayout(this.selectedLayout)
        .subscribe(pageLayout => this.onLayoutSaved(pageLayout));
    }
    else {
      //Create new layout
      this._layoutService.createLayout(this.selectedLayout)
        .subscribe(pageLayout => this.onLayoutSaved(pageLayout));
    }
  }

  onLayoutSaved(formValue: any): void {
    this.isNewMode = false;
    if (formValue) {
      this.selectedLayout.id = formValue.id;
      this.selectedLayout.placeHolders = formValue.placeHolders;
      this.selectedLayout.isChanged = false;
      this.setPageLayout();
      this._alertService.showMessage(AlertType.Success, 'Layout has been saved');
    }
    else {
      this._alertService.showMessage(AlertType.Error, 'Unable to save the Layout');
    }
  }

  onDeleteLayout() {
    this._confirmDialogComponent.openModal(this.selectedLayout);
  }

  onYesToDelete(layout: any) {
    this._layoutService.deleteLayout(this.selectedLayout.id)
      .subscribe(response => this.onLayoutDeleted(response))
  }

  onNoToDelete(layout: any) {
    console.log('declined');
  }

  onLayoutDeleted(response: any) {
    console.log(response);
    this.init();
    this.onNewLayout();
  }

  getLayoutTypeName(layoutTypeId: string): string {
    return this.getLayoutType(layoutTypeId).label;
  }

  getLayoutType(layoutTypeId: string): LayoutType {
    return this.layoutTypes.find(lt => lt.id === layoutTypeId)
  }

  // getLayoutTemplate(layoutType: string) {
  //   if (layoutType === 'row' || layoutType === 'column' || layoutType === 'wrapper' || layoutType === 'container') {
  //     return layoutType;
  //   }
  //   return 'repeater';
  // }

  getPlaceHolderClass(node: PlaceHolder) {
    let className = this.selectedPlaceHolder && node && this.selectedPlaceHolder.id === node.id ? 'selected' : '';
    return className;
  }

  getColumnClass(node: PlaceHolder) {
    if (node.type === 'column') {
      let columnClass: string = this._sharedService.getColumnWidth(node.properties);
      return columnClass.replace('col-md-', 'col-');
    }
    return '';
  }

  private setPageLayout(): void {
    if (!this.selectedLayout) return;
    
    if (this.selectedLayout.placeHolders &&
      this.selectedLayout.placeHolders.length > 0) {
      this.parsePlaceHolders(this.selectedLayout.placeHolders);
    }
    this.root = {
      id: 'root',
      // layoutTemplate: 'repeater',
      layoutTypeId: '9341f92e-83d8-4afe-ad4a-a95deeda9ae3',
      type: 'container',
      placeHolders: this.selectedLayout.placeHolders
    };
  }

  private parsePlaceHolders(placeHolders: PlaceHolder[], toSave: boolean = false) {
    if (placeHolders) {
      placeHolders.forEach((item, index) => {
        let masterLayout = this.layoutTypes.find(lt => lt.id === item.layoutTypeId);
        if (masterLayout) {
          item.label = masterLayout.label;
        }

        if (toSave && item.properties) {
          item.properties.forEach(prop => {
            prop.optionList = null;
          });
        }
        else {
          this.syncPropertyForElement(item);
          //Refresh selected item
          if (this.selectedPlaceHolder && item.id === this.selectedPlaceHolder.id) {
            this.selectedPlaceHolder = item;
          }
        }
        item.sortOrder = index + 1;

        if (item.placeHolders) {
          this.parsePlaceHolders(item.placeHolders, toSave);
        }
      });
    }
  }

  private syncPropertyForElement(placeHolder: PlaceHolder) {
    if (!placeHolder.properties) {
      placeHolder.properties = [];
    }
    let propertiesValue = placeHolder.properties;
    let masterLayout = this.layoutTypes.find(lt => lt.id === placeHolder.layoutTypeId);
    // placeHolder.label = masterLayout.label;
    let masterProperties = masterLayout.properties;
    masterProperties.forEach(prop => {
      if (prop) {
        let propVal = propertiesValue.find(pv => pv.id === prop.id);
        if (propVal) {
          //Property exist, update property label
          propVal.label = prop.label;
          propVal.description = prop.description;
          propVal.defaultValue = prop.defaultValue;
          propVal.optionList = prop.optionList;
          propVal.optionListId = prop.optionListId;
        }
        else {
          //Property not exist, add the property                      
          placeHolder.properties.push(JSON.parse(JSON.stringify(prop)));
        }
      }
    });
  }

  private canBeDropped(event: CdkDragDrop<PlaceHolder, PlaceHolder>): boolean {
    const movingItem: PlaceHolder = event.item.data;

    return event.previousContainer.id !== event.container.id
      && this.isNotSelfDrop(event)
      && !this.hasChild(movingItem, event.container.data);
  }

  private isNotSelfDrop(event: CdkDragDrop<PlaceHolder> | CdkDragEnter<PlaceHolder> | CdkDragExit<PlaceHolder>): boolean {
    return event.container.data.id !== event.item.data.id;
  }

  private hasChild(parent: PlaceHolder, child: PlaceHolder): boolean {
    const hasChild = parent.placeHolders.some((item) => item.id === child.id);
    return hasChild ? true : parent.placeHolders.some((item) => this.hasChild(item, child));
  }

  private getIdsRecursive(item: PlaceHolder): string[] {
    if (!item) {
      return [''];
    }
    let ids = [item.id];
    item.placeHolders.forEach((childItem) => { ids = ids.concat(this.getIdsRecursive(childItem)); });
    return ids;
  }

}
