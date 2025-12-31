import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ToastrMessage, ToastrService } from '../../services/toastr.service';

@Component({
  selector: 'app-toastr',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toastr.component.html',
  styleUrls: ['./toastr.component.scss']
})
export class ToastrComponent {
  readonly messages$ = this.toastrService.messages$;

  constructor(private readonly toastrService: ToastrService) {}

  titleFor(type: ToastrMessage['type']): string {
    switch (type) {
      case 'danger':
        return 'Erro';
      case 'info':
        return 'Info';
      case 'warning':
        return 'Atenção';
      default:
        return type;
    }
  }

  dismiss(id: number): void {
    this.toastrService.dismiss(id);
  }

  trackById(_: number, toast: ToastrMessage): number {
    return toast.id;
  }
}
