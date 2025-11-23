import { Component, inject } from '@angular/core';
import { DynamicService } from '../../core/dynamic.service';
import { TableColumn, DataTableComponent } from '../../shared/components/data-table/data-table.component';
import { FormField, EntityFormComponent } from '../../shared/components/entity-form/entity-form.component';
import { Service } from '../../core/models/service.model';
import { CommonModule } from '@angular/common';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-services',
  imports: [DataTableComponent, EntityFormComponent, CommonModule, TranslatePipe],
  templateUrl: './services.component.html',
  styleUrl: './services.component.css'
})
export class ServicesComponent {
  private http = inject(DynamicService);

  services: Service[] = [];
  filteredServices: Service[] = [];

  showForm = false;
  selectedService: any = null;

  serviceColumns: TableColumn[] = [
    { key: 'name', label: 'SERVICE.FIELDS.NAME' },
    { key: 'price', label: 'SERVICE.FIELDS.PRICE', render: v => v + ' EUR' },
  ];

  serviceFields: FormField[] = [
    { name: 'name', label: 'SERVICE.FIELDS.NAME', type: 'text', required: true },
    { name: 'price', label: 'SERVICE.FIELDS.PRICE', type: 'number', required: true, suffix: 'EUR' },
  ];

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.http.getAll<Service[]>('services').subscribe((services) => {
      this.services = services;
      this.filteredServices = services;
    });
  }

  openForm(service?: any) {
    this.selectedService = service || null;
    this.showForm = true;
  }

  closeForm() {
    this.showForm = false;
    this.selectedService = null;
  }

  onSubmit(data: any) {
    if (this.selectedService) {
      this.http.update('services', this.selectedService.id, data).subscribe(() => {
        this.closeForm();
        this.loadData();
      });
    } else {
      this.http.create('services', data).subscribe(() => {
        this.closeForm();
        this.loadData();
      });
    }
  }

  onDelete(service: any) {
    this.http.delete('services', service.id).subscribe(() => {
      this.loadData();
    });
  }
}
