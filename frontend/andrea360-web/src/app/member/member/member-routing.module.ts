import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ShopComponent } from '../shop/shop.component';
import { ReservationComponent } from '../reservation/reservation.component';

const routes: Routes = [
  {path:'shop', component: ShopComponent},
  {path:'booking', component: ReservationComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MemberRoutingModule { }
