import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { EMPTY, Subject, catchError, exhaustMap, takeUntil, tap } from 'rxjs';
import { BasketCheckout } from '../../core/models/basket.model';
import { BasketService } from '../../core/services/basket.service';
import { ToastrService } from '../../shared/services/toastr.service';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit, OnDestroy {
  submitting = false;

  // Usamos um Subject para orquestrar envios da mesma forma que um fluxo de eventos.
  // Isso nos permite aplicar operadores como exhaustMap e centralizar o tratamento de resposta.
  private readonly checkoutRequests$ = new Subject<BasketCheckout>();
  private readonly destroy$ = new Subject<void>();

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

  constructor(
    private readonly fb: FormBuilder,
    private readonly basketService: BasketService,
    private readonly toastrService: ToastrService
  ) {}

  ngOnInit(): void {
    this.checkoutForm.patchValue({ country: 'US', state: 'WA', paymentMethod: 1 });

    this.checkoutRequests$
      .pipe(
        // exhaustMap ignora cliques repetidos enquanto uma submissão está em andamento,
        // prevenindo disparos duplicados e aproveitando o estado "submitting" como trava.
        exhaustMap((checkout) => {
          this.submitting = true;

          return this.basketService.checkoutBasket(checkout).pipe(
            tap({
              next: () => {
                this.toastrService.showInfo('Pedido enviado com sucesso!');
                this.submitting = false;
              },
              error: () => {
                this.toastrService.showDanger('Falha ao finalizar o pedido. Tente novamente.');
                this.submitting = false;
              }
            }),
            // Mantemos o fluxo vivo após falhas com EMPTY, evitando que novos envios sejam bloqueados.
            catchError(() => EMPTY)
          );
        }),
        takeUntil(this.destroy$)
      )
      .subscribe();
  }

  submit(): void {
    if (this.checkoutForm.invalid) {
      this.checkoutForm.markAllAsTouched();
      this.toastrService.showWarning('Preencha os dados de pagamento e entrega para finalizar o pedido.');
      return;
    }

    const checkout: BasketCheckout = {
      userName: 'swn',
      customerId: '00000000-0000-0000-0000-000000000000',
      totalPrice: 0,
      ...this.checkoutForm.value
    } as BasketCheckout;

    // Emitimos no Subject para que a cadeia com exhaustMap assuma a responsabilidade
    // de serializar envios e evitar duplicidades de requisições.
    this.checkoutRequests$.next(checkout);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.checkoutRequests$.complete();
  }
}
