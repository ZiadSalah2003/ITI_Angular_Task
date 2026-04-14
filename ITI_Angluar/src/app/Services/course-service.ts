import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../environments/environment';
import { ApiResult } from '../Interface/api-result';
import { PagedValue } from '../Interface/paged-value';
import { Course } from '../Interface/course';
import { AssignCourseDegreesRequest, CourseRequest } from '../Interface/course-request';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  private readonly apiBaseUrl = `${environment.apiBaseUrl}/api/Course`;

  constructor(private readonly http: HttpClient) {}

  getAll(): Observable<Course[]> {
    return this.http
      .get<ApiResult<PagedValue<Course>>>(this.apiBaseUrl)
      .pipe(map((response) => (response.value?.items ?? []).map((item) => this.normalizeCourse(item))));
  }

  getById(id: number): Observable<Course> {
    return this.http.get<Course>(`${this.apiBaseUrl}/${id}`).pipe(map((item) => this.normalizeCourse(item)));
  }

  create(payload: CourseRequest): Observable<Course> {
    return this.http.post<Course>(this.apiBaseUrl, payload).pipe(map((item) => this.normalizeCourse(item)));
  }

  update(id: number, payload: CourseRequest): Observable<Course> {
    return this.http
      .put<Course>(`${this.apiBaseUrl}/${id}`, {
        id,
        crs_Name: payload.crs_Name,
        crs_Description: payload.crs_Description,
        duration: payload.duration,
        departmentId: payload.departmentId,
        studentDegrees: payload.studentDegrees,
      })
      .pipe(map((item) => this.normalizeCourse(item)));
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiBaseUrl}/${id}`);
  }

  assignDegrees(payload: AssignCourseDegreesRequest): Observable<Course> {
    return this.http
      .post<Course>(`${this.apiBaseUrl}/assign-degrees`, payload)
      .pipe(map((item) => this.normalizeCourse(item)));
  }

  private normalizeCourse(course: Course): Course {
    const source = course as Course & {
      crsName?: string | null;
      crsDescription?: string | null;
      studentDegrees?: Course['studentDegrees'];
    };

    return {
      id: source.id,
      crs_Name: source.crs_Name ?? source.crsName ?? '',
      crs_Description: source.crs_Description ?? source.crsDescription ?? '',
      duration: source.duration,
      departmentId: source.departmentId,
      departmentName: source.departmentName,
      studentDegrees: source.studentDegrees ?? [],
    };
  }
}