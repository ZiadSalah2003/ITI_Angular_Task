import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../environments/environment';
import { ApiResult } from '../Interface/api-result';
import { PagedValue } from '../Interface/paged-value';
import { Student } from '../Interface/student';
import { StudentRequest } from '../Interface/student-request';

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  private readonly apiBaseUrl = `${environment.apiBaseUrl}/api/Student`;

  constructor(private readonly http: HttpClient) {}

  getAll(pageNumber = 1, pageSize = 1000): Observable<Student[]> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    return this.http
      .get<ApiResult<PagedValue<Student>>>(this.apiBaseUrl, { params })
      .pipe(map((response) => response.value?.items ?? []));
  }

  getById(id: number | string): Observable<Student> {
    return this.http.get<Student>(`${this.apiBaseUrl}/${id}`);
  }

  update(id: string, payload: StudentRequest): Observable<Student> {
    return this.http.put<Student>(`${this.apiBaseUrl}/${id}`, {
      id,
      name: payload.name,
      age: payload.age,
      departmentId: payload.departmentId,
    });
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiBaseUrl}/${id}`);
  }
}