import { TestBed } from '@angular/core/testing';
import { Router, UrlTree } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { redirectAuthenticatedGuard } from './redirect-authenticated.guard';
import { AuthService } from '../services/auth.service';

describe('redirectAuthenticatedGuard', () => {
  const createContext = (isAuthenticated: boolean) => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule.withRoutes([])],
      providers: [{ provide: AuthService, useValue: { isAuthenticated } }]
    });
  };

  it('should redirect authenticated users away from login', () => {
    createContext(true);
    const router = TestBed.inject(Router);

    const result = TestBed.runInInjectionContext(() => redirectAuthenticatedGuard({} as any, {} as any));

    expect(result instanceof UrlTree).toBeTrue();
    expect(router.serializeUrl(result as UrlTree)).toBe('/account/resumo');
  });

  it('should allow guests to continue', () => {
    createContext(false);

    const result = TestBed.runInInjectionContext(() => redirectAuthenticatedGuard({} as any, {} as any));

    expect(result).toBeTrue();
  });
});
