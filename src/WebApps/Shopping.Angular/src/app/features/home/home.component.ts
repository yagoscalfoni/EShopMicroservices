import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { map } from 'rxjs';
import { Product } from '../../core/models/product.model';
import { CatalogService } from '../../core/services/catalog.service';
import { ProductCardComponent } from '../../shared/components/product-card/product-card.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule, ProductCardComponent],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  // OnPush evita disparos desnecessários de detecção de mudanças, útil quando o componente
  // recebe dados imutáveis/observables e queremos otimizar listas e páginas mais pesadas.
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent {
  // Observable consumido com async pipe para liberar/fechar a inscrição automaticamente e
  // manter a tela reativa sem precisar de lifecycle hooks ou manual subscribe.
  readonly featuredProducts$ = this.catalogService.getProducts(1, 6).pipe(
    map((response) => response.products.slice(0, 3))
  );

  constructor(private readonly catalogService: CatalogService) {}

  // trackBy reduz rerenderizações em *ngFor ao reaproveitar itens com a mesma chave.
  // É indicado em listas moderadas/grandes ou quando usamos ChangeDetectionStrategy.OnPush.
  trackByProductId(_: number, product: Product): string {
    return product.id;
  }
}
