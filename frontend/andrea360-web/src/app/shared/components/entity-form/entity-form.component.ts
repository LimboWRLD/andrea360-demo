import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';

export interface FormField {
  name: string;
  label: string;
  type: 'text' | 'select' | 'number' | 'multiselect' | 'switch' | 'datetime-local';
  required?: boolean;
  options?: { value: any; label: string }[];
  placeholder?: string;
  validators?: string[];
  defaultValue?: any;
  suffix?: string;
}

@Component({
  selector: 'app-entity-form',
  imports: [CommonModule, ReactiveFormsModule, TranslatePipe],
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
      const validators: any[] = [];
      
      if (field.required) {
        validators.push(Validators.required);
      }
      
      if (field.validators) {
        field.validators.forEach(validatorName => {
          switch (validatorName.toLowerCase()) {
            case 'email':
              validators.push(Validators.email);
              break;
            case 'minlength':
              validators.push(Validators.minLength(3));
              break;
            case 'maxlength':
              validators.push(Validators.maxLength(100));
              break;
          }
        });
      }
      
      const value = this.data?.[field.name] ?? field.defaultValue ?? '';
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
