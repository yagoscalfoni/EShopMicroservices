import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { OrderModel } from '../../core/models/order.model';
import { OrderingService } from '../../core/services/ordering.service';
import { ToastrService } from '../../shared/services/toastr.service';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  orders: OrderModel[] = [];

  constructor(
    private readonly orderingService: OrderingService,
    private readonly toastrService: ToastrService
  ) {}

  ngOnInit(): void {
    this.orderingService.getOrders().subscribe({
      next: (response) => {
        this.orders = response.orders.data;
        if (this.orders.length === 0) {
          this.toastrService.showInfo('Você ainda não possui pedidos. Comece adicionando produtos ao carrinho.');
        }
      },
      error: () => this.toastrService.showDanger('Não foi possível carregar seus pedidos.')
    });
  }
}
