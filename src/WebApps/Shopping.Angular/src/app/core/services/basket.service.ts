import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, map, Observable, shareReplay, switchMap, take, tap } from 'rxjs';
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

  // Usamos um gatilho reativo para controlar quando a cesta deve ser carregada novamente
  // sem expor detalhes de implementação aos componentes. O BehaviorSubject guarda
  // o último usuário solicitado, permitindo reenviar o mesmo valor ao atualizar o cache.
  private readonly basketRefresh$ = new BehaviorSubject<string>(this.defaultUser);

  // O stream abaixo dispara uma chamada HTTP sempre que o usuário muda ou quando
  // `refreshBasket()` é invocado. O switchMap cancela requisições anteriores caso
  // um novo valor chegue rápido (evita respostas fora de ordem), e o shareReplay(1)
  // mantém o último resultado em cache para reutilizar entre múltiplas inscrições
  // sem repetir chamadas ao backend.
  readonly basket$ = this.basketRefresh$.pipe(
    switchMap((userName) =>
      this.http
        .get<GetBasketResponse>(`${this.baseUrl}/basket-service/basket/${userName}`)
        .pipe(map((response) => this.enrichTotals(response.cart)))
    ),
    shareReplay({ bufferSize: 1, refCount: true })
  );

  constructor(private readonly http: HttpClient) {}

  getBasket(userName: string = this.defaultUser): Observable<ShoppingCart> {
    // Atualizamos o gatilho apenas quando o usuário solicitado muda, evitando
    // recriar o fluxo sem necessidade. Em seguida reutilizamos o stream cacheado.
    if (userName !== this.basketRefresh$.value) {
      this.basketRefresh$.next(userName);
    }

    return this.basket$;
  }

  storeBasket(cart: ShoppingCart): Observable<StoreBasketResponse> {
    const request: StoreBasketRequest = { cart: this.enrichTotals(cart) };
    return this.http.post<StoreBasketResponse>(`${this.baseUrl}/basket-service/basket`, request).pipe(
      // Ao salvar, forçamos o refresh para que o cache via shareReplay reflita o novo estado.
      tap(() => this.refreshBasket())
    );
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
      take(1),
      // switchMap garante que utilizamos sempre a versão mais recente do carrinho
      // e evita empilhar inscrições caso o usuário clique repetidamente em "adicionar".
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
      take(1),
      // switchMap impede condições de corrida quando várias remoções acontecem
      // rapidamente, usando sempre o carrinho mais atualizado disponível.
      switchMap((cart) => {
        cart.items = cart.items.filter((item) => item.productId !== productId);
        return this.storeBasket(cart).pipe(map(() => this.enrichTotals(cart)));
      })
    );
  }

  // Exposto para operações que precisam forçar o recarregamento do carrinho cacheado.
  refreshBasket(): void {
    this.basketRefresh$.next(this.basketRefresh$.value);
  }

  private enrichTotals(cart: ShoppingCart): ShoppingCart {
    const totalPrice = cart.items.reduce((total, item) => total + item.price * item.quantity, 0);
    return { ...cart, totalPrice };
  }
}
