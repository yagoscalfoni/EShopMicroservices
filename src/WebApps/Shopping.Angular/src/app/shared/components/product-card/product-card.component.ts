import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Product } from '../../../core/models/product.model';
import { productImageUrl } from '../../../core/utils/image-url';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss']
})
export class ProductCardComponent {
  @Input() product!: Product;
  @Output() addToCart = new EventEmitter<void>();

  get imageUrl(): string {
    console.log(this.product);
    return productImageUrl(this.product?.imageFile, this.product?.name ?? 'Produto');
  }
}
