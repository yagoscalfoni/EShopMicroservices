import { CommonModule } from '@angular/common';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { of, Subject, throwError } from 'rxjs';
import { CheckoutBasketResponse } from '../../core/models/basket.model';
import { BasketService } from '../../core/services/basket.service';
import { CheckoutComponent } from './checkout.component';

describe('CheckoutComponent', () => {
  let fixture: ComponentFixture<CheckoutComponent>;
  let component: CheckoutComponent;
  let basketService: jasmine.SpyObj<BasketService>;

  const fillValidForm = () => {
    component.checkoutForm.setValue({
      firstName: 'John',
      lastName: 'Doe',
      emailAddress: 'john@doe.com',
      addressLine: '123 st',
      country: 'US',
      state: 'WA',
      zipCode: '98101',
      cardName: 'JOHN DOE',
      cardNumber: '4111111111111111',
      expiration: '12/30',
      cvv: '123',
      paymentMethod: 1
    });
  };

  beforeEach(() => {
    basketService = jasmine.createSpyObj<BasketService>('BasketService', ['checkoutBasket']);

    TestBed.configureTestingModule({
      imports: [CommonModule, ReactiveFormsModule, RouterTestingModule, CheckoutComponent],
      providers: [FormBuilder, { provide: BasketService, useValue: basketService }]
    });

    fixture = TestBed.createComponent(CheckoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should ignore rapid consecutive submits while a checkout request is in flight (exhaustMap)', () => {
    fillValidForm();
    const inFlight = new Subject<CheckoutBasketResponse>();
    basketService.checkoutBasket.and.returnValue(inFlight);

    component.submit();
    component.submit();

    // exhaustMap should ignore the second submission until the first completes.
    expect(basketService.checkoutBasket).toHaveBeenCalledTimes(1);
    expect(component.submitting).toBeTrue();

    inFlight.next({ isSuccess: true });
    inFlight.complete();

    expect(component.submitting).toBeFalse();
    expect(component.message).toContain('sucesso');
  });

  it('should allow a new submission after a failure thanks to catchError + exhaustMap', () => {
    fillValidForm();
    basketService.checkoutBasket.and.returnValue(throwError(() => new Error('fail')));

    component.submit();

    expect(component.message).toContain('Falha');
    expect(component.submitting).toBeFalse();
    expect(basketService.checkoutBasket).toHaveBeenCalledTimes(1);

    // After the failure we return a successful observable to prove the pipeline stays alive.
    basketService.checkoutBasket.and.returnValue(of({ isSuccess: true }));

    component.submit();

    expect(basketService.checkoutBasket).toHaveBeenCalledTimes(2);
    expect(component.message).toContain('sucesso');
  });
});
