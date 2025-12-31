import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ToastrService } from '../../../shared/services/toastr.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  readonly form = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  constructor(
    private readonly fb: FormBuilder,
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly toastrService: ToastrService
  ) {}

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.toastrService.showWarning('Preencha todos os campos obrigatórios.');
      return;
    }

    this.authService
      .register(this.form.value as any)
      .subscribe({
        next: () => {
          this.toastrService.showInfo('Conta criada com sucesso!');
          this.router.navigate(['/login']);
        },
        error: () => this.toastrService.showDanger('Não foi possível registrar. Tente novamente.')
      });
  }
}
