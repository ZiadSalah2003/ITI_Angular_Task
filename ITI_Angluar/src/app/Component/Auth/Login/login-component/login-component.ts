import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../Services/auth-service';
import { LoginRequest } from '../../../../Interface/login-request';

@Component({
  selector: 'app-login-component',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login-component.html',
  styleUrl: './login-component.css',
})
export class LoginComponent {
  errorMessage = '';
  loginPayload: LoginRequest = {
    email: '',
    password: '',
  };

  constructor(private readonly authService: AuthService,private readonly router: Router) {}

  submit(): void {
    this.errorMessage = '';

    this.authService.login(this.loginPayload).subscribe({
      next: () => {
        this.router.navigate(['/student']);
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = this.authService.mapErrorMessage(err);
      },
    });
  }
}
