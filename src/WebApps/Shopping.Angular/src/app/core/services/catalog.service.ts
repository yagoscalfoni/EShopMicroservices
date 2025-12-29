import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  GetProductByCategoryResponse,
  GetProductByIdResponse,
  GetProductsResponse
} from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class CatalogService {
  private readonly baseUrl = environment.apiBaseUrl;

  constructor(private readonly http: HttpClient) {}

  getProducts(pageNumber: number = 1, pageSize: number = 10): Observable<GetProductsResponse> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    return this.http.get<GetProductsResponse>(`${this.baseUrl}/catalog-service/products`, { params });
  }

  getProductsByCategory(category: string): Observable<GetProductByCategoryResponse> {
    return this.http.get<GetProductByCategoryResponse>(
      `${this.baseUrl}/catalog-service/products/category/${encodeURIComponent(category)}`
    );
  }

  getProduct(id: string): Observable<GetProductByIdResponse> {
    return this.http.get<GetProductByIdResponse>(`${this.baseUrl}/catalog-service/products/${id}`);
  }
}
