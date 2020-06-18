import { Component, OnInit, Inject } from '@angular/core';
import { PageContext } from '../../domain-types/page-context';
import { WINDOW } from '../../services/window.service';
import { AlertService } from '../../services/alert.service';
import { Alert, AlertType } from '../../domain-types/alert';
import { PageService } from '../../services/page.service';
import { PageContentService } from '../../services/page-content.service';
import { LayoutService } from '../../services/layout.service';
import { LayoutTypeService } from '../../services/layout-type.service';
import { ContentTypeService } from '../../services/content-type.service';
import { ModuleViewService } from '../../services/module-view.service';
import { PageModuleService } from '../../services/page-module.service';
import { forkJoin } from 'rxjs';
import { Page } from '../../domain-types/page';
import { Globals } from '../../config/globals';
import { PageLayout } from '../../domain-types/page-layout';
import { ContentType } from '../../domain-types/content-type';
import { PageElement } from '../../domain-types/page-element';
import { LayoutType } from '../../domain-types/layout-type';
import { ModuleView } from '../../domain-types/module-view';
import { PageContent } from '../../domain-types/page-content';
import { PageModule } from '../../domain-types/page-module';
import { Placeholder } from '@angular/compiler/src/i18n/i18n_ast';
import { PlaceHolder } from '../../domain-types/place-holder';
import { element } from 'protractor';
import { sortBy, includes, reject, iteratee } from 'lodash-es';
import { SharedService } from '../../services/shared.service';
import { CdkDragDrop, moveItemInArray, transferArrayItem, DragRef, DropListRef, CdkDragEnter, CdkDragExit } from '@angular/cdk/drag-drop';
import { Guid } from '../../services/guid';
import { ItemsList } from '@ng-select/ng-select/lib/items-list';
import { PageState } from '../../domain-types/page-state';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditComponent implements OnInit {

  alerts: Alert[] = [];
  currentPage: Page;
  currentPageState: PageState;
  containerIds: string[] = [];
  contentTypes: PlaceHolder[];
  layoutTypes: LayoutType[];
  moduleViews: PlaceHolder[];
  pageContents: PageContent[];
  pageContext: PageContext;
  pageLayout: PageLayout;
  pageModules: PageModule[];
  root: PlaceHolder;
  selectedPlaceHolder: PlaceHolder;
  uaLayout: any = {}

  pageState = PageState;

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

  constructor(private _alertService: AlertService,
    private _contentTypeService: ContentTypeService,
    private _layoutService: LayoutService,
    private _layoutTypeService: LayoutTypeService,
    private _moduleViewService: ModuleViewService,
    private _pageService: PageService,
    private _pageContentService: PageContentService,
    private _pageModuleService: PageModuleService,
    private _sharedService: SharedService,
    @Inject(WINDOW) window: any) {
    this.pageContext = window.pageContext;
    this._alertService.alerts.subscribe(alert => {
      if (alert) {
        this.alerts.push(alert)
      }
    });

    this.init();
  }

  ngOnInit(): void {
  }

  onDragDrop(event: CdkDragDrop<any, any | LayoutType[]>) {
    const containerData: PlaceHolder = event.container.data;
    const containerPlaceHolders: PlaceHolder[] = containerData.placeHolders;
    const previousContainerData: PlaceHolder[] = event.previousContainer.data as PlaceHolder[];
    if (previousContainerData === containerPlaceHolders) {
      moveItemInArray(containerPlaceHolders, event.previousIndex, event.currentIndex);
    } else if (this.contentTypes === previousContainerData) {
      //New content types
      const item = previousContainerData[event.previousIndex];
      const itemToCopy: PlaceHolder = {
        //id: Guid.newGuid(),
        type: item.type,
        contentType: item.contentType,
        layoutTemplate: this.getLayoutTemplate(item),
        placeHolders: []
      };
      // copyArrayItem(previousContainerData, containerData, event.previousIndex, event.currentIndex);
      containerPlaceHolders.splice(event.currentIndex, 0, itemToCopy);
    } else if (this.moduleViews === previousContainerData) {
      //New Modules
      const item = previousContainerData[event.previousIndex];
      const itemToCopy: PlaceHolder = {
        //id: Guid.newGuid(),
        type: item.type,
        moduleView: item.moduleView,
        layoutTemplate: this.getLayoutTemplate(item),
        placeHolders: []
      };
      // copyArrayItem(previousContainerData, containerData, event.previousIndex, event.currentIndex);
      containerPlaceHolders.splice(event.currentIndex, 0, itemToCopy);
    }
    else {
      transferArrayItem(previousContainerData,
        containerPlaceHolders,
        event.previousIndex,
        event.currentIndex);
    }

    this.updateElements(containerData);
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
    // const dragPlaceHolder = drag.data as PlaceHolder;
    // const dragLayoutType = drag.data as LayoutType;
    // const dropListPlaceHolder = drop.data as PlaceHolder
    // if ((dragPlaceHolder || dragLayoutType) && dropListPlaceHolder) {
    //   let dragItemType: LayoutType;
    //   const dropItemType = this.getLayoutType(dropListPlaceHolder.layoutTypeId);
    //   if (dragLayoutType && dragLayoutType.name && dragLayoutType.layoutTypeIds) {
    //     dragItemType = dragLayoutType;
    //   }
    //   else {
    //     dragItemType = this.getLayoutType(dragPlaceHolder.layoutTypeId);
    //   }
    //   return dropItemType.layoutTypeIds.replace(/\s/g, '').split(',').indexOf(dragItemType.id) >= 0;
    // }
    return false;
  }

  onPlaceHolderSelected($event: Event, node: PlaceHolder) {
    $event.stopPropagation();
    if (node.layoutTemplate === "content" || node.layoutTemplate === "module") {
      this.selectedPlaceHolder = node;
      node.properties.forEach(prop => {
        if (prop.optionList && prop.optionList.list) {
          prop.optionList = prop.optionList;
        }
      });
    }
  }

  getLayoutTypeName(layoutTypeId: string): string {
    return this.getLayoutType(layoutTypeId).label;
  }

  getLayoutType(layoutTypeId: string): LayoutType {
    return this.layoutTypes.find(lt => lt.id === layoutTypeId)
  }

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

  onSaveProperties() {

    let properties = [];
    this.selectedPlaceHolder.properties.forEach(srcProp => {
      if (srcProp) {
        let prop = {
          id: srcProp.id,
          name: srcProp.name,
          label: srcProp.label,
          value: srcProp.value,
          description: srcProp.description,
          defaultValue: srcProp.defaultValue
        }
        properties.push(prop);
      }
    });
    //It Saves content with properties
    if (this.selectedPlaceHolder.layoutTemplate === "content") {

      //Prepare content to update
      let pageContent: PageContent = {
        id: this.selectedPlaceHolder.pageContent.id,
        pageId: this.selectedPlaceHolder.pageContent.pageId,
        title: this.selectedPlaceHolder.title,
        contentTypeId: this.selectedPlaceHolder.pageContent.contentTypeId,
        properties: properties,
        containerId: this.selectedPlaceHolder.pageContent.containerId,
        sortOrder: this.selectedPlaceHolder.sortOrder,
      };
      this._pageContentService.updatePageContent(pageContent).subscribe(response => {
        console.log(response);
        this.selectedPlaceHolder.isPropertyChanged = false;
        const alert: Alert = {
          alertType: AlertType.Success,
          message: 'Content properities have been saved successfully.',
          timeout: 5000
        }
        this._alertService.addAlert(alert);
      }, error => {
        console.log(error);
        const alert: Alert = {
          alertType: AlertType.Error,
          message: 'Cannot save the content properities,please contact administrator',
          timeout: 5000
        }
        this._alertService.addAlert(alert);
      });
    }
    else if (this.selectedPlaceHolder.layoutTemplate === "module") {
      let pageModule = this.selectedPlaceHolder.pageModule;

      pageModule.title = this.selectedPlaceHolder.title;
      pageModule.containerId = this.selectedPlaceHolder.pageModule.containerId;
      pageModule.sortOrder = this.selectedPlaceHolder.sortOrder;
      pageModule.properties = properties;

      this._pageModuleService.updatePageModule(pageModule).subscribe(response => {
        console.log(response);
        const alert: Alert = {
          alertType: AlertType.Success,
          message: 'Module properities have been saved successfully.',
          timeout: 5000
        }
        this._alertService.addAlert(alert);
      }, error => {
        console.log(error);
        const alert: Alert = {
          alertType: AlertType.Error,
          message: 'Cannot save the module properities,please contact administrator',
          timeout: 5000
        }
        this._alertService.addAlert(alert);
      });
    }
  }

  onDraft() {
    this._pageService.draftPage(this.currentPage.id).subscribe(response => {
      ;
      this.currentPageState = PageState.Draft;
      const alert: Alert = {
        alertType: AlertType.Success,
        message: 'The Page has been drafted.',
        timeout: 5000
      }
    }, error => {
      const alert: Alert = {
        alertType: AlertType.Error,
        message: 'Cannot draft the page, please contact the administrator.',
        timeout: 5000
      }
      this._alertService.addAlert(alert);
    });
  }

  onPublish() {
    if (this.currentPageState === PageState.Draft) {
      this._pageService.publishPage(this.currentPage.id).subscribe(response => {
        this.currentPageState = PageState.Published;
        const alert: Alert = {
          alertType: AlertType.Success,
          message: 'The Page has been published.',
          timeout: 5000
        }
      }, function (error) {
        const alert: Alert = {
          alertType: AlertType.Error,
          message: 'Cannot publish the page, please contact the administrator.',
          timeout: 5000
        }
        this._alertService.addAlert(alert);
      });
    }
  }

  private init() {
    const page$ = this._pageService.getPage(this.pageContext.currentPageId);
    const pageLayout$ = this._layoutService.getLayout(this.pageContext.layoutId);
    const contentTypes$ = this._contentTypeService.getContentTypes();
    const layoutTypes$ = this._layoutTypeService.getLayoutTypes();
    const moduleViews$ = this._moduleViewService.getModuleViews();
    const pageContents$ = this._pageContentService.getPageContents(this.pageContext.currentPageId, this.pageContext.currentLocale);
    const pageModules$ = this._pageModuleService.getPageModules(this.pageContext.currentPageId);

    forkJoin([page$, pageLayout$, contentTypes$, layoutTypes$, moduleViews$, pageContents$, pageModules$]).subscribe(results => {

      // Globals. .appSettings.roles.allUsers 

      //Assemble page
      this.currentPage = results[0];
      let permission = this.currentPage.pagePermissions.find(pp => pp.roleId === Globals.appSettings.roles.allUsers);
      if (permission) {
        this.currentPageState = PageState.Published;
      }
      else {
        this.currentPageState = PageState.Draft;
      }

      //Assemble pageLayout
      this.pageLayout = results[1];

      //Assemble contentTypes
      let contentTypes: ContentType[] = results[2];
      this.contentTypes = [];
      contentTypes.forEach(contentType => {
        this.contentTypes.push({
          type: 'content',
          contentType: contentType,
          properties: contentType.properties
        });
      });

      //Assemble layoutTypes
      this.layoutTypes = results[3];

      //Assemble moduleViews
      let moduleViews: ModuleView[] = results[4];
      this.moduleViews = [];
      moduleViews.forEach(moduleView => {
        this.moduleViews.push({
          type: "module",
          moduleView: moduleView,
          properties: moduleView.properties
        });
      });

      this.pageContents = results[5];

      this.pageModules = results[6];

      this.loadPageElements();

    }, error => {
      const alert: Alert = {
        alertType: AlertType.Error,
        message: 'Unable to get this item, please contact administrator',
        timeout: 5000
      }
      this._alertService.addAlert(alert);
    });
  }

  private loadPageElements() {

    let unAssignedContents = [],
      unAssignedModules = [];

    //First, position elements in correct order and then assign the pageLayout to VM.
    this.positionPageElements(this.pageLayout.placeHolders);
    // vm.pageLayout = pageLayout;
    this.pageLayout.pageId = this.pageContext.currentPageId;

    let unAssignedSrcConents = this.pageContents.filter(pageContent => {
      this.containerIds.indexOf(pageContent.containerId) === -1;
    });

    let unAssignedSrcModules = this.pageModules.filter(pageModule => {
      this.containerIds.indexOf(pageModule.containerId) === -1
    });

    unAssignedSrcConents.forEach(pageContent => {
      let content = this.getPageContentWithProperties(pageContent);
      content.isUnassigned = true;
      unAssignedContents.push(content);
    });

    unAssignedSrcModules.forEach(pageModule => {
      let index = pageModule.sortOrder - 1;
      let module = this.getPageModulesWithProperties(pageModule);
      module.isUnassigned = true;
      unAssignedModules.push(module);
    });

    this.uaLayout.placeHolders = [];
    this.uaLayout.placeHolders = this.uaLayout.placeHolders.concat(unAssignedContents);
    this.uaLayout.placeHolders = this.uaLayout.placeHolders.concat(unAssignedModules);

    this.root = {
      id: 'root',
      // layoutTemplate: 'repeater',
      layoutTypeId: '9341f92e-83d8-4afe-ad4a-a95deeda9ae3',
      type: 'container',
      layoutTemplate: 'container',
      placeHolders: this.pageLayout.placeHolders
    };

  }

  private positionPageElements(placeHolders: PlaceHolder[]) {
    if (placeHolders) {
      placeHolders.forEach(item => {
        //console.log(item)

        //adding containerId to filter unallocated items in a separate dndlist
        this.containerIds.push(item.id);

        //Load content items if found
        let pageContents = this.pageContents.filter(pageContent => pageContent.containerId === item.id);
        if (pageContents && pageContents.length > 0) {
          pageContents.forEach(pageContent => {
            let content = this.getPageContentWithProperties(pageContent);
            //item.placeHolders.splice(index, 0, contentTypeInfo); //Insert placeHolder into specified index
            if (!content.title) {
              content.title = content.contentType.label + ' ' + (item.placeHolders.length + 1);
            }
            item.placeHolders.push(content);
          });
        }

        item.layoutTemplate = this.getLayoutTemplate(item);
        //To sync and fetch the layout properties
        if (this.isLayoutType(item)) {
          this.syncPropertyForElement(item)
        }

        //Load modules if found
        let pageModules = this.pageModules.filter(pageModule => pageModule.containerId === item.id);
        if (pageModules && pageModules.length > 0) {
          pageModules.forEach(pageModule => {
            let index = pageModule.sortOrder - 1;
            let module = this.getPageModulesWithProperties(pageModule);
            if (!module.title) {
              module.title = module.moduleView.displayName + ' ' + (item.placeHolders.length + 1);
            }
            item.placeHolders.push(module);
          });
        }

        item.placeHolders = sortBy(item.placeHolders, ['sortOrder']);

        if (item.placeHolders) {
          this.positionPageElements(item.placeHolders);
        }
      });
    }
  }

  private getPageContentWithProperties(pageContent: PageContent): PlaceHolder {
    let propertiesValue = pageContent.properties;
    let masterContentType = this.contentTypes.find(element => element.contentType.id === pageContent.contentType.id);
    let properties = JSON.parse(JSON.stringify(masterContentType.properties));

    //Loading values to the properties
    properties.forEach(prop => {
      let propVal = propertiesValue.find(propVal => propVal.id === prop.id);
      if (propVal) {
        prop.value = propVal.value;
      }
      if (prop.optionList && prop.optionList.list) {
        prop.optionList = JSON.parse(JSON.stringify(prop.optionList.list));
      }
    });

    let content: PlaceHolder = {
      id: pageContent.id,
      // layoutTemplate: 'content',
      type: 'content',
      title: pageContent.title,
      properties: properties,
      pageContent: pageContent,
      contentType: pageContent.contentType,
      sortOrder: pageContent.sortOrder
    }
    return content;
  }

  private getPageModulesWithProperties(pageModule: PageModule): PlaceHolder {
    let propertiesValue = pageModule.properties;
    let masterModule = this.moduleViews.find(item => item.moduleView && item.moduleView.id === pageModule.moduleView.id);
    let properties = JSON.parse(JSON.stringify(masterModule.moduleView.properties));

    //Loading values to the properties
    properties.forEach(prop => {
      let propVal = propertiesValue.find(propVal => propVal.id === prop.id);
      if (propVal) {
        prop.value = propVal.value;
      }
      if (prop.optionList && prop.optionList.list) {
        prop.optionList = JSON.parse(prop.optionList.list);
      }
    });

    let module: PlaceHolder = {
      id: pageModule.id,
      // layoutTemplate: "module",
      type: "module",
      title: pageModule.title,
      moduleView: pageModule.moduleView,
      pageModule: pageModule,
      sortOrder: pageModule.sortOrder,
      properties: properties
    };

    return module;
  }

  //To sync and fetch the layout properties
  private syncPropertyForElement(element: PlaceHolder) {
    let propertiesValue = element.properties;
    let masterLayout = this.layoutTypes.find(layoutType => layoutType.id === element.layoutTypeId);
    let masterProperties = masterLayout.properties;
    masterProperties.forEach(prop => {
      if (prop) {
        let propVal = propertiesValue.find(propVal => propVal.id === prop.id);
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
          element.properties.push(JSON.parse(JSON.stringify(prop)));
        }
      }
    });
  }

  private getLayoutTemplate(placeHolder: PlaceHolder) {
    if (this.isLayoutType(placeHolder)) {
      return 'container';
    }
    return placeHolder.type;
  }

  private isLayoutType(placeHolder: PlaceHolder): boolean {
    return placeHolder.type !== 'module' && placeHolder.type !== 'content';
  }

  private updateElements(placeHolder: PlaceHolder) {

    // let elementsToSort = {
    //   contents: [],
    //   modules: []
    // };
    let contentContainer: PlaceHolder = JSON.parse(JSON.stringify(placeHolder));
    let moduleContainer: PlaceHolder = JSON.parse(JSON.stringify(placeHolder));
    contentContainer.placeHolders = [];
    moduleContainer.placeHolders = [];

    placeHolder.placeHolders.forEach((item, index) => {
      item.sortOrder = index + 1;
      if (item.layoutTemplate === "content") {
        contentContainer.placeHolders.push(item);
      }
      else if (item.layoutTemplate === "module") {
        moduleContainer.placeHolders.push(item);
      }
    });

    //updatePageContents(elementsToSort);

    //Clone current layout and get layout only (without contents and modules)
    let layoutOnly = JSON.parse(JSON.stringify(this.pageLayout));
    this.filterLayout(layoutOnly);
    console.log("--------------------------");
    console.log("Layout only");
    console.log(layoutOnly)

    let pageContents = this.parseAndSortPageContents(contentContainer);
    let pageModules = this.parseAndSortPageModules(moduleContainer);

    let pageContents$ = this._pageContentService.updatePageContents(pageContents);
    let pageModules$ = this._pageModuleService.updatePageModules(pageModules);
    let pageLayout$ = this._layoutService.updateLayout(layoutOnly)

    forkJoin([pageContents$, pageModules$, pageLayout$]).subscribe(result => {

      const alert: Alert = {
        alertType: AlertType.Success,
        message: 'Page Content and Module View in selected layout has been saved',
        timeout: 5000
      }
      this._alertService.addAlert(alert);

    });
  }

  private filterLayout(placeHolder: PlaceHolder) {

    placeHolder.placeHolders = reject(placeHolder.placeHolders, function (item) {
      return (item.layoutTemplate === "content" || item.layoutTemplate === "module");
    });

    placeHolder.placeHolders.forEach(item => {
      item.properties.forEach((prop, index) => {
        prop.optionList = null;
      });
      if (item.placeHolders) {
        this.filterLayout(item);
      }
    });
  }

  private parseAndSortPageContents(placeHolder: PlaceHolder): PageContent[] {
    let pageContents: PageContent[] = [];
    placeHolder.placeHolders.forEach((item, index) => {
      let pageContent: PageContent;
      if (item.id) {
        pageContent = item.pageContent;
        pageContent.sortOrder = index + 1;
        pageContent.containerId = placeHolder.id;
        pageContent.properties = item.properties;
      }
      else {
        pageContent = {
          id: Guid.newGuid(),
          pageId: this.pageContext.currentPageId,
          contentTypeId: item.contentType.id,
          containerId: placeHolder.id,
          sortOrder: index + 1,
          hasEditPermission: true, //New content always has edit permission
          properties: item.properties
        }
      }
      pageContents.push(pageContent);
    });
    return pageContents;
  }

  private parseAndSortPageModules(placeHolder: PlaceHolder): PageModule[] {
    let pageModules: PageModule[] = [];
    placeHolder.placeHolders.forEach((item, index) => {
      let pageModule: PageModule;
      if (item.id) {
        pageModule = item.pageModule;
        pageModule.sortOrder = index + 1;
        pageModule.containerId = placeHolder.id;
        pageModule.properties = item.properties;
      }
      else {
        pageModule = {
          id: Guid.newGuid(),
          pageId: this.pageContext.currentPageId,
          moduleId: item.moduleView.moduleId,
          moduleViewId: item.moduleView.id,
          containerId: placeHolder.id,
          sortOrder: index + 1,
          hasEditPermission: true, //New content always has edit permission
          properties: item.properties
        }
      }
      pageModules.push(pageModule);
    });
    return pageModules;
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
