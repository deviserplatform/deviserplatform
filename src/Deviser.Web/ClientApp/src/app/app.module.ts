import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AccordionModule } from 'ngx-bootstrap/accordion';
import { AlertModule } from 'ngx-bootstrap/alert';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ImageCropperModule } from 'ngx-image-cropper';
import { InlineSVGModule } from 'ng-inline-svg-2';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgOptionHighlightModule } from '@ng-select/ng-option-highlight';
import { QuillModule } from 'ngx-quill';

import { DeviserSharedModule } from 'deviser-shared';
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
    DeviserSharedModule,
    DragDropModule,
    FormsModule,
    HttpClientModule,
    ImageCropperModule,
    InlineSVGModule.forRoot(),
    ModalModule.forRoot(), 
    NgSelectModule,
    NgOptionHighlightModule,
    QuillModule.forRoot(),
    ReactiveFormsModule
  ],
  // providers: [WINDOW_PROVIDERS],
  bootstrap: [AppComponent]
})
export class AppModule { }
