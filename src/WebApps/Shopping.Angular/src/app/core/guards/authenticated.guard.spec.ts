import { TestBed } from '@angular/core/testing';
import { Router, UrlTree } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { authenticatedGuard } from './authenticated.guard';
import { AuthService } from '../services/auth.service';

describe('authenticatedGuard', () => {
  const createContext = (isAuthenticated: boolean) => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule.withRoutes([])],
      providers: [{ provide: AuthService, useValue: { isAuthenticated } }]
    });
  };

  it('should allow navigation when authenticated', () => {
    createContext(true);

    const result = TestBed.runInInjectionContext(() =>
      authenticatedGuard({} as any, { url: '/account/resumo' } as any)
    );

    expect(result).toBeTrue();
  });

  it('should redirect to login preserving redirect url when not authenticated', () => {
    createContext(false);
    const router = TestBed.inject(Router);

    const result = TestBed.runInInjectionContext(() =>
      authenticatedGuard({} as any, { url: '/account/resumo' } as any)
    );

    expect(result instanceof UrlTree).toBeTrue();
    expect(router.serializeUrl(result as UrlTree)).toBe('/login?redirectUrl=%2Faccount%2Fresumo');
  });
});
