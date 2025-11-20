import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslatePipe } from '@ngx-translate/core';
import { PaymentComponent } from '../../shared/components/payment/payment.component';
import { DynamicService } from '../../core/dynamic.service';
import { Service } from '../../core/models/service.model';

@Component({
  selector: 'app-shop',
  imports: [CommonModule, TranslatePipe, PaymentComponent],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.css'
})
export class ShopComponent {
  private http = inject(DynamicService);

  services: Service[] = [];
  selectedServiceId: string | null = null;
  showPayment = false;

  ngOnInit(): void {
    this.loadServices();
  }

  loadServices() {
    this.http.getAll<Service[]>('services').subscribe((services) => {
      this.services = services;
    });
  }

  selectService(service: Service) {
    this.selectedServiceId = service.id;
    this.showPayment = true;
  }

  closePayment() {
    this.showPayment = false;
    this.selectedServiceId = null;
  }

  getSelectedService(): Service | undefined {
    return this.services.find(s => s.id === this.selectedServiceId);
  }
}
