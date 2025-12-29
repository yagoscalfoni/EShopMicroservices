import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Product } from '../../../core/models/product.model';
import { BasketService } from '../../../core/services/basket.service';
import { CatalogService } from '../../../core/services/catalog.service';
import { productImageUrl } from '../../../core/utils/image-url';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {
  product: Product | null = null;
  quantity = 1;
  readonly Math = Math;

  constructor(
    private readonly catalogService: CatalogService,
    private readonly basketService: BasketService,
    private readonly route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.catalogService.getProduct(id).subscribe((response) => (this.product = response.product));
    }
  }

  addToCart(): void {
    if (!this.product) {
      return;
    }

    this.basketService.addItem(this.product, this.quantity).subscribe();
  }

  get productImageUrl(): string {
    return productImageUrl(this.product?.imageFile, this.product?.name ?? 'Produto');
  }
}
