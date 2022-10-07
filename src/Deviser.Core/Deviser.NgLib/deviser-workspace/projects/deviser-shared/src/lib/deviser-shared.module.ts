import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AlertModule } from 'ngx-bootstrap/alert';
import { NgSelectModule } from '@ng-select/ng-select';
import { ImageCropperModule } from 'ngx-image-cropper';
import { InlineSVGModule } from 'ng-inline-svg-2';
import { ModalModule } from 'ngx-bootstrap/modal';

import { WINDOW_PROVIDERS } from './services/window.service';
import { EnvPathPipe } from './pipes/env-path.pipe';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { DevAlertComponent } from './components/alert/dev-alert.component';
import { SanitizeHtmlPipe } from './pipes/sanitize-html.pipe';
import { LinkComponent } from './components/link/link.component';
import { ImageComponent } from './components/image/image.component';
import { ImageSelectorComponent } from './components/image-selector/image-selector.component';
import { DndUploadDirective } from './directives/dnd-upload.directive';
import { EditLinkComponent } from './components/edit-link/edit-link.component';


@NgModule({
  declarations: [EnvPathPipe,
    ConfirmDialogComponent,
    DevAlertComponent,
    SanitizeHtmlPipe,
    LinkComponent,
    ImageComponent,
    ImageSelectorComponent,
    DndUploadDirective,
    EditLinkComponent,
  ],
  imports: [
    AlertModule.forRoot(),
    BrowserModule,
    DragDropModule,
    HttpClientModule,
    ImageCropperModule,
    InlineSVGModule.forRoot(),
    NgSelectModule,
    FormsModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
  ],
  exports: [    
    EnvPathPipe,
    ConfirmDialogComponent,
    DevAlertComponent,
    SanitizeHtmlPipe,
    LinkComponent,
    ImageComponent,
    ImageSelectorComponent,
    DndUploadDirective,
    EditLinkComponent
  ],
  providers: [WINDOW_PROVIDERS]
})
export class DeviserSharedModule { }