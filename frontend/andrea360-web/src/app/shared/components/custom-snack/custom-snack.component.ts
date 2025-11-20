import { Component, Inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MAT_SNACK_BAR_DATA } from '@angular/material/snack-bar';

@Component({
  selector: 'app-custom-snack',
  imports: [MatIconModule],
  templateUrl: './custom-snack.component.html',
  styleUrl: './custom-snack.component.css'
})
export class CustomSnackComponent {
    constructor(@Inject(MAT_SNACK_BAR_DATA) public data: { message: string; type: 'success' | 'error' | 'info' }) {}
}
