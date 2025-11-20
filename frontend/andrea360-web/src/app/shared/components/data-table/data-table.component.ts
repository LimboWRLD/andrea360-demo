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

  getValue(row: any, column: TableColumn): string {
    if (column.render) {
      return column.render(row[column.key], row);
    }
    
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
}
