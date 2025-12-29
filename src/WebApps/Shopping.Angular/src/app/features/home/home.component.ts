import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Product } from '../../core/models/product.model';
import { CatalogService } from '../../core/services/catalog.service';
import { ProductCardComponent } from '../../shared/components/product-card/product-card.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule, ProductCardComponent],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  featuredProducts: Product[] = [];

  constructor(private readonly catalogService: CatalogService) {}

  ngOnInit(): void {
    this.catalogService.getProducts(1, 6).subscribe((response) => {
      this.featuredProducts = response.products.slice(0, 3);
    });
  }
}
