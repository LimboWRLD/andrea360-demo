import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

export interface FormField {
  name: string;
  label: string;
  type: 'text' | 'select' | 'number';
  required?: boolean;
  options?: { value: any; label: string }[];
  placeholder?: string;
}

@Component({
  selector: 'app-entity-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './entity-form.component.html',
  styleUrl: './entity-form.component.css'
})
export class EntityFormComponent implements OnInit, OnChanges {
  @Input() fields: FormField[] = [];
  @Input() data: any = null;
  @Input() title: string = 'Form';
  @Output() onSubmit = new EventEmitter<any>();
  @Output() onCancel = new EventEmitter<void>();

  form!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.buildForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['data'] && !changes['data'].firstChange) {
      this.buildForm();
    }
    if (changes['fields'] && !changes['fields'].firstChange) {
      this.buildForm();
    }
  }

  buildForm(): void {
    const group: any = {};
    
    this.fields.forEach(field => {
      const validators = field.required ? [Validators.required] : [];
      const value = this.data?.[field.name] ?? '';
      group[field.name] = [value, validators];
    });

    this.form = this.fb.group(group);
  }

  submit(): void {
    if (this.form.valid) {
      this.onSubmit.emit(this.form.value);
    } else {
      this.form.markAllAsTouched();
    }
  }

  cancel(): void {
    this.onCancel.emit();
    this.form.reset();
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.form.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }
}
