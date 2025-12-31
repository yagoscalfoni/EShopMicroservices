import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { of } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { LoginComponent } from './login.component';

describe('LoginComponent', () => {
  let fixture: ComponentFixture<LoginComponent>;
  let router: Router;
  let authService: jasmine.SpyObj<AuthService>;

  beforeEach(() => {
    authService = jasmine.createSpyObj<AuthService>('AuthService', ['login'], { isAuthenticated: false });
    authService.login.and.returnValue(of());

    TestBed.configureTestingModule({
      imports: [LoginComponent, RouterTestingModule.withRoutes([])],
      providers: [{ provide: AuthService, useValue: authService }]
    });

    fixture = TestBed.createComponent(LoginComponent);
    router = TestBed.inject(Router);
  });

  it('should redirect authenticated users automatically', () => {
    const navigateSpy = spyOn(router, 'navigate');
    Object.defineProperty(authService, 'isAuthenticated', { get: () => true });

    fixture.detectChanges();

    expect(navigateSpy).toHaveBeenCalledWith(['/account/resumo']);
  });

  it('should not redirect guests on init', () => {
    const navigateSpy = spyOn(router, 'navigate');

    fixture.detectChanges();

    expect(navigateSpy).not.toHaveBeenCalled();
  });
});
