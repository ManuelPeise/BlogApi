import { Injectable } from '@angular/core';
import { LocalStorageEnum } from '../lib/enums/LocalStorageEnum';

@Injectable({
  providedIn: 'root',
})
export class LocalStorageService<TModel> {
  constructor(private key: LocalStorageEnum) {}

  data = this.getItem();

  getItem(): TModel | null {
    const data = localStorage.getItem(this.key);
    return data ? (JSON.parse(data) as TModel) : null;
  }

  setItem(data: TModel): void {
    localStorage.setItem(this.key, JSON.stringify(data));
  }

  deleteItem(): void {
    localStorage.removeItem(this.key);
  }
}
