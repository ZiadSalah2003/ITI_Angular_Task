import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { DepartmentService } from '../../../Services/department-service';
import { DepartmentRequest } from '../../../Interface/department-request';

@Component({
  selector: 'app-department-edit-component',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './department-edit-component.html',
})
export class DepartmentEditComponent implements OnInit {
  id = 0;
  request: DepartmentRequest = { name: '' };
  errorMessage = '';

  constructor(private readonly route: ActivatedRoute,private readonly departmentService: DepartmentService,private readonly router: Router) {}

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id') ?? 0);
    if (!this.id) {
      this.errorMessage = 'Invalid department id.';
      return;
    }

    this.departmentService.getById(this.id).subscribe({
      next: (department) => {
        this.request = { name: department.name };
      },
      error: () => {
        this.errorMessage = 'Cannot load department.';
      },
    });
  }

  submit(): void {
    const payload: DepartmentRequest = { name: this.request.name.trim() };
    if (!payload.name) {
      this.errorMessage = 'Department name is required.';
      return;
    }

    this.departmentService.update(this.id, payload).subscribe({
      next: () => this.router.navigate(['/department']),
      error: (err: HttpErrorResponse) => {
        this.errorMessage = 'Update department failed.';
      },
    });
  }
}
