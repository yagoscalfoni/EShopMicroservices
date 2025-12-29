import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../core/models/product.model';
import { productImageUrl } from '../../../core/utils/image-url';

@Component({
  selector: 'app-product-card',
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
