import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  DataTableComponent,
  TableColumn,
} from '../../shared/components/data-table/data-table.component';
import {
  EntityFormComponent,
  FormField,
} from '../../shared/components/entity-form/entity-form.component';
import { Location } from '../../core/models/location.model';
import { Address } from '../../core/models/address.model';
import { City } from '../../core/models/city.model';
import { Country } from '../../core/models/country.model';
import { TranslatePipe } from '@ngx-translate/core';
import { DynamicService } from '../../core/dynamic.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-location',
  imports: [
    CommonModule,
    DataTableComponent,
    EntityFormComponent,
    TranslatePipe,
  ],
  templateUrl: './location.component.html',
  styleUrl: './location.component.css',
})
export class LocationComponent implements OnInit {
  activeTab: 'locations' | 'addresses' | 'cities' | 'countries' = 'locations';
  showForm = false;
  selectedEntity: any = null;

  constructor(private httpService: DynamicService) {}

  locations: Location[] = [];
  addresses: Address[] = [];
  cities: City[] = [];
  countries: Country[] = [];

  locationColumns: TableColumn[] = [
    { key: 'name', label: 'Name' },
    { key: 'address.street', label: 'Street' },
    { key: 'address.number', label: 'Number' },
    { key: 'address.city.name', label: 'City' },
    { key: 'address.city.country.name', label: 'Country' },
  ];

  addressColumns: TableColumn[] = [
    { key: 'street', label: 'Street' },
    { key: 'number', label: 'Number' },
    { key: 'city.name', label: 'City' },
    { key: 'city.country.name', label: 'Country' },
  ];

  cityColumns: TableColumn[] = [
    { key: 'name', label: 'Name' },
    { key: 'country.name', label: 'Country' },
  ];

  countryColumns: TableColumn[] = [{ key: 'name', label: 'Name' }];

  locationFields: FormField[] = [
    {
      name: 'name',
      label: 'Location Name',
      type: 'text',
      required: true,
      placeholder: 'Enter location name',
    },
    {
      name: 'addressId',
      label: 'Address',
      type: 'select',
      required: true,
      options: [],
    },
  ];

  addressFields: FormField[] = [
    {
      name: 'street',
      label: 'Street',
      type: 'text',
      required: true,
      placeholder: 'Enter street name',
    },
    {
      name: 'number',
      label: 'Number',
      type: 'text',
      required: true,
      placeholder: 'Enter number',
    },
    {
      name: 'cityId',
      label: 'City',
      type: 'select',
      required: true,
      options: [],
    },
  ];

  cityFields: FormField[] = [
    {
      name: 'name',
      label: 'City Name',
      type: 'text',
      required: true,
      placeholder: 'Enter city name',
    },
    {
      name: 'countryId',
      label: 'Country',
      type: 'select',
      required: true,
      options: [],
    },
  ];

  countryFields: FormField[] = [
    {
      name: 'name',
      label: 'Country Name',
      type: 'text',
      required: true,
      placeholder: 'Enter country name',
    },
  ];

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    forkJoin({
      countries: this.httpService.getAll<Country[]>('countries'),
      cities: this.httpService.getAll<City[]>('cities'),
      addresses: this.httpService.getAll<Address[]>('addresses'),
      locations: this.httpService.getAll<Location[]>('locations'),
    }).subscribe((result) => {
      this.countries = result.countries;
      this.cities = result.cities;
      this.addresses = result.addresses;
      this.locations = result.locations;

      this.updateFormOptions();
    });
  }

  updateFormOptions(): void {
    this.locationFields[1].options = this.addresses.map((a) => ({
      value: a.id,
      label: `${a.street} ${a.number}, ${a.city.name}`,
    }));

    this.addressFields[2].options = this.cities.map((c) => ({
      value: c.id,
      label: c.name,
    }));

    this.cityFields[1].options = this.countries.map((c) => ({
      value: c.id,
      label: c.name,
    }));
  }

  setTab(tab: 'locations' | 'addresses' | 'cities' | 'countries'): void {
    this.activeTab = tab;
    this.showForm = false;
    this.selectedEntity = null;
  }

  openForm(entity?: any): void {
    this.selectedEntity = entity || null;
    this.showForm = true;
  }

  closeForm(): void {
    this.showForm = false;
    this.selectedEntity = null;
  }

  onEdit(entity: any): void {
    this.openForm(entity);
  }

onDelete(entity: any): void {
  if (!entity?.id) return;

  this.httpService.delete(this.activeTab, entity.id)
    .subscribe({
      next: () => {
        console.log('Deleted (soft) successfully');
        this.loadData();
      },
      error: err => {
        console.error('Delete error', err);
      }
    });
}

  onFormSubmit(data: any): void {
    if (this.selectedEntity) {
      this.httpService
        .update(this.activeTab, this.selectedEntity.id, data)
        .subscribe({
          next: () => {
            console.log('Updated successfully');
            this.closeForm();
            this.loadData();
          },
          error: (err) => {
            console.error('Update error', err);
          },
        });
    } else {
      this.httpService.create(this.activeTab, data).subscribe({
        next: () => {
          console.log('Created successfully');
          this.closeForm();
          this.loadData();
        },
        error: (err) => {
          console.error('Create error', err);
        },
      });
    }
  }

  get currentColumns(): TableColumn[] {
    switch (this.activeTab) {
      case 'locations':
        return this.locationColumns;
      case 'addresses':
        return this.addressColumns;
      case 'cities':
        return this.cityColumns;
      case 'countries':
        return this.countryColumns;
    }
  }

  get currentData(): any[] {
    switch (this.activeTab) {
      case 'locations':
        return this.locations;
      case 'addresses':
        return this.addresses;
      case 'cities':
        return this.cities;
      case 'countries':
        return this.countries;
    }
  }

  get currentFields(): FormField[] {
    switch (this.activeTab) {
      case 'locations':
        return this.locationFields;
      case 'addresses':
        return this.addressFields;
      case 'cities':
        return this.cityFields;
      case 'countries':
        return this.countryFields;
    }
  }

  get formTitle(): string {
    const action = this.selectedEntity ? 'Edit' : 'Create';
    return `${action} ${
      this.activeTab.slice(0, -1).charAt(0).toUpperCase() +
      this.activeTab.slice(1, -1)
    }`;
  }
}
