import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, catchError, map, of } from 'rxjs';
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

    return this.http
      .get<GetProductsResponse>(`${this.baseUrl}/catalog-service/products`, { params })
      .pipe(
        map((response) => this.normalizeProductsResponse(response)),
        catchError(() => of({ products: [] }))
      );
  }

  getProductsByCategory(category: string): Observable<GetProductByCategoryResponse> {
    return this.http
      .get<GetProductByCategoryResponse>(
        `${this.baseUrl}/catalog-service/products/category/${encodeURIComponent(category)}`
      )
      .pipe(
        map((response) => this.normalizeProductsResponse(response)),
        catchError(() => of({ products: [] }))
      );
  }

  getProduct(id: string): Observable<GetProductByIdResponse> {
    return this.http
      .get<GetProductByIdResponse>(`${this.baseUrl}/catalog-service/products/${id}`)
      .pipe(
        map((response) => this.normalizeProductResponse(response)),
        catchError(() => of({ product: null as any }))
      );
  }

  private normalizeProductsResponse(
    response: GetProductsResponse | GetProductByCategoryResponse | any
  ): GetProductsResponse {
    const products = (response as any)?.products ?? (response as any)?.Products ?? [];

    return {
      products
    };
  }

  private normalizeProductResponse(response: GetProductByIdResponse | any): GetProductByIdResponse {
    const product = (response as any)?.product ?? (response as any)?.Product ?? null;

    return {
      product
    };
  }
}
