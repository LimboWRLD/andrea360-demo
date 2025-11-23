import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Session } from '../../core/models/session.model';
import { DynamicService } from '../../core/dynamic.service';
import { Reservation } from '../../core/models/reservation.model';
import {
  DataTableComponent,
  TableColumn,
} from '../../shared/components/data-table/data-table.component';
import {
  EntityFormComponent,
  FormField,
} from '../../shared/components/entity-form/entity-form.component';
import { User } from '../../core/models/user.model';
import { AuthService } from '../../core/auth.service';
import { CommonModule, Location } from '@angular/common';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-session-details',
  imports: [
    CommonModule,
    TranslatePipe,
    DataTableComponent,
    EntityFormComponent,
    RouterLink
  ],
  templateUrl: './session-details.component.html',
  styleUrl: './session-details.component.css',
})
export class SessionDetailsComponent implements OnInit {
  sessionLoading = true;
  showForm = false;
  selectedReservation: Reservation | null = null;
  session: Session | null = null;
  reservations: Reservation[] = [];
  users: User[] = [];

  constructor(
    private http: DynamicService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private location: Location
  ) {}
  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;

    this.authService.getCurrentUser().subscribe((user) => {
      if (!user?.locationId) return;

      this.loadSession(id, () => {
        this.loadReservations(id);
      });
    });
  }

  loadSession(id: string, callback?: () => void) {
    this.http.getById<Session>('sessions', id).subscribe({
      next: (session) => {
        this.session = session;
        this.sessionLoading = false;
        callback?.();
      },
      error: (err) => console.error(err),
    });
  }

  loadReservations(sessionId: string): void {
    this.http
      .getById<Reservation[]>('reservations/session', sessionId)
      .subscribe((data) => (this.reservations = data));
  }

  reservationColumns: TableColumn[] = [
    {
      key: 'reservedAt',
      label: 'RESERVATION.FIELDS.RESERVED_AT',
      render: (v) => new Date(v).toLocaleString(),
    },
    { key: 'userName', label: 'RESERVATION.FIELDS.MEMBER_EMAIL' },
    {
      key: 'isCancelled',
      label: 'RESERVATION.FIELDS.CANCELED',
      render: (v) => (v ? 'Yes' : 'No'),
    },
  ];

  reservationFields: FormField[] = [
    {
      name: 'reservedAt',
      label: 'SESSION.FIELDS.END_TIME',
      type: 'datetime-local',
      required: true,
    },
    {
      name: 'isCancelled',
      label: 'MEMBER.FIELDS.ENABLED',
      type: 'switch',
      defaultValue: false,
    },
  ];

  openForm(reservation?: any) {
    this.selectedReservation = reservation || null;
    this.showForm = true;
  }

  closeForm() {
    this.showForm = false;
    this.selectedReservation = null;
  }

  private convertToUtc(dateString: string): string {
    const local = new Date(dateString);
    return new Date(
      local.getTime() - local.getTimezoneOffset() * 60000
    ).toISOString();
  }

  saveFromEntityForm(formValue: any) {
    const payload = {
      ...formValue,
      reservedAt: this.convertToUtc(formValue.reservedAt),
      userId: this.selectedReservation?.userId,
      sessionId: this.session?.id,
    };

    if (this.selectedReservation) {
      this.http
        .update('reservations', this.selectedReservation.id, payload)
        .subscribe(() => {
          this.loadReservations(this.session!.id);
          this.closeForm();
        });
    } else {
      this.http.create('reservations', payload).subscribe(() => {
        this.loadReservations(this.session!.id);
        this.closeForm();
      });
    }
  }

  deleteReservation(id: string): void {
    if (confirm('Are you sure you want to delete this reservation?')) {
      this.http.delete('sessions', id).subscribe({
        next: () => this.loadReservations(this.session!.id),
        error: (err) => console.error('Failed to delete reservation', err),
      });
    }
  }

  goBack() {
    this.location.back();
  }
}
