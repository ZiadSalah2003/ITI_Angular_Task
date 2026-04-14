import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { LoginRequest } from '../Interface/login-request';
import { Observable, tap } from 'rxjs';
import { AuthResponse } from '../Interface/auth-response';
import { RegisterRequest } from '../Interface/register-request';
import { ErrorResponse } from '../Interface/error-response';
import { jwtDecode } from 'jwt-decode';

type JwtRolePayload = {
  role?: string | string[];
  Role?: string | string[];
  roles?: string[];
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string | string[];
};

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiBaseUrl = environment.apiBaseUrl;
  private readonly tokenKey = environment.tokenKey;
  
  constructor(private readonly http: HttpClient) {}

  login(payload: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiBaseUrl}/api/student/Login`, payload)
      .pipe(
        tap((response) => {
          if (response?.token) {
            localStorage.setItem(this.tokenKey, response.token);
          }
        })
      );
  }

  register(payload: RegisterRequest): Observable<void> {
    return this.http.post<void>(`${this.apiBaseUrl}/api/student/register`, payload);
  }

  mapErrorMessage(error: HttpErrorResponse): string {
    if (error.status === 0) {
      return 'Cannot reach backend server.';
    }

    const body = error.error as ErrorResponse;
    if (body?.errors?.length) {
      return body.errors.join(' | ');
    }

    if (error.status === 401) {
      return 'Invalid email or password.';
    }

    if (error.status === 409) {
      return 'Email already exists.';
    }

    if (error.status === 400) {
      return 'Invalid data. Please check your inputs.';
    }

    return 'Unexpected error occurred.';
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    return this.getToken() !== null;
  }

  getUserRoles(): string[] {
    const token = this.getToken();
    if (!token) {
      return [];
    }

    try {
      const payload = jwtDecode<JwtRolePayload>(token);
      const roleValue =
        payload.role ??
        payload.Role ??
        payload.roles ??
        payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

      if (Array.isArray(roleValue)) {
        return roleValue;
      }

      if (typeof roleValue === 'string' && roleValue.trim().length > 0) {
        return [roleValue];
      }

      return [];
    } catch {
      return [];
    }
  }

  hasRole(role: string): boolean {
    return this.getUserRoles().some((userRole) => userRole.toLowerCase() === role.toLowerCase());
  }

  isAdmin(): boolean {
    return this.hasRole('Admin');
  }
}
