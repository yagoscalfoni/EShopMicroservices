import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateUserRequest, UpdateUserRequest, User } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly baseUrl = `${environment.apiBaseUrl}/user-service`;

  constructor(private readonly http: HttpClient) {}

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.baseUrl}/users`);
  }

  getUser(userId: string): Observable<User> {
    return this.http.get<User>(`${this.baseUrl}/users/${userId}`);
  }

  createUser(request: CreateUserRequest): Observable<User> {
    return this.http.post<User>(`${this.baseUrl}/users`, request);
  }

  updateUser(userId: string, request: UpdateUserRequest): Observable<User> {
    return this.http.put<User>(`${this.baseUrl}/users/${userId}`, request);
  }

  deleteUser(userId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/users/${userId}`);
  }
}
