import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { OrderModel } from '../../core/models/order.model';
import { OrderingService } from '../../core/services/ordering.service';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  orders: OrderModel[] = [];

  constructor(private readonly orderingService: OrderingService) {}

  ngOnInit(): void {
    this.orderingService.getOrders().subscribe((response) => {
      this.orders = response.orders.data;
    });
  }
}
