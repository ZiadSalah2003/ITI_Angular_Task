import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { DepartmentService } from '../../../Services/department-service';
import { DepartmentRequest } from '../../../Interface/department-request';

@Component({
  selector: 'app-department-create-component',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './department-create-component.html',
})
export class DepartmentCreateComponent {
  request: DepartmentRequest = { name: '' };
  errorMessage = '';

  constructor(private readonly departmentService: DepartmentService,private readonly router: Router) {}

  submit(): void {
    const payload: DepartmentRequest = { name: this.request.name.trim() };
    if (!payload.name) {
      this.errorMessage = 'Department name is required.';
      return;
    }

    this.departmentService.create(payload).subscribe({
      next: () => this.router.navigate(['/department']),
      error: (err: HttpErrorResponse) => {
        this.errorMessage = 'Create department failed.';
      },
    });
  }
}