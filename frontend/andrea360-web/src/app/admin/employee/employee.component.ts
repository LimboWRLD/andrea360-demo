import { Component } from '@angular/core';
import { TableColumn, DataTableComponent } from '../../shared/components/data-table/data-table.component';
import { FormField, EntityFormComponent } from '../../shared/components/entity-form/entity-form.component';
import { DynamicService } from '../../core/dynamic.service';
import { Location } from '../../core/models/location.model';
import { CommonModule } from '@angular/common';
import { TranslatePipe } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { User } from '../../core/models/user.model';

@Component({
  selector: 'app-employee',
  imports: [CommonModule, TranslatePipe, FormsModule, DataTableComponent, EntityFormComponent],
  templateUrl: './employee.component.html',
  styleUrl: './employee.component.css',
})
export class EmployeeComponent {
  employees: any[] = [];
  locations: Location[] = [];
  filteredEmployees: any[] = [];

  locationFilter = '';
  showForm = false;
  selectedEmployee: any = null;

  constructor(private http: DynamicService) {}

  employeeColumns: TableColumn[] = [
    { key: 'firstName', label: 'EMPLOYEE.FIELDS.FIRST_NAME' },
    { key: 'lastName', label: 'EMPLOYEE.FIELDS.LAST_NAME' },
    { key: 'email', label: 'EMPLOYEE.FIELDS.EMAIL' },
    { 
      key: 'locationId', 
      label: 'EMPLOYEE.FIELDS.LOCATION',
      render: (locationId) => {
        const location = this.locations.find(l => l.id === locationId);
        return location ? location.name : '-';
      }
    },
    { key: 'enabled', label: 'EMPLOYEE.FIELDS.ENABLED', render: v => (v ? 'Yes' : 'No') },
    { key: 'realmRoles', label: 'EMPLOYEE.FIELDS.ROLES', render: (_, row) => row.realmRoles.join(', ') }
  ];

  employeeFields: FormField[] = [
    { name: 'firstName', label: 'EMPLOYEE.FIELDS.FIRST_NAME', type: 'text', required: true },
    { name: 'lastName', label: 'EMPLOYEE.FIELDS.LAST_NAME', type: 'text', required: true },
    { name: 'email', label: 'EMPLOYEE.FIELDS.EMAIL', type: 'text', required: true },
    { name: 'locationId', label: 'EMPLOYEE.FIELDS.LOCATION', type: 'select', required: true, options: [] },
    { name: 'enabled', label: 'EMPLOYEE.FIELDS.ENABLED', type: 'switch' },
    { name: 'realmRoles', label: 'EMPLOYEE.FIELDS.ROLES', type: 'multiselect', options: [] }
  ];

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.http.getAll<User[]>('users').subscribe((users) => {
      this.employees = users;
      console.log(users);
      
      this.filteredEmployees = users;
    });

    this.http.getAll<Location[]>('locations').subscribe((locs) => {
      this.locations = locs;
      this.employeeFields[3].options = locs.map(l => ({ value: l.id, label: l.name }));
    });

    const allRoles = ['employee', 'member'];
    this.employeeFields[5].options = allRoles.map(r => ({ value: r, label: r }));
  }

  filterEmployees() {
    this.filteredEmployees =
      this.locationFilter === ''
        ? this.employees
        : this.employees.filter(e => e.locationId === this.locationFilter);
  }

  openForm(employee?: any) {
    this.selectedEmployee = employee || null;
    this.showForm = true;
  }

  closeForm() {
    this.showForm = false;
    this.selectedEmployee = null;
  }

  onSubmit(data: any) {
    console.log(data);
    
    if (this.selectedEmployee) {
      this.http.update('users', this.selectedEmployee.id, data).subscribe(() => {
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
