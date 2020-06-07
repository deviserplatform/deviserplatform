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

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditComponent implements OnInit {

  alerts: Alert[] = [];
  currentPage: Page;
  currentPageState: string;
  contentTypes: PageElement[];
  layoutTypes: LayoutType[];
  moduleViews: PageElement[];
  pageContents: PageContent[];
  pageContext: PageContext;
  pageLayout: PageLayout;
  pageModules: PageModule[];

  constructor(private _alertService: AlertService,
    private _contentTypeService: ContentTypeService,
    private _layoutService: LayoutService,
    private _layoutTypeService: LayoutTypeService,
    private _moduleViewService: ModuleViewService,
    private _pageService: PageService,
    private _pageContentService: PageContentService,
    private _pageModuleService: PageModuleService,
    @Inject(WINDOW) window: any) {
    this.pageContext = window.pageContext;
    this._alertService.alerts.subscribe(alert => {
      if (alert) {
        this.alerts.push(alert)
      }
    });
  }

  ngOnInit(): void {
  }

  init() {
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
      if (permission)
        this.currentPageState = "Published";
      else
        this.currentPageState = "Publish";

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
    positionPageElements(pageLayout.placeHolders);
    vm.pageLayout = pageLayout;
    vm.pageLayout.pageId = vm.currentPage.id;

    var unAssignedSrcConents = _.reject(vm.pageContents, function (content) {
      return _.includes(containerIds, content.containerId);
    });

    var unAssignedSrcModules = _.reject(vm.pageModules, function (module) {
      return _.includes(containerIds, module.containerId);
    });

    _.forEach(unAssignedSrcConents, function (pageContent) {
      var content = getPageContentWithProperties(pageContent);
      content.isUnassigned = true;
      unAssignedContents.push(content);
    });

    _.forEach(unAssignedSrcModules, function (pageModule) {
      var index = pageModule.sortOrder - 1;
      var module = getPageModulesWithProperties(pageModule);
      module.isUnassigned = true;
      unAssignedModules.push(module);
    })

    vm.uaLayout.placeHolders = [];

    vm.uaLayout.placeHolders = vm.uaLayout.placeHolders.concat(unAssignedContents);
    vm.uaLayout.placeHolders = vm.uaLayout.placeHolders.concat(unAssignedModules);

  }

  private positionPageElements(placeHolders: Placeholder[]) {
    if (placeHolders) {
      placeHolders.forEach(item => {
        //console.log(item)

        //adding containerId to filter unallocated items in a separate dndlist
        containerIds.push(item.id);

        //Load content items if found
        var pageContents = _.filter(vm.pageContents, { containerId: item.id });
        if (pageContents) {
          _.forEach(pageContents, function (pageContent) {
            var content = getPageContentWithProperties(pageContent);
            //item.placeHolders.splice(index, 0, contentTypeInfo); //Insert placeHolder into specified index
            if (!content.title) {
              content.title = content.contentType.label + ' ' + (item.placeHolders.length + 1);
            }
            item.placeHolders.push(content);
          });
        }

        //To sync and fetch the layout properties
        if (item.layoutTemplate !== 'content' && item.layoutTemplate !== 'module') {
          syncPropertyForElement(item)
        }

        //Load modules if found
        var pageModules = _.filter(vm.pageModules, { containerId: item.id });
        if (pageModules) {
          _.forEach(pageModules, function (pageModule) {
            var index = pageModule.sortOrder - 1;
            var module = getPageModulesWithProperties(pageModule);
            if (!module.title) {
              module.title = module.moduleView.displayName + ' ' + (item.placeHolders.length + 1);
            }
            item.placeHolders.push(module);
          });
        }

        item.placeHolders = _.sortBy(item.placeHolders, ['sortOrder']);

        if (item.placeHolders) {
          positionPageElements(item.placeHolders);
        }
      });
    }
  }

}
