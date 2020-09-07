import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AccordionModule } from 'ngx-bootstrap/accordion';
import { AlertModule } from 'ngx-bootstrap/alert';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { ImageCropperModule } from 'ngx-image-cropper';
import { InlineSVGModule } from 'ng-inline-svg';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgSelectModule } from '@ng-select/ng-select';

import { DeviserSharedModule } from 'deviser-shared';
// import { EnvPathPipe } from 'deviser-shared';
// import { ConfirmDialogComponent } from 'deviser-shared';
// import { DevAlertComponent } from 'deviser-shared';
// import { SanitizeHtmlPipe } from 'deviser-shared';
// import { LinkComponent } from 'deviser-shared';
// import { ImageComponent } from 'deviser-shared';
// import { ImageSelectorComponent } from 'deviser-shared';
// import { DndUploadDirective } from 'deviser-shared';
// import { EditLinkComponent } from 'deviser-shared';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EditComponent } from './common/components/edit/edit.component';
import { LayoutComponent } from './common/components/layout/layout.component';
import { EditPermissionComponent } from './common/components/edit-permission/edit-permission.component';
import { EditContentComponent } from './common/components/edit-content/edit-content.component';
import { PreviewContentComponent } from './common/components/preview-content/preview-content.component';




@NgModule({
  declarations: [
    AppComponent,
    EditComponent,
    LayoutComponent,
   
    EditPermissionComponent,
    EditContentComponent,
    PreviewContentComponent    
  ],
  imports: [
    AccordionModule.forRoot(),
    AlertModule.forRoot(),
    AppRoutingModule,
    BrowserAnimationsModule,
    BrowserModule,
    BsDatepickerModule.forRoot(),
    CKEditorModule,
    DeviserSharedModule,
    // ConfirmDialogComponent,
    // DevAlertComponent,
    DragDropModule,
    // DndUploadDirective,
    // EditLinkComponent,
    // EnvPathPipe,
    FormsModule,
    HttpClientModule,
    // ImageComponent,
    ImageCropperModule, 
    // ImageSelectorComponent,
    InlineSVGModule.forRoot(),
    // LinkComponent,
    ModalModule.forRoot(), 
    NgSelectModule,    
    ReactiveFormsModule,
    // SanitizeHtmlPipe
  ],
  // providers: [WINDOW_PROVIDERS],
  bootstrap: [AppComponent]
})
export class AppModule { }
