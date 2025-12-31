import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Product } from '../../../core/models/product.model';
import { BasketService } from '../../../core/services/basket.service';
import { CatalogService } from '../../../core/services/catalog.service';
import { productImageUrl } from '../../../core/utils/image-url';
import { ToastrService } from '../../../shared/services/toastr.service';

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
    private readonly route: ActivatedRoute,
    private readonly toastrService: ToastrService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.catalogService.getProduct(id).subscribe({
        next: (response) => (this.product = response.product),
        error: () => this.toastrService.showDanger('Não foi possível carregar o produto selecionado.')
      });
    } else {
      this.toastrService.showWarning('Produto não encontrado.');
    }
  }

  addToCart(): void {
    if (!this.product) {
      this.toastrService.showWarning('Selecione um produto válido antes de adicionar ao carrinho.');
      return;
    }

    this.basketService.addItem(this.product, this.quantity).subscribe({
      next: () => this.toastrService.showInfo(`${this.product?.name} adicionado ao carrinho.`),
      error: () => this.toastrService.showDanger('Não foi possível adicionar o produto ao carrinho.')
    });
  }

  get productImageUrl(): string {
    return productImageUrl(this.product?.imageFile, this.product?.name ?? 'Produto');
  }
}
