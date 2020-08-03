import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AlertModule } from 'ngx-bootstrap/alert';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { ImageCropperModule } from 'ngx-image-cropper';
import { InlineSVGModule } from 'ng-inline-svg';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgSelectModule } from '@ng-select/ng-select';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EditComponent } from './common/components/edit/edit.component';
import { LayoutComponent } from './common/components/layout/layout.component';
import { WINDOW_PROVIDERS } from './common/services/window.service';
import { EnvPathPipe } from './common/pipes/env-path.pipe';
import { ConfirmDialogComponent } from './common/components/confirm-dialog/confirm-dialog.component';
import { DevAlertComponent } from './common/components/alert/dev-alert.component';
import { EditPermissionComponent } from './common/components/edit-permission/edit-permission.component';
import { EditContentComponent } from './common/components/edit-content/edit-content.component';
import { PreviewContentComponent } from './common/components/preview-content/preview-content.component';
import { SanitizeHtmlPipe } from './common/pipes/sanitize-html.pipe';
import { AttachmentComponent } from './common/components/attachment/attachment.component';
import { LinkComponent } from './common/components/link/link.component';
import { ImageComponent } from './common/components/image/image.component';
import { ImageSelectorComponent } from './common/components/image-selector/image-selector.component';
import { DndUploadDirective } from './common/directives/dnd-upload.directive';
import { EditLinkComponent } from './common/components/edit-link/edit-link.component';



@NgModule({
  declarations: [
    AppComponent,
    EditComponent,
    LayoutComponent,
    EnvPathPipe,
    ConfirmDialogComponent,
    DevAlertComponent,
    EditPermissionComponent,
    EditContentComponent,
    PreviewContentComponent,
    SanitizeHtmlPipe,
    AttachmentComponent,
    LinkComponent,
    ImageComponent,
    ImageSelectorComponent,
    DndUploadDirective,
    EditLinkComponent
  ],
  imports: [
    AlertModule.forRoot(),
    AppRoutingModule,
    BrowserModule,
    BsDatepickerModule.forRoot(),
    CKEditorModule,
    DragDropModule,
    HttpClientModule,
    ImageCropperModule, 
    InlineSVGModule.forRoot(),
    NgSelectModule,
    FormsModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
  ],
  providers: [WINDOW_PROVIDERS],
  bootstrap: [AppComponent]
})
export class AppModule { }
