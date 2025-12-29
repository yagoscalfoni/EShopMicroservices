import { Component, OnInit } from '@angular/core';
import { Product } from '../../core/models/product.model';
import { CatalogService } from '../../core/services/catalog.service';

@Component({
  selector: 'app-home',
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
