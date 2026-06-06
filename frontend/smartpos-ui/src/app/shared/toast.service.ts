import { Injectable, signal } from '@angular/core';
import { ToastType } from './toast/toast';

@Injectable({ providedIn: 'root' })
export class ToastService {

  message = signal('');
  type = signal<ToastType>('success');
  visible = signal(false);

  show(message: string, type: ToastType = 'success') {
    this.message.set(message);
    this.type.set(type);
    this.visible.set(true);

    setTimeout(() => {
      this.visible.set(false);
    }, 3000);
  }

  success(message: string) {
    this.show(message, 'success');
  }

  error(message: string) {
    this.show(message, 'danger');
  }

  warning(message: string) {
    this.show(message, 'warning');
  }

}