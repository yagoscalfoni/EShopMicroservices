import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, switchMap } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  BasketCheckout,
  CheckoutBasketRequest,
  CheckoutBasketResponse,
  GetBasketResponse,
  ShoppingCart,
  ShoppingCartItem,
  StoreBasketRequest,
  StoreBasketResponse
} from '../models/basket.model';
import { Product } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class BasketService {
  private readonly baseUrl = environment.apiBaseUrl;
  private readonly defaultUser = 'swn';

  constructor(private readonly http: HttpClient) {}

  getBasket(userName: string = this.defaultUser): Observable<ShoppingCart> {
    return this.http
      .get<GetBasketResponse>(`${this.baseUrl}/basket-service/basket/${userName}`)
      .pipe(map((response) => this.enrichTotals(response.cart)));
  }

  storeBasket(cart: ShoppingCart): Observable<StoreBasketResponse> {
    const request: StoreBasketRequest = { cart: this.enrichTotals(cart) };
    return this.http.post<StoreBasketResponse>(`${this.baseUrl}/basket-service/basket`, request);
  }

  deleteBasket(userName: string = this.defaultUser): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/basket-service/basket/${userName}`);
  }

  checkoutBasket(checkout: BasketCheckout): Observable<CheckoutBasketResponse> {
    const request: CheckoutBasketRequest = { basketCheckoutDto: checkout };
    return this.http.post<CheckoutBasketResponse>(`${this.baseUrl}/basket-service/basket/checkout`, request);
  }

  addItem(product: Product, quantity: number = 1): Observable<ShoppingCart> {
    return this.getBasket().pipe(
      switchMap((cart) => {
        const existingItem = cart.items.find((i) => i.productId === product.id);
        if (existingItem) {
          existingItem.quantity += quantity;
        } else {
          const newItem: ShoppingCartItem = {
            productId: product.id,
            productName: product.name,
            price: product.price,
            quantity,
            color: 'Black'
          };
          cart.items = [...cart.items, newItem];
        }

        return this.storeBasket(cart).pipe(map(() => this.enrichTotals(cart)));
      })
    );
  }

  removeItem(productId: string): Observable<ShoppingCart> {
    return this.getBasket().pipe(
      switchMap((cart) => {
        cart.items = cart.items.filter((item) => item.productId !== productId);
        return this.storeBasket(cart).pipe(map(() => this.enrichTotals(cart)));
      })
    );
  }

  private enrichTotals(cart: ShoppingCart): ShoppingCart {
    const totalPrice = cart.items.reduce((total, item) => total + item.price * item.quantity, 0);
    return { ...cart, totalPrice };
  }
}
