import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AuthenticateUserRequest,
  AuthenticateUserResponse,
  AuthenticatedUser,
  RegisterUserRequest,
  RegisterUserResponse
} from '../models/auth.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly baseUrl = environment.apiBaseUrl;
  private readonly storageKey = 'shopping-user';
  private readonly currentUserSubject = new BehaviorSubject<AuthenticatedUser | null>(this.loadUser());
  readonly currentUser$ = this.currentUserSubject.asObservable();

  constructor(private readonly http: HttpClient) {}

  login(request: AuthenticateUserRequest): Observable<AuthenticateUserResponse> {
    return this.http
      .post<AuthenticateUserResponse>(`${this.baseUrl}/user-service/authenticate`, request)
      .pipe(tap((user) => this.persistUser(user)));
  }

  register(request: RegisterUserRequest): Observable<RegisterUserResponse> {
    return this.http.post<RegisterUserResponse>(`${this.baseUrl}/user-service/register`, request);
  }

  logout(): void {
    localStorage.removeItem(this.storageKey);
    this.currentUserSubject.next(null);
  }

  get token(): string | null {
    return this.currentUserSubject.value?.token ?? null;
  }

  get isAuthenticated(): boolean {
    return !!this.token;
  }

  get userId(): string | null {
    return this.currentUserSubject.value?.userId ?? null;
  }

  private persistUser(user: AuthenticatedUser): void {
    localStorage.setItem(this.storageKey, JSON.stringify(user));
    this.currentUserSubject.next(user);
  }

  private loadUser(): AuthenticatedUser | null {
    const stored = localStorage.getItem(this.storageKey);
    if (!stored) {
      return null;
    }

    try {
      return JSON.parse(stored) as AuthenticatedUser;
    } catch {
      return null;
    }
  }
}
