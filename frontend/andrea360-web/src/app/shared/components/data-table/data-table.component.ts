import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslatePipe } from '@ngx-translate/core';

export interface TableColumn {
  key: string;
  label: string;
  render?: (value: any, row: any) => string;
}

@Component({
  selector: 'app-data-table',
  imports: [CommonModule, TranslatePipe],
  templateUrl: './data-table.component.html',
  styleUrl: './data-table.component.css'
})
export class DataTableComponent {
  @Input() columns: TableColumn[] = [];
  @Input() data: any[] = [];
  @Input() loading: boolean = false;
  @Output() onEdit = new EventEmitter<any>();
  @Output() onDelete = new EventEmitter<any>();

  sortColumn: string | null = null;
  sortDirection: 'asc' | 'desc' = 'asc';

  get sortedData(): any[] {
    if (!this.sortColumn) return this.data;

    return [...this.data].sort((a, b) => {
      const aValue = this.getValue(a, { key: this.sortColumn!, label: '' });
      const bValue = this.getValue(b, { key: this.sortColumn!, label: '' });

      if (aValue == null) return 1;
      if (bValue == null) return -1;

      if (typeof aValue === 'number' && typeof bValue === 'number') {
        return this.sortDirection === 'asc' ? aValue - bValue : bValue - aValue;
      }

      return this.sortDirection === 'asc'
        ? String(aValue).localeCompare(String(bValue))
        : String(bValue).localeCompare(String(aValue));
    });
  }

  getValue(row: any, column: TableColumn): any {
    if (column.render) return column.render(row[column.key], row);

    const keys = column.key.split('.');
    let value = row;
    for (const key of keys) {
      value = value?.[key];
    }
    return value ?? '';
  }

  editRow(row: any): void {
    this.onEdit.emit(row);
  }

  deleteRow(row: any): void {
    this.onDelete.emit(row);
  }

  sortBy(column: TableColumn): void {
    if (this.sortColumn === column.key) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column.key;
      this.sortDirection = 'asc';
    }
  }
}
