import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config/api.config';
import {
  GetOrdersByCustomerResponse,
  GetOrdersByNameResponse,
  GetOrdersResponse
} from '../models/order.model';

@Injectable({ providedIn: 'root' })
export class OrderingService {
  private readonly baseUrl = API_BASE_URL;

  constructor(private readonly http: HttpClient) {}

  getOrders(pageIndex: number = 1, pageSize: number = 10): Observable<GetOrdersResponse> {
    const params = new HttpParams()
      .set('pageIndex', pageIndex)
      .set('pageSize', pageSize);

    return this.http.get<GetOrdersResponse>(`${this.baseUrl}/ordering-service/orders`, { params });
  }

  getOrdersByName(orderName: string): Observable<GetOrdersByNameResponse> {
    return this.http.get<GetOrdersByNameResponse>(`${this.baseUrl}/ordering-service/orders/${orderName}`);
  }

  getOrdersByCustomer(customerId: string): Observable<GetOrdersByCustomerResponse> {
    return this.http.get<GetOrdersByCustomerResponse>(`${this.baseUrl}/ordering-service/orders/customer/${customerId}`);
  }
}
