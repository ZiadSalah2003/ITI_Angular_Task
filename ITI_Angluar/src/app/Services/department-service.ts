import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../environments/environment';
import { Department } from '../Interface/department';
import { DepartmentRequest } from '../Interface/department-request';
import { ApiResult } from '../Interface/api-result';
import { PagedValue } from '../Interface/paged-value';

@Injectable({
  providedIn: 'root',
})
export class DepartmentService {
  private readonly apiBaseUrl = `${environment.apiBaseUrl}/api/Department`;

  constructor(private readonly http: HttpClient) {}

  getAll(): Observable<Department[]> {
    return this.http
      .get<ApiResult<PagedValue<Department>>>(this.apiBaseUrl)
      .pipe(map((response) => response.value?.items ?? []));
  }

  getById(id: number): Observable<Department> {
    return this.http.get<Department>(`${this.apiBaseUrl}/${id}`);
  }

  create(payload: DepartmentRequest): Observable<Department> {
    return this.http.post<Department>(this.apiBaseUrl, payload);
  }

  update(id: number, payload: DepartmentRequest): Observable<void> {
    return this.http.put<void>(`${this.apiBaseUrl}/${id}`, {
      id,
      name: payload.name,
    });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiBaseUrl}/${id}`);
  }
}
