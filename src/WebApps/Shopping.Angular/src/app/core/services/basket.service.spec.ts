import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { environment } from '../../../environments/environment';
import { BasketService } from './basket.service';
import { ShoppingCart, ShoppingCartItem } from '../models/basket.model';
import { Product } from '../models/product.model';

describe('BasketService', () => {
  let service: BasketService;
  let httpMock: HttpTestingController;
  const baseUrl = environment.apiBaseUrl;

  const sampleItem: ShoppingCartItem = {
    productId: 'p1',
    productName: 'Phone',
    price: 100,
    quantity: 1,
    color: 'Black'
  };

  const sampleCart: ShoppingCart = {
    userName: 'swn',
    totalPrice: 0,
    items: [sampleItem]
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule]
    });

    service = TestBed.inject(BasketService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should share the latest basket between subscribers without duplicating HTTP calls', () => {
    const received: ShoppingCart[] = [];

    service.getBasket().subscribe((cart) => received.push(cart));
    service.getBasket().subscribe((cart) => received.push(cart));

    const request = httpMock.expectOne(`${baseUrl}/basket-service/basket/swn`);
    expect(request.request.method).toBe('GET');

    request.flush({ cart: sampleCart });

    // shareReplay(1) must return the same enriched cart to all subscribers.
    expect(received.length).toBe(2);
    expect(received[0].totalPrice).toBe(100);
    expect(received[1]).toEqual(received[0]);

    // No additional HTTP request should be pending because the cached value is reused.
    httpMock.expectNone(`${baseUrl}/basket-service/basket/swn`);
  });

  it('should trigger a new request when the basket is refreshed or user changes', () => {
    service.getBasket('alice').subscribe();
    const initial = httpMock.expectOne(`${baseUrl}/basket-service/basket/alice`);
    initial.flush({ cart: { ...sampleCart, userName: 'alice' } });

    // Calling getBasket again for the same user should reuse the cached response.
    service.getBasket('alice').subscribe();
    httpMock.expectNone(`${baseUrl}/basket-service/basket/alice`);

    // Switching user forces a new HTTP call via the BehaviorSubject trigger.
    service.getBasket('bob').subscribe();
    const bobRequest = httpMock.expectOne(`${baseUrl}/basket-service/basket/bob`);
    bobRequest.flush({ cart: { ...sampleCart, userName: 'bob' } });

    // Explicit refresh also emits on the trigger and should request the basket again.
    service.refreshBasket();
    const refreshed = httpMock.expectOne(`${baseUrl}/basket-service/basket/bob`);
    refreshed.flush({ cart: { ...sampleCart, userName: 'bob', items: [] } });
  });

  it('should merge quantities when adding an existing item and persist the updated cart', () => {
    const product: Product = { id: 'p1', name: 'Phone', category: ['tech'], description: '', imageFile: '', price: 100 };

    const updatedCart = service.addItem(product, 2);
    let result: ShoppingCart | undefined;

    updatedCart.subscribe((cart) => (result = cart));

    // First the service loads the latest basket snapshot.
    const loadRequest = httpMock.expectOne(`${baseUrl}/basket-service/basket/swn`);
    loadRequest.flush({ cart: sampleCart });

    // Then it persists the merged cart back to the API.
    const storeRequest = httpMock.expectOne(`${baseUrl}/basket-service/basket`);
    expect(storeRequest.request.method).toBe('POST');
    expect((storeRequest.request.body as any).cart.items[0].quantity).toBe(3);

    storeRequest.flush({ cart: { ...sampleCart, items: [{ ...sampleItem, quantity: 3 }] } });

    expect(result?.items[0].quantity).toBe(3);
    expect(result?.totalPrice).toBe(300);
  });
});
