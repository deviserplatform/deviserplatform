import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { InlineSVGModule } from 'ng-inline-svg';
import { NgSelectModule } from '@ng-select/ng-select';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EditComponent } from './common/components/edit/edit.component';
import { LayoutComponent } from './common/components/layout/layout.component';
import { WINDOW_PROVIDERS } from './common/services/window.service';
import { EnvPathPipe } from './common/pipes/env-path.pipe';


@NgModule({
  declarations: [
    AppComponent,
    EditComponent,
    LayoutComponent,
    EnvPathPipe
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,    
    DragDropModule,
    HttpClientModule,
    InlineSVGModule.forRoot(),
    NgSelectModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [WINDOW_PROVIDERS],
  bootstrap: [AppComponent]
})
export class AppModule { }
