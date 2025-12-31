import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export type ToastrType = 'info' | 'warning' | 'danger';

export interface ToastrMessage {
  id: number;
  message: string;
  type: ToastrType;
  timeout: number;
}

@Injectable({ providedIn: 'root' })
export class ToastrService {
  private readonly messagesSubject = new BehaviorSubject<ToastrMessage[]>([]);
  readonly messages$ = this.messagesSubject.asObservable();

  showInfo(message: string, timeout = 4000): void {
    this.enqueue(message, 'info', timeout);
  }

  showWarning(message: string, timeout = 4000): void {
    this.enqueue(message, 'warning', timeout);
  }

  showDanger(message: string, timeout = 5000): void {
    this.enqueue(message, 'danger', timeout);
  }

  dismiss(id: number): void {
    const messages = this.messagesSubject.getValue().filter((toast) => toast.id !== id);
    this.messagesSubject.next(messages);
  }

  private enqueue(message: string, type: ToastrType, timeout: number): void {
    const toast: ToastrMessage = {
      id: Date.now() + Math.floor(Math.random() * 1000),
      message,
      type,
      timeout
    };

    const messages = [...this.messagesSubject.getValue(), toast];
    this.messagesSubject.next(messages);

    setTimeout(() => this.dismiss(toast.id), timeout);
  }
}
