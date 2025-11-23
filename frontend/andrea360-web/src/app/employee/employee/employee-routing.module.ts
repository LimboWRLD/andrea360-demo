import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MembersComponent } from '../members/members.component';
import { ServicesComponent } from '../services/services.component';
import { ScheduleComponent } from '../schedule/schedule.component';
import { SessionDetailsComponent } from '../session-details/session-details.component';

const routes: Routes = [
  {path: 'members', component: MembersComponent},
  {path: 'services', component: ServicesComponent},
  {path: 'schedule', component: ScheduleComponent,},
  {path: 'schedule/session/:id', component: SessionDetailsComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeRoutingModule { }
