import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { API_BASE_URL } from '../config/api.config';
import { AuthService } from './auth.service';
import { AuthenticateUserRequest, AuthenticatedUser } from '../models/auth.model';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  const baseUrl = API_BASE_URL;
  const storageKey = 'shopping-user';

  beforeEach(() => {
    localStorage.clear();
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule]
    });
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should persist the authenticated user on login and expose it via the stream', () => {
    service = TestBed.inject(AuthService);

    const request: AuthenticateUserRequest = { email: 'alice@email.com', password: 'pwd' };
    const response: AuthenticatedUser = { token: 'jwt-token', name: 'Alice', userId: 'user-1' };

    let emitted: AuthenticatedUser | null = null;
    service.currentUser$.subscribe((user) => (emitted = user));

    service.login(request).subscribe((user) => {
      // The login observable should forward the HTTP payload.
      expect(user).toEqual(response);
    });

    const httpRequest = httpMock.expectOne(`${baseUrl}/user-service/authenticate`);
    expect(httpRequest.request.method).toBe('POST');
    expect(httpRequest.request.body).toEqual(request);

    httpRequest.flush(response);

    // BehaviorSubject must now carry the authenticated user and persist it to storage.
    expect(emitted!).toEqual(response);
    expect(localStorage.getItem(storageKey)).toBe(JSON.stringify(response));
    expect(service.isAuthenticated).toBeTrue();
    expect(service.token).toBe('jwt-token');
  });

  it('should clear authentication state on logout', () => {
    const storedUser: AuthenticatedUser = { token: 'token', name: 'Bob', userId: 'user-2' };
    localStorage.setItem(storageKey, JSON.stringify(storedUser));

    // Create the service after seeding storage so the BehaviorSubject picks it up.
    service = TestBed.inject(AuthService);

    service.logout();

    expect(localStorage.getItem(storageKey)).toBeNull();
    service.currentUser$.subscribe((user) => expect(user).toBeNull());
    expect(service.isAuthenticated).toBeFalse();
  });

  it('should ignore malformed cached data and start unauthenticated', () => {
    localStorage.setItem(storageKey, '{not-json');

    // Recreate the service to exercise the loadUser guard.
    service = TestBed.inject(AuthService);

    expect(service.isAuthenticated).toBeFalse();
    service.currentUser$.subscribe((user) => expect(user).toBeNull());
  });
});
