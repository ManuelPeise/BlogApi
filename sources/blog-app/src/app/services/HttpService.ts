import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

export class HttpService<TModel> {
  apiBaseUrl = environment.apiBaseUrl;
  responseData: Observable<TModel | null> | null = null;

  constructor(private http: HttpClient) {}

  sendRequest(
    method: string,
    endpoint: string,
    data?: any,
    params?: Record<string, string | number | boolean>,
  ) {
    const url = `${this.apiBaseUrl}${endpoint}`;
    this.responseData = this.http.request<TModel | null>(method, url, { body: data, params });
  }
}
