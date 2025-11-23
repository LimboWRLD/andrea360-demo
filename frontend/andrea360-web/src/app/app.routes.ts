import { Routes } from '@angular/router';

export const routes: Routes = [
    {path:'', loadChildren: ()=> import('./admin/admin/admin-routing.module').then(m => m.AdminRoutingModule)},
    {path:'', loadChildren: ()=> import('./employee/employee/employee-routing.module').then(m => m.EmployeeRoutingModule)},
    {path:'', loadChildren: ()=> import('./member/member/member-routing.module').then(m => m.MemberRoutingModule)},
];
