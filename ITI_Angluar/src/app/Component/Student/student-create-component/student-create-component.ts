import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../Services/auth-service';
import { DepartmentService } from '../../../Services/department-service';
import { Department } from '../../../Interface/department';
import { RegisterRequest } from '../../../Interface/register-request';

@Component({
  selector: 'app-student-create-component',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './student-create-component.html',
})
export class StudentCreateComponent implements OnInit {
  departments: Department[] = [];
  request: RegisterRequest = {
    name: '',
    email: '',
    password: '',
    age: 18,
    departmentId: 0,
  };
  errorMessage = '';

  constructor(private readonly authService: AuthService,private readonly departmentService: DepartmentService,private readonly router: Router) {}

  ngOnInit(): void {
    this.departmentService.getAll().subscribe({
      next: (departments) => {
        this.departments = departments;
      },
      error: () => {
        this.departments = [];
      },
    });
  }

  submit(): void {
    this.authService.register(this.request).subscribe({
      next: () => this.router.navigate(['/student']),
      error: (err: HttpErrorResponse) => {
        this.errorMessage = this.authService.mapErrorMessage(err);
      },
    });
  }
}
