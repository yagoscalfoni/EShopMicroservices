import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ShoppingCart } from '../../core/models/basket.model';
import { BasketService } from '../../core/services/basket.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {
  cart: ShoppingCart | null = null;

  constructor(private readonly basketService: BasketService) {}

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    this.basketService.getBasket().subscribe((cart) => (this.cart = cart));
  }

  remove(productId: string): void {
    this.basketService.removeItem(productId).subscribe((cart) => (this.cart = cart));
  }
}
