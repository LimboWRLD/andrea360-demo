import { Component, inject } from '@angular/core';
import { DynamicService } from '../../core/dynamic.service';
import { TableColumn, DataTableComponent } from '../../shared/components/data-table/data-table.component';
import { FormField, EntityFormComponent } from '../../shared/components/entity-form/entity-form.component';
import { User } from '../../core/models/user.model';
import { CommonModule } from '@angular/common';
import { TranslatePipe } from '@ngx-translate/core';
import Keycloak from 'keycloak-js';

@Component({
  selector: 'app-members',
  imports: [DataTableComponent, EntityFormComponent, CommonModule, TranslatePipe],
  templateUrl: './members.component.html',
  styleUrl: './members.component.css'
})
export class MembersComponent {
  private http = inject(DynamicService);
  private keycloak = inject(Keycloak);

  members: any[] = [];
  locations: Location[] = [];
  filteredMembers: any[] = [];
  currentLocationId = '';

  locationFilter = '';
  showForm = false;
  selectedMember: any = null;

  employeeColumns: TableColumn[] = [
    { key: 'firstName', label: 'MEMBER.FIELDS.FIRST_NAME' },
    { key: 'lastName', label: 'MEMBER.FIELDS.LAST_NAME' },
    { key: 'email', label: 'MEMBER.FIELDS.EMAIL' },
    { key: 'enabled', label: 'MEMBER.FIELDS.ENABLED', render: v => (v ? 'Yes' : 'No') },
  ];

  employeeFields: FormField[] = [
    { name: 'firstName', label: 'MEMBER.FIELDS.FIRST_NAME', type: 'text', required: true },
    { name: 'lastName', label: 'MEMBER.FIELDS.LAST_NAME', type: 'text', required: true },
    { name: 'email', label: 'MEMBER.FIELDS.EMAIL', type: 'text', required: true, validators: ['email'] },
    { name: 'enabled', label: 'MEMBER.FIELDS.ENABLED', type: 'switch', defaultValue: true },
  ];

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.http.getAll<User[]>('users').subscribe((users) => {
      this.members =  users.filter(u => u.realmRoles.includes('member') && u.locationId === this.currentLocationId);
      
      this.filteredMembers =  users.filter(u => u.realmRoles.includes('member') && u.locationId === this.currentLocationId);
      
      const currentUserEmail = this.keycloak.tokenParsed?.['email'];
      if (currentUserEmail) {
        const currentUser = users.find(u => u.email === currentUserEmail);
        if (currentUser) {
          this.currentLocationId = currentUser.locationId;
          this.members =  users.filter(u => u.realmRoles.includes('member') && u.locationId === this.currentLocationId);
          this.filteredMembers =  users.filter(u => u.realmRoles.includes('member') && u.locationId === this.currentLocationId);
        }
      }
    });
  }

  openForm(employee?: any) {
    this.selectedMember = employee || null;
    this.showForm = true;
  }

  closeForm() {
    this.showForm = false;
    this.selectedMember = null;
  }

  onSubmit(data: any) {
    data.locationId = this.currentLocationId;
    data.realmRoles = ['member'];
    
    if (this.selectedMember) {
      this.http.update('users', this.selectedMember.id, data).subscribe(() => {
        this.closeForm();
        this.loadData();
      });
    } else {
      this.http.create('users', data).subscribe(() => {
        this.closeForm();
        this.loadData();
      });
    }
  }

  onDelete(employee: any) {
    this.http.delete('users', employee.id).subscribe(() => {
      this.loadData();
    });
  }
}
