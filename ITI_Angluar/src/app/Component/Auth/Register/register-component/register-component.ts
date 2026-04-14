import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../Services/auth-service';
import { RegisterRequest } from '../../../../Interface/register-request';

@Component({
  selector: 'app-register-component',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register-component.html',
  styleUrl: './register-component.css',
})
export class RegisterComponent {
  errorMessage = '';
  successMessage = '';
  registerPayload: RegisterRequest = {
    name: '',
    email: '',
    password: '',
    age: 0,
    departmentId: null,
  };

  constructor(private readonly authService: AuthService,private readonly router: Router) {}

  submit(): void {
    this.errorMessage = '';
    this.successMessage = '';

    const rawDepartmentId = this.registerPayload.departmentId;
    const departmentId = rawDepartmentId == null || Number(rawDepartmentId) <= 0
      ? null
      : Number(rawDepartmentId);

    const registerPayload: RegisterRequest = {
      name: this.registerPayload.name,
      email: this.registerPayload.email,
      password: this.registerPayload.password,
      age: Number(this.registerPayload.age),
      departmentId,
    };

    this.authService.register(registerPayload).subscribe({
      next: () => {
        this.successMessage = 'Register success. Redirecting to login.';
        this.router.navigate(['/login']);
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = this.authService.mapErrorMessage(err);
      },
    });
  }
}
