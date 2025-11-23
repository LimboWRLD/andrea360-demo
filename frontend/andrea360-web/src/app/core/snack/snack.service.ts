import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CustomSnackComponent } from '../../shared/components/custom-snack/custom-snack.component';

@Injectable({
  providedIn: 'root',
})
export class SnackService {
  constructor(private snackBar: MatSnackBar) {}

  show(
    message: string,
    type: 'success' | 'error' | 'info' = 'info',
    duration = 3000
  ) {
    this.snackBar.openFromComponent(CustomSnackComponent, {
      data: { message, type },
      duration,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: ['snack-container', `snack-${type}`],
    });
  }
}
