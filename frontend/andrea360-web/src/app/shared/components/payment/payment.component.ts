import { Component, Input, OnInit, OnDestroy, CUSTOM_ELEMENTS_SCHEMA, inject, Output, EventEmitter } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, Validators } from '@angular/forms';
import { StripeElementsOptions, StripePaymentElementOptions } from '@stripe/stripe-js';
import { NgxStripeModule, injectStripe } from 'ngx-stripe';
import { DynamicService } from '../../../core/dynamic.service';
import { CommonModule } from '@angular/common';
import { User } from '../../../core/models/user.model';
import Keycloak from 'keycloak-js';
import { SnackService } from '../../../core/snack/snack.service';

@Component({
  selector: 'app-payment',
  imports: [CommonModule, NgxStripeModule, ReactiveFormsModule],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class PaymentComponent implements OnInit, OnDestroy {
  @Input() serviceId!: string;
  @Input() servicePrice!: number;

  @Output() paymentComplete = new EventEmitter<void>();
  @Output() paymentCancelled = new EventEmitter<void>();

  private readonly fb = inject(UntypedFormBuilder);
  private httpService = inject(DynamicService);
  private keycloak = inject(Keycloak);
  private snackService = inject(SnackService);

  stripe = injectStripe();

  billingForm = this.fb.group({
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
  });

  elementsOptions: StripeElementsOptions = {
    locale: 'en',
    appearance: { theme: 'flat' },
  };

  paymentElementOptions: StripePaymentElementOptions = {
    layout: { type: 'tabs' },
  };

  clientSecret: string | null = null;
  elementsInstance: any = null;
  paymentReady = false;
  processing = false;
  currentUserId: string | null = null;

  ngOnInit() {
    const email = this.keycloak.tokenParsed?.['email'];
    const givenName = this.keycloak.tokenParsed?.['given_name'] || '';
    const familyName = this.keycloak.tokenParsed?.['family_name'] || '';

    if (email) {
      this.billingForm.patchValue({ email, firstName: givenName, lastName: familyName });
      this.httpService.getAll<User[]>('users').subscribe((users) => {
        const u = users.find((x) => x.email === email);
        if (u) {
          this.currentUserId = u.id;
          this.createPaymentIntentIfReady();
        }
      });
    }
  }

  ngOnDestroy(): void {
  }

  private createPaymentIntent() {
    if (!this.serviceId || !this.currentUserId) return;
    this.httpService.createPaymentIntent(this.serviceId, this.currentUserId).subscribe((response: any) => {
      const secret = typeof response === 'string' ? response : response.clientSecret;
      this.clientSecret = secret;
      if (secret) this.elementsOptions = { ...this.elementsOptions, clientSecret: secret };
    });
  }

  private createPaymentIntentIfReady() {
    if (this.clientSecret) return;
    if (this.serviceId && this.currentUserId) this.createPaymentIntent();
  }

  onElementsReady(elements: any) {
    this.elementsInstance = elements;
    this.paymentReady = true;
  }

  pay() {
    if (!this.billingForm.valid) {
      console.error('Form is invalid');
      return;
    }
    if (!this.elementsInstance) {
      console.error('Payment elements not ready');
      return;
    }

    this.processing = true;

    const billingName = `${this.billingForm.get('firstName')?.value} ${this.billingForm.get('lastName')?.value}`;
    const billingEmail = this.billingForm.get('email')?.value as string;

    this.stripe
      .confirmPayment({
        elements: this.elementsInstance,
        confirmParams: {
          return_url: window.location.href,
          payment_method_data: {
            billing_details: { name: billingName, email: billingEmail },
          },
        },
        redirect: 'if_required',
      } as any)
      .subscribe(
        (result: any) => {
          this.processing = false;
          if (result.error) {
            console.error('Payment error', result.error);
            if (result.error.type === 'invalid_request_error' && result.error.request_log_url) {
              console.error('Stripe request log:', result.error.request_log_url);
            }
            return;
          }
          if (result.paymentIntent && result.paymentIntent.status === 'succeeded') {
            this.onSuccess(result.paymentIntent.id);
            this.paymentComplete.emit();
          } else {
            this.snackService.show("Payment not yet succeeded.", 'info');
          }
        },
        (err: any) => {
          this.processing = false;
          console.error('confirmPayment failed', err);
        }
      );
  }

  onSuccess(stripeId: string) {
    this.httpService
      .create('transactions', {
        userId: this.currentUserId,
        serviceId: this.serviceId,
        amount: this.servicePrice,
        StripeTransactionId : stripeId
      })
      .subscribe(() => {
        this.snackService.show("Transaction succesfull!", 'success');
      });
  }
}
