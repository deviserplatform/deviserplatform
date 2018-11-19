import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminGridComponent } from './admin-grid/admin-grid.component';
import { AdminFormComponent } from './admin-form/admin-form.component';

const routes: Routes = [
  { path: '', redirectTo: '/list', pathMatch: 'full' },
  { path: 'list', component: AdminGridComponent },
  { path: 'detail/:id', component: AdminFormComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
