import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { RouterLink } from '@angular/router';
import { DepartmentService } from '../../../Services/department-service';
import { Department } from '../../../Interface/department';

@Component({
  selector: 'app-department-component',
  imports: [CommonModule, RouterLink],
  templateUrl: './department-component.html',
  styleUrl: './department-component.css',
})
export class DepartmentComponent implements OnInit {
  departments = signal<Department[]>([]);
  errorMessage = signal('');

  constructor(private readonly departmentService: DepartmentService) {}

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.errorMessage.set('');

    this.departmentService.getAll().subscribe({
      next: (data) => {
        this.departments.set(data);
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage.set(this.getErrorMessage(err));
      },
    });
  }

  private getErrorMessage(error: HttpErrorResponse): string {
    if (error.status === 0) {
      return 'Cannot reach backend server.';
    }

    return  error.error ?? 'Department request failed.';
  }
}
