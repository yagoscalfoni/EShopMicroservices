import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { BasketCheckout } from '../../core/models/basket.model';
import { BasketService } from '../../core/services/basket.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  submitting = false;
  message = '';

  readonly checkoutForm = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    emailAddress: ['', [Validators.required, Validators.email]],
    addressLine: ['', Validators.required],
    country: ['', Validators.required],
    state: ['', Validators.required],
    zipCode: ['', Validators.required],
    cardName: ['', Validators.required],
    cardNumber: ['', Validators.required],
    expiration: ['', Validators.required],
    cvv: ['', Validators.required],
    paymentMethod: [1, Validators.required]
  });

  constructor(private readonly fb: FormBuilder, private readonly basketService: BasketService) {}

  ngOnInit(): void {
    this.checkoutForm.patchValue({ country: 'US', state: 'WA', paymentMethod: 1 });
  }

  submit(): void {
    if (this.checkoutForm.invalid) {
      this.checkoutForm.markAllAsTouched();
      return;
    }

    this.submitting = true;
    this.message = '';

    const checkout: BasketCheckout = {
      userName: 'swn',
      customerId: '00000000-0000-0000-0000-000000000000',
      totalPrice: 0,
      ...this.checkoutForm.value
    } as BasketCheckout;

    this.basketService.checkoutBasket(checkout).subscribe({
      next: () => {
        this.message = 'Pedido enviado com sucesso!';
        this.submitting = false;
      },
      error: () => {
        this.message = 'Falha ao finalizar o pedido. Tente novamente.';
        this.submitting = false;
      }
    });
  }
}
