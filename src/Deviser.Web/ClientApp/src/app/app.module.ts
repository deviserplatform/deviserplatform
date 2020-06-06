import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { InlineSVGModule } from 'ng-inline-svg';
import { NgSelectModule } from '@ng-select/ng-select';
import { AlertModule } from 'ngx-bootstrap/alert';
import { ModalModule } from 'ngx-bootstrap/modal';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EditComponent } from './common/components/edit/edit.component';
import { LayoutComponent } from './common/components/layout/layout.component';
import { WINDOW_PROVIDERS } from './common/services/window.service';
import { EnvPathPipe } from './common/pipes/env-path.pipe';
import { ConfirmDialogComponent } from './common/components/confirm-dialog/confirm-dialog.component';
import { DevAlertComponent } from './common/components/alert/dev-alert.component';


@NgModule({
  declarations: [
    AppComponent,
    EditComponent,
    LayoutComponent,
    EnvPathPipe,
    ConfirmDialogComponent,
    DevAlertComponent
  ],
  imports: [
    AlertModule.forRoot(),
    AppRoutingModule,
    BrowserModule,    
    DragDropModule,
    HttpClientModule,
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
