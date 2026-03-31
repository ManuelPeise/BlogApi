import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';

export class HttpService {
  private apiBaseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  sendRequest<T>(
    method: 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE',
    endpoint: string,
    data?: any,
    params?: Record<string, string | number | boolean>,
    headers?: Record<string, string>,
  ) {
    const url = `${this.apiBaseUrl}${endpoint}`;

    return this.http.request<T>(method, url, {
      body: data,
      params,
      headers: new HttpHeaders(headers || {}),
    });
  }
}
