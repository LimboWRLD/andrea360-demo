import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class DynamicService {
 private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getAll<T>(endpoint: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/${endpoint}`);
  }

  getById<T>(endpoint: string, id: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/${endpoint}/${id}`);
  }

  getByPath<T>(url: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/${url}`);
  }

  getPlainTextByPath(url: string): Observable<string> {
    return this.http.get(`${this.baseUrl}/${url}`, { responseType: 'text' });
  }

  create<T,R>(endpoint: string, data: T): Observable<R> {
    return this.http.post<R>(`${this.baseUrl}/${endpoint}`, data);
  }

  update<T,R>(endpoint: string, id: string, data: T): Observable<R> {
    return this.http.put<R>(`${this.baseUrl}/${endpoint}/${id}`, data);
  }

  delete(endpoint: string, id: string | string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${endpoint}/${id}`);
  }

  getImage(imageUrl: string): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/${imageUrl}`, { responseType: 'blob' });
  }

  putByPath<R>(path: string, data: any = null): Observable<R> {
    return this.http.put<R>(`${this.baseUrl}/${path}`, data);
  }

  createPaymentIntent(serviceId: string, userId: string): Observable<string> {
    return this.http.post<string>(`${this.baseUrl}/transactions/create-intent`, { serviceId, userId });
  }
}
