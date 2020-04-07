import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminGridComponent } from './admin-grid/admin-grid.component';
import { AdminFormComponent } from './admin-form/admin-form.component';
import { AdminTreeComponent } from './admin-tree/admin-tree.component';

const routes: Routes = [
  // { path: '', redirectTo: '/list', pathMatch: 'full' },
  { path: 'list', component: AdminGridComponent },
  { path: 'detail/:id', component: AdminFormComponent },
  { path: 'tree', component: AdminTreeComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {useHash:true})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
