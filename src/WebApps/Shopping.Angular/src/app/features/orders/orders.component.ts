import { Component, OnInit } from '@angular/core';
import { OrderModel } from '../../core/models/order.model';
import { OrderingService } from '../../core/services/ordering.service';

@Component({
  selector: 'app-orders',
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
