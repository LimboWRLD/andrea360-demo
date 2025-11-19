import { Routes } from '@angular/router';

export const routes: Routes = [
    {path:'', loadChildren: ()=> import('./admin/admin/admin-routing.module').then(m => m.AdminRoutingModule)}
];
