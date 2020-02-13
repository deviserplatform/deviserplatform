import { BrowserModule } from '@angular/platform-browser';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { AlertModule } from 'ngx-bootstrap/alert';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { InlineSVGModule } from 'ng-inline-svg';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginationModule } from 'ngx-bootstrap/pagination';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdminGridComponent } from './admin-grid/admin-grid.component';
import { MessagesComponent } from './messages/messages.component';
import { AdminFormComponent } from './admin-form/admin-form.component';
import { RecordIdPipe } from './common/pipes/record-id.pipe';
import { FormControlComponent } from './form-control/form-control.component';
import { FormatFieldPipe } from './common/pipes/format-field.pipe';
import { ConfirmDialogComponent } from './common/components/confirm-dialog/confirm-dialog.component';
import { ValidationErrorComponent } from './validation-error/validation-error.component';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { EntityFormComponent } from './entity-form/entity-form.component';
import { ChildGridComponent } from './child-grid/child-grid.component';
import { CheckboxListComponent } from './common/components/checkbox-list/checkbox-list.component';
import { AdminAlertComponent } from './common/components/admin-alert/admin-alert.component';
import { WINDOW_PROVIDERS } from './common/services/window.service';
import { EnvPathPipe } from './common/pipes/env-path.pipe';


@NgModule({
  declarations: [
    AppComponent,
    AdminFormComponent,
    AdminGridComponent,
    ConfirmDialogComponent,
    FormControlComponent,
    FormatFieldPipe,
    MessagesComponent,
    RecordIdPipe,
    ValidationErrorComponent,
    EntityFormComponent,
    ChildGridComponent,
    CheckboxListComponent,
    AdminAlertComponent,
    EnvPathPipe
  ],
  imports: [
    AlertModule.forRoot(),
    AppRoutingModule,
    BrowserModule,
    BsDatepickerModule.forRoot(),
    CKEditorModule,
    FormsModule,
    HttpClientModule,
    InlineSVGModule.forRoot(),
    ModalModule.forRoot(),
    NgSelectModule,
    PaginationModule.forRoot(),
    ReactiveFormsModule
  ],
  providers: [
    DatePipe,
    RecordIdPipe,
    WINDOW_PROVIDERS
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
