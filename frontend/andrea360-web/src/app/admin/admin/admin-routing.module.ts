import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LocationComponent } from '../location/location.component';
import { EmployeeComponent } from '../employee/employee.component';
import { DashboardComponent } from '../../shared/components/dashboard/dashboard.component';

const routes: Routes = [
  {path:'', component: DashboardComponent},
  {path:"locations", component:LocationComponent},
  {path:"employees", component:EmployeeComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
