import { Component, Inject, ViewChild } from '@angular/core';
import { CdkDragDrop, CdkDragEnter, CdkDragExit, moveItemInArray, transferArrayItem, DragRef, DropListRef } from '@angular/cdk/drag-drop';
import { Guid } from '../../services/guid';
import { LayoutTypeService } from '../../services/layout-type.service';
import { LayoutService } from '../../services/layout.service';
import { PageService } from '../../services/page.service';
import { PageContext } from '../../domain-types/page-context';
import { WINDOW } from '../../services/window.service';
import { forkJoin } from 'rxjs';
import { PageLayout } from '../../domain-types/page-layout';
import { LayoutType } from '../../domain-types/layout-type';
import { Alert, AlertType } from '../../domain-types/alert';
import { AlertService } from '../../services/alert.service';
import { PlaceHolder } from '../../domain-types/place-holder';
import { SharedService } from '../../services/shared.service';
import { Property } from '../../domain-types/property';
import { Placeholder } from '@angular/compiler/src/i18n/i18n_ast';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';

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

  root: PlaceHolder;
  // newElements: Node[];

  layoutTypes: LayoutType[] = [];
  pageLayouts: PageLayout[] = [];
  selectedLayout: PageLayout;
  selectedPlaceHolder: PlaceHolder;
  isLayoutNameEditable: boolean;
  layoutName: string;

  @ViewChild(ConfirmDialogComponent)
  private confirmDialogComponent: ConfirmDialogComponent;

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

  private _pageContext: PageContext;

  constructor(private _alertService: AlertService,
    private _layoutService: LayoutService,
    private _layoutTypeService: LayoutTypeService,
    private _sharedService: SharedService,
    @Inject(WINDOW) private _window: any
  ) {

    this._pageContext = _window.pageContext;

    // this.root = {
    //   id: 'root',
    //   type: 'container',
    //   children: TREE_DATA
    // };
    this.init();
    // this.newElements = NEW_ELEMENTS;
  }

  init() {
    const layoutTypes$ = this._layoutTypeService.getLayoutTypes();
    const pageLayout$ = this._layoutService.getLayouts();
    forkJoin([layoutTypes$, pageLayout$]).subscribe(results => {
      this.layoutTypes = results[0];
      this.pageLayouts = results[1];
      this.onGetLayouts();
    }, error => {
      const alert: Alert = {
        alertType: AlertType.Error,
        message: 'Unable to get this item, please contact administrator',
        timeout: 5000
      }
      this._alertService.addAlert(alert);
    });
  }

  onGetLayouts() {
    this.selectedLayout = this.pageLayouts.find(pl => pl.id === this._pageContext.layoutId);
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
      pageId: this._pageContext.currentPageId,
      placeHolders: [],
      isChanged: true,
      isActive: true
    };
    this.setPageLayout();
    this.isLayoutNameEditable = true;
  }

  onCopyLayout() {
    this.selectedLayout.id = undefined;
    this.selectedLayout.name = "";
    this.selectedLayout.isChanged = true;
  }

  onSaveLayout() {
    this.parsePlaceHolders(this.selectedLayout.placeHolders, true);

    //vm.pageLayout.config = JSON.stringify(vm.pageLayout.placeHolders);
    this.selectedLayout.pageId = this._pageContext.currentPageId;
    if (this.selectedLayout.id) {
      //Update layout
      this._layoutService.updateLayout(this.selectedLayout)
        .subscribe(pageLayout => this.onActionResult(pageLayout));
      // layoutService.put(vm.pageLayout)
      //   .then(function (data) {
      //     //console.log(data);
      //     vm.pageLayout.id = data.id;
      //     vm.pageLayout.placeHolders = data.placeHolders;
      //     vm.pageLayout.isChanged = false;
      //     showMessage("success", "Layout has been saved");
      //     processplaceHolders(vm.pageLayout.placeHolders);
      //     vm.isLayoutEdit = false;
      //   }, function (error) {
      //     showMessage("error", SYS_ERROR_MSG);
      //   });
    }
    else {
      //Create new layout
      this._layoutService.createLayout(this.selectedLayout)
        .subscribe(pageLayout => this.onActionResult(pageLayout));
      // layoutService.post(vm.pageLayout)
      //   .then(function (data) {
      //     console.log(data);
      //     vm.pageLayout.id = data.id;
      //     vm.pageLayout.placeHolders = data.placeHolders;
      //     vm.pageLayout.isChanged = false;
      //     showMessage("success", "Layout has been saved");
      //     getLayouts().then(function () {
      //       processplaceHolders(vm.pageLayout.placeHolders);
      //     });
      //     vm.isLayoutEdit = false;
      //   }, function (error) {
      //     showMessage("error", SYS_ERROR_MSG);
      //   });
    }
  }

  onDeleteLayout() {
    // let labels = {
    //   title: 'Delete Confirmation',
    //   body: 'Are you sure to delete the layout',
    //   okLabel: 'Yes',
    //   cancelLabel: 'No'
    // };

    this.confirmDialogComponent.openModal(this.selectedLayout);

    // let modalInstance = modalService.showConfirmation(labels);

    // modalInstance.then(function () {
    //   layoutService.remove(vm.pageLayout.id).then(function (data) {
    //     console.log('Layout deletion agreed at:' + new Date());
    //     showMessage("success", "Layout has been removed");
    //     getLayouts();
    //     newLayout();
    //   }, function (error) {
    //     showMessage("error", SYS_ERROR_MSG);
    //   })
    // },
    //   function () {
    //     console.log('Layout deletion disagreed at:' + new Date());
    //   });
  }

  onYesToDelete(layout: any) {
    this._layoutService.deleteLayout(this.selectedLayout.id)
      .subscribe(response => this.onLayoutDeleted(response))
  }

  onNoToDelete(layout: any) {
    console.log('declined');
  }

  onActionResult(formValue: any): void {
    if (formValue) {
      this.selectedLayout.id = formValue.id;
      this.selectedLayout.placeHolders = formValue.placeHolders;
      this.selectedLayout.isChanged = false;
      this.parsePlaceHolders(this.selectedLayout.placeHolders);
      //     vm.isLayoutEdit = false;

      const alert: Alert = {
        alertType: AlertType.Success,
        message: 'Layout has been saved',
        timeout: 5000
      }
      this._alertService.addAlert(alert);
    }
    else {
      const alert: Alert = {
        alertType: AlertType.Error,
        message: 'Layout has been saved',
        timeout: 5000
      }
      this._alertService.addAlert(alert);
    }
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
    if(!placeHolder.properties){
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
    let ids = [item.id];
    item.placeHolders.forEach((childItem) => { ids = ids.concat(this.getIdsRecursive(childItem)); });
    return ids;
  }

}
