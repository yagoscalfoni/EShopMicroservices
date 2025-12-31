import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { CreateUserRequest, UpdateUserRequest, User } from '../../core/models/user.model';
import { UserService } from '../../core/services/user.service';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss']
})
export class UserManagementComponent implements OnInit {
  users: User[] = [];
  selectedUserId: string | null = null;
  loading = false;
  errorMessage = '';

  readonly form = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phoneNumber: [''],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  constructor(private readonly fb: FormBuilder, private readonly userService: UserService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading = true;
    this.errorMessage = '';
    this.userService
      .getUsers()
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: (users) => (this.users = users),
        error: () => (this.errorMessage = 'Não foi possível carregar os usuários.')
      });
  }

  submitForm(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { firstName, lastName, email, phoneNumber, password } = this.form.value;

    if (this.selectedUserId) {
      const request: UpdateUserRequest = {
        firstName: firstName ?? '',
        lastName: lastName ?? '',
        email: email ?? '',
        phoneNumber: phoneNumber ?? ''
      };

      this.userService.updateUser(this.selectedUserId, request).subscribe({
        next: (updated) => {
          this.users = this.users.map((u) => (u.userId === updated.userId ? updated : u));
          this.resetForm();
        },
        error: () => (this.errorMessage = 'Não foi possível atualizar o usuário.')
      });
    } else {
      const request: CreateUserRequest = {
        firstName: firstName ?? '',
        lastName: lastName ?? '',
        email: email ?? '',
        phoneNumber: phoneNumber ?? '',
        password: password ?? ''
      };

      this.userService.createUser(request).subscribe({
        next: (created) => {
          this.users = [...this.users, created];
          this.resetForm();
        },
        error: () => (this.errorMessage = 'Não foi possível criar o usuário.')
      });
    }
  }

  startEdit(user: User): void {
    this.selectedUserId = user.userId;
    this.form.patchValue({
      firstName: user.firstName,
      lastName: user.lastName,
      email: user.email,
      phoneNumber: user.phoneNumber,
      password: ''
    });
    this.form.get('password')?.clearValidators();
    this.form.get('password')?.updateValueAndValidity();
  }

  removeUser(userId: string): void {
    this.userService.deleteUser(userId).subscribe({
      next: () => {
        this.users = this.users.filter((u) => u.userId !== userId);
        if (this.selectedUserId === userId) {
          this.resetForm();
        }
      },
      error: () => (this.errorMessage = 'Não foi possível remover o usuário.')
    });
  }

  resetForm(): void {
    this.selectedUserId = null;
    this.errorMessage = '';
    this.form.reset();
    const passwordControl = this.form.get('password');
    passwordControl?.setValidators([Validators.required, Validators.minLength(6)]);
    passwordControl?.updateValueAndValidity();
  }

  get isEditing(): boolean {
    return !!this.selectedUserId;
  }
}
