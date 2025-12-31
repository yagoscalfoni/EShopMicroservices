import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Product } from '../../../core/models/product.model';
import { BasketService } from '../../../core/services/basket.service';
import { CatalogService } from '../../../core/services/catalog.service';
import { productImageUrl } from '../../../core/utils/image-url';
import { CategoryMenuComponent } from '../../../shared/components/category-menu/category-menu.component';
import { ProductCardComponent } from '../../../shared/components/product-card/product-card.component';
import { ToastrService } from '../../../shared/services/toastr.service';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterModule, CategoryMenuComponent, ProductCardComponent],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  categories: string[] = [];
  selectedCategory: string | null = null;

  constructor(
    private readonly catalogService: CatalogService,
    private readonly basketService: BasketService,
    private readonly toastrService: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(category?: string): void {
    const request$ = category
      ? this.catalogService.getProductsByCategory(category)
      : this.catalogService.getProducts();

    request$.subscribe({
      next: (response) => {
        this.products = response.products;
        this.categories = Array.from(new Set(response.products.flatMap((p) => p.category)));
        this.selectedCategory = category ?? null;
      },
      error: () => this.toastrService.showDanger('Não foi possível carregar os produtos.')
    });
  }

  onSelectCategory(category: string | null): void {
    this.loadProducts(category || undefined);
  }

  addToCart(product: Product): void {
    this.basketService.addItem(product).subscribe({
      next: () => this.toastrService.showInfo(`${product.name} adicionado ao carrinho.`),
      error: () => this.toastrService.showDanger('Não foi possível adicionar o produto ao carrinho.')
    });
  }

  imageUrl(product: Product): string {
    return productImageUrl(product.imageFile, product.name);
  }
}
