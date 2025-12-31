import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ShoppingCart } from '../../core/models/basket.model';
import { BasketService } from '../../core/services/basket.service';
import { ToastrService } from '../../shared/services/toastr.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {
  cart: ShoppingCart | null = null;

  constructor(
    private readonly basketService: BasketService,
    private readonly toastrService: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    this.basketService.getBasket().subscribe({
      next: (cart) => (this.cart = cart),
      error: () => this.toastrService.showDanger('Não foi possível carregar o carrinho.')
    });
  }

  remove(productId: string): void {
    this.basketService.removeItem(productId).subscribe({
      next: (cart) => {
        this.cart = cart;
        this.toastrService.showInfo('Item removido do carrinho.');
      },
      error: () => this.toastrService.showDanger('Não foi possível remover o item do carrinho.')
    });
  }
}
