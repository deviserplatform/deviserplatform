import { BrowserModule } from '@angular/platform-browser';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';


import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { InlineSVGModule } from 'ng-inline-svg';
import { ModalModule } from 'ngx-bootstrap/modal';

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


@NgModule({
  declarations: [
    AppComponent,
    AdminFormComponent,
    AdminGridComponent,
    FormControlComponent,
    MessagesComponent,
    RecordIdPipe,
    FormatFieldPipe,
    ConfirmDialogComponent,
    ValidationErrorComponent
  ],
  imports: [
    AppRoutingModule,
    FormsModule,
    BrowserModule,
    HttpClientModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    InlineSVGModule.forRoot(),
    ReactiveFormsModule
  ],
  providers: [
    DatePipe,
    RecordIdPipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
