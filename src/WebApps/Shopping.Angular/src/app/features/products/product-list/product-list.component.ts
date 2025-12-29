import { Component, OnInit } from '@angular/core';
import { Product } from '../../../core/models/product.model';
import { BasketService } from '../../../core/services/basket.service';
import { CatalogService } from '../../../core/services/catalog.service';
import { productImageUrl } from '../../../core/utils/image-url';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  categories: string[] = [];
  selectedCategory: string | null = null;

  constructor(
    private readonly catalogService: CatalogService,
    private readonly basketService: BasketService
  ) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(category?: string): void {
    const request$ = category
      ? this.catalogService.getProductsByCategory(category)
      : this.catalogService.getProducts();

    request$.subscribe((response) => {
      this.products = category ? response.products : response.products;
      this.categories = Array.from(new Set(response.products.flatMap((p) => p.category)));
      this.selectedCategory = category ?? null;
    });
  }

  onSelectCategory(category: string | null): void {
    this.loadProducts(category || undefined);
  }

  addToCart(product: Product): void {
    this.basketService.addItem(product).subscribe();
  }

  imageUrl(product: Product): string {
    return productImageUrl(product.imageFile, product.name);
  }
}
