import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-category-menu',
  templateUrl: './category-menu.component.html',
  styleUrls: ['./category-menu.component.scss']
})
export class CategoryMenuComponent {
  @Input() categories: string[] = [];
  @Input() selectedCategory: string | null = null;
  @Output() selectCategory = new EventEmitter<string | null>();
}
