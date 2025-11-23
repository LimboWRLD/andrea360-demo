import { Injectable } from '@angular/core';
import { DynamicService } from './dynamic.service';
import { AuthService } from './auth.service';
import { map, forkJoin, Observable, shareReplay } from 'rxjs';

export interface AdminStats {
  locationCount: number;
  activeServicesCount: number;
  reservationsToday: number;
  sessionsToday: number;
  topServiceWeekName?: string;
  revenueToday: number;
  newMembersToday: number;
}
export interface EmployeeStats {
  nextSessionTime?: string;
  remainingSessionsToday: number;
  lastReservationsCount: number;
  topEmployeeServiceName?: string;
  todaysSchedule: any[];
}
export interface MemberStats {
  nextReservationDate?: string;
  totalReservations: number;
  loyaltyPoints: number;
  recommendedServices: string[];
  purchaseHistory: string[];
}

@Injectable({ providedIn: 'root' })
export class DashboardDataService {
  private todayISO = new Date().toISOString().substring(0,10);
  constructor(private api: DynamicService, private auth: AuthService) {}

  private adminCache$?: Observable<AdminStats>;
  getAdminStats(): Observable<AdminStats> {
    if (this.adminCache$) return this.adminCache$;
    const today = this.todayISO;
    this.adminCache$ = forkJoin({
      locations: this.api.getAll<any[]>('locations'),
      services: this.api.getAll<any[]>('services'),
      reservations: this.api.getAll<any[]>('reservations'),
      sessions: this.api.getAll<any[]>('sessions'),
      transactions: this.api.getAll<any[]>('transactions').pipe(map(t => t || [])),
      users: this.api.getAll<any[]>('users')
    }).pipe(
      map(({locations, services, reservations, sessions, transactions, users}) => {
        const reservationsToday = reservations.filter(r => (r.reservedAt||'').startsWith(today)).length;
        const sessionsToday = sessions.filter(s => (s.startTime||'').startsWith(today)).length;
        const weekStart = new Date(); weekStart.setDate(weekStart.getDate()-7);
        const weekStr = weekStart.toISOString().substring(0,10);
        const serviceMap: Record<string, number> = {};
        sessions.filter(s => (s.startTime||'') >= weekStr).forEach(s => { serviceMap[s.serviceName] = (serviceMap[s.serviceName]||0)+1; });
        const topServiceWeekName = Object.entries(serviceMap).sort((a,b)=>b[1]-a[1])[0]?.[0];
        const revenueToday = transactions.filter(t => (t.createdAt||'').startsWith(today)).reduce((sum,t)=>sum+(t.amount||0),0);
        const newMembersToday = users.filter(u => (u.createdAtTimestamp && new Date(u.createdAtTimestamp).toISOString().substring(0,10)===today)).length;
        return {
          locationCount: locations.length,
          activeServicesCount: services.length,
          reservationsToday,
          sessionsToday,
          topServiceWeekName,
          revenueToday,
          newMembersToday
        };
      }),
      shareReplay(1)
    );
    return this.adminCache$;
  }

  getEmployeeStats(): Observable<EmployeeStats> {
    const today = this.todayISO;
    return forkJoin({
      sessions: this.api.getAll<any[]>('sessions'),
      reservations: this.api.getAll<any[]>('reservations')
    }).pipe(map(({sessions, reservations}) => {
      const todaysSchedule = sessions.filter(s => (s.startTime||'').startsWith(today));
      const upcoming = todaysSchedule.slice().sort((a,b)=>a.startTime.localeCompare(b.startTime))[0];
      const mapSvc: Record<string, number> = {};
      todaysSchedule.forEach(s => mapSvc[s.serviceName] = (mapSvc[s.serviceName]||0)+1);
      const topEmployeeServiceName = Object.entries(mapSvc).sort((a,b)=>b[1]-a[1])[0]?.[0];
      const lastReservationsCount = reservations.filter(r => (r.reservedAt||'').startsWith(today)).length;
      return {
        nextSessionTime: upcoming?.startTime,
        remainingSessionsToday: todaysSchedule.length,
        lastReservationsCount,
        topEmployeeServiceName,
        todaysSchedule
      };
    }));
  }

  getMemberStats(userId: string): Observable<MemberStats> {
    const today = this.todayISO;
    return forkJoin({
      reservations: this.api.getAll<any[]>('reservations'),
      userServices: this.api.getAll<any[]>('user-services'),
      services: this.api.getAll<any[]>('services')
    }).pipe(map(({reservations, userServices, services}) => {
      const mine = reservations.filter(r => r.userId === userId);
      const totalReservations = mine.length;
      const upcoming = mine.filter(r => !r.isCancelled).sort((a,b)=>a.reservedAt.localeCompare(b.reservedAt))[0];
      console.log(userServices);
      
      const myServices = userServices.filter(x => x.userId === userId);
      const purchaseHistory = myServices.map(x => x.serviceName || 'Usluga');
      const loyaltyPoints = myServices.reduce((sum,x)=>sum+(x.remainingSessions||0),0);
      const recommendedServices = services.filter(s => !purchaseHistory.includes(s.name)).slice(0,3).map(s=>s.name);
      return {
        nextReservationDate: upcoming?.reservedAt,
        totalReservations,
        loyaltyPoints,
        recommendedServices,
        purchaseHistory
      };
    }));
  }
}
