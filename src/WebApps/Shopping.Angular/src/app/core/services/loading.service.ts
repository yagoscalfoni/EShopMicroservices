import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class LoadingService {
  private loadingCount = 0;
  private loadingSubject = new BehaviorSubject<boolean>(false);

  get isLoading$(): Observable<boolean> {
    return this.loadingSubject.asObservable();
  }

  show(): void {
    this.loadingCount++;
    this.updateLoadingState();
  }

  hide(): void {
    this.loadingCount = Math.max(this.loadingCount - 1, 0);
    this.updateLoadingState();
  }

  private updateLoadingState(): void {
    this.loadingSubject.next(this.loadingCount > 0);
  }
}
