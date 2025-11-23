import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../core/auth.service';
import { AsyncPipe, CommonModule } from '@angular/common';
import { DashboardDataService, AdminStats, EmployeeStats, MemberStats } from '../../../core/dashboard-data.service';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, AsyncPipe, TranslatePipe],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  // Admin stats
  locationCount?: number;
  activeServicesCount?: number;
  reservationsToday?: number;
  sessionsToday?: number;
  topServiceWeekName?: string;
  revenueToday?: number;
  newMembersToday?: number;

  // Employee stats
  nextSessionTime?: string;
  remainingSessionsToday?: number;
  lastReservationsCount?: number;
  topEmployeeServiceName?: string;
  todaysSchedule: any[] = [];

  // Member stats
  nextReservationDate?: string;
  totalReservations?: number;
  loyaltyPoints?: number;
  recommendedServices: string[] = [];
  purchaseHistory: string[] = [];

  constructor(public auth: AuthService, private data: DashboardDataService) {}

  ngOnInit(): void {
    this.auth.getCurrentUser().subscribe(u => {
      if (!u) return;
      if (this.auth.hasRole('admin')) {
        this.data.getAdminStats().subscribe((s: AdminStats) => {
          this.locationCount = s.locationCount;
          this.activeServicesCount = s.activeServicesCount;
          this.reservationsToday = s.reservationsToday;
          this.sessionsToday = s.sessionsToday;
          this.topServiceWeekName = s.topServiceWeekName;
          this.revenueToday = s.revenueToday;
          this.newMembersToday = s.newMembersToday;
        });
      } else if (this.auth.hasRole('employee')) {
        this.data.getEmployeeStats().subscribe((s: EmployeeStats) => {
          this.nextSessionTime = s.nextSessionTime;
          this.remainingSessionsToday = s.remainingSessionsToday;
          this.lastReservationsCount = s.lastReservationsCount;
          this.topEmployeeServiceName = s.topEmployeeServiceName;
          this.todaysSchedule = s.todaysSchedule;
        });
      } else if (this.auth.hasRole('member')) {
        this.data.getMemberStats(u.id).subscribe((s: MemberStats) => {
          this.nextReservationDate = s.nextReservationDate;
          this.totalReservations = s.totalReservations;
          this.loyaltyPoints = s.loyaltyPoints;
          this.recommendedServices = s.recommendedServices;
          this.purchaseHistory = s.purchaseHistory;
        });
      }
    });
  }
}
