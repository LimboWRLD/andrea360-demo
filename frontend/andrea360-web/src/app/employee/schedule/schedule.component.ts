import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DynamicService } from '../../core/dynamic.service';
import { SignalRService } from '../../core/signalr.service';
import { AuthService } from '../../core/auth.service';
import { Session } from '../../core/models/session.model';
import { Service } from '../../core/models/service.model';
import { Subscription } from 'rxjs';
import { EntityFormComponent, FormField } from '../../shared/components/entity-form/entity-form.component';
import { TranslatePipe } from '@ngx-translate/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-schedule',
  imports: [CommonModule, FormsModule, EntityFormComponent, TranslatePipe, RouterLink],
  templateUrl: './schedule.component.html',
  styleUrl: './schedule.component.css',
})
export class ScheduleComponent implements OnInit, OnDestroy {
  sessions: Session[] = [];
  services: Service[] = [];
  showForm:boolean = false;

  selectedSession: any = null;
  private capacitySubscription?: Subscription;

  constructor(
    private dynamicService: DynamicService,
    private signalRService: SignalRService,
    private authService: AuthService
  ) {}

  sessionFields: FormField[] = [
    { name: 'startTime', label: 'SESSION.FIELDS.START_TIME', type: 'datetime-local', required: true },
    { name: 'endTime', label: 'SESSION.FIELDS.END_TIME', type: 'datetime-local', required: true },
    { name: 'serviceId', label: 'SESSION.FIELDS.SERVICE', type: 'select', required: true, options: [] },
    { name: 'maxCapacity', label: 'SESSION.FIELDS.CAPACITY', type: 'number', defaultValue: 10 },
  ];

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe((user) => {
      console.log('AuthService user loaded:', user);

      if (user?.locationId) {
        console.log('Location ID found:', user.locationId);
        this.loadSessions();
      } else {
        console.warn('Location ID missing, waiting...');
      }
    });
    this.loadServices();
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
      console.error('User location not found');
      return;
    }

    this.dynamicService.getByPath<Session[]>(`sessions`).subscribe({
      next: (data) =>
        (this.sessions = data.filter((s) => s.locationId == locationId)),
      error: (err) => console.error('Failed to load sessions', err),
    });
  }

  loadServices() {
    this.dynamicService.getAll<Service[]>('services').subscribe({
      next: (data) => {
        this.services = data;
        this.sessionFields.find((f) => f.name === 'serviceId')!.options =
          data.map((s) => ({ value: s.id, label: s.name }));
      },
    });
  }


  openForm(session?: any) {
    this.selectedSession = session || null;
    console.log("Session", session);
    
    this.showForm = true;
  }

  closeForm() {
    this.showForm = false;
    this.selectedSession = null;
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
      locationId: this.authService.getLocationId(),
      startTime: this.convertToUtc(formValue.startTime),
      endTime: this.convertToUtc(formValue.endTime),
    };

    console.log("Form value", formValue);
    console.log(this.selectedSession);
    

    if (this.selectedSession) {
      this.dynamicService
        .update('sessions', this.selectedSession.id, payload)
        .subscribe(() => {
          this.loadSessions();
          this.closeForm();
        });
    } else {
      this.dynamicService.create('sessions', payload).subscribe(() => {
        this.loadSessions();
        this.closeForm();
      });
    }
  }

  deleteSession(id: string): void {
    if (confirm('Are you sure you want to delete this session?')) {
      this.dynamicService.delete('sessions', id).subscribe({
        next: () => this.loadSessions(),
        error: (err) => console.error('Failed to delete session', err),
      });
    }
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
}
