import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DynamicService } from '../../core/dynamic.service';
import { SignalRService } from '../../core/signalr.service';
import { AuthService } from '../../core/auth.service';
import { Session } from '../../core/models/session.model';
import { UserService } from '../../core/models/user-service.model';
import { Subscription } from 'rxjs';
import { Reservation } from '../../core/models/reservation.model';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-reservation',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './reservation.component.html',
  styleUrl: './reservation.component.css',
})
export class ReservationComponent implements OnInit, OnDestroy {
  sessions: Session[] = [];
  userServices: UserService[] = [];
  userReservations: Reservation[] = [];
  userId = '';
  private capacitySubscription?: Subscription;

  constructor(
    private dynamicService: DynamicService,
    private signalRService: SignalRService,
    private authService: AuthService,
    private translate: TranslateService
  ) {}

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe((user) => {
      if (user?.locationId) {
        this.userId = user.id;
        this.loadSessions();
        this.loadUserServices();
        this.loadUserReservations();
      }
    });
    this.signalRService.startConnection();

    this.capacitySubscription = this.signalRService.capacityUpdate$.subscribe(
      ({ sessionId, newCapacity }) => {
        const session = this.sessions.find((s) => s.id === sessionId);
        if (session) {
          session.currentCapacity = newCapacity;
        }
      }
    );
  }

  ngOnDestroy(): void {
    this.capacitySubscription?.unsubscribe();
  }

  loadSessions(): void {
    const locationId = this.authService.getLocationId();
    if (!locationId) {
      return;
    }

    this.dynamicService
      .getByPath<Session[]>(`sessions`)
      .subscribe({
        next: (data) =>
          (this.sessions = data.filter(
            (s) => s.locationId == locationId
          )),
        error: (err) => console.error('Failed to load sessions', err),
      });
  }

  loadUserServices(): void {
    this.dynamicService
      .getById<UserService[]>("user-services/user", this.userId)
      .subscribe({
        next: (data) =>
          (this.userServices = data.filter((us) => us.remainingSessions > 0)),
        error: (err) => console.error('Failed to load user services', err),
      });
  }

  loadUserReservations(): void {
    this.dynamicService.getById<Reservation[]>('reservations/user', this.userId).subscribe({
      next: (data) => this.userReservations = data,
      error: (err) => console.error('Failed to load reservations from the user', err)
    });
  }

  isSessionFull(session: Session): boolean {
    return session.currentCapacity >= session.maxCapacity;
  }

  hasUserService(session: Session): boolean {
    return this.userServices.some(
      (us) => us.serviceId === session.serviceId && us.remainingSessions > 0
    );
  }

  isAlreadyReserved(session: Session): boolean {
    return this.userReservations.some(ur => ur.sessionId === session.id);
  }

  canReserve(session: Session): boolean {
    return !this.isSessionFull(session) && this.hasUserService(session) && !this.isAlreadyReserved(session);
  }

  getCannotReserveReason(session: Session): string {
    if (this.isAlreadyReserved(session)) return 'RESERVATION.ALREADY_RESERVED';
    if (this.isSessionFull(session)) return 'RESERVATION.SESSION_FULL';
    if (!this.hasUserService(session)) return 'RESERVATION.NO_SERVICE';
    return 'RESERVATION.UNAVAILABLE_GENERIC';
  }

  getUserServiceForSession(session: Session): UserService | undefined {
    return this.userServices.find(
      (us) => us.serviceId === session.serviceId && us.remainingSessions > 0
    );
  }

  makeReservation(session: Session): void {
    const userService = this.getUserServiceForSession(session);

    if (!userService) {
      alert(this.translate.instant('RESERVATION.NO_AVAILABLE_SESSIONS_FOR_SERVICE'));
      return;
    }

    if (!this.canReserve(session)) {
      alert(this.translate.instant(this.getCannotReserveReason(session)));
      return;
    }

    const reservation = {
      sessionId: session.id,
      userId: this.userId,
      userServiceId: userService.id,
    };

    this.dynamicService.create('reservations', reservation).subscribe({
      next: () => {
        alert(this.translate.instant('RESERVATION.SUCCESS'));
        this.loadSessions();
        this.loadUserServices();
        this.loadUserReservations();
      },
      error: (err) => {
        console.error('Failed to create reservation', err);
        alert(this.translate.instant('RESERVATION.CREATE_FAILED'));
      },
    });
  }

  getCapacityPercentage(session: Session): number {
    return (session.currentCapacity / session.maxCapacity) * 100;
  }

  getCapacityClass(session: Session): string {
    const percentage = this.getCapacityPercentage(session);
    if (percentage >= 90) return 'bg-red-500';
    if (percentage >= 70) return 'bg-yellow-500';
    return 'bg-green-500';
  }

  getRemainingSessionsForService(serviceId: string): number {
    const userService = this.userServices.find(
      (us) => us.serviceId === serviceId
    );
    return userService?.remainingSessions ?? 0;
  }
}
