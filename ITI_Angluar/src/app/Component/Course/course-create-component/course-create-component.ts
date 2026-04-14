import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { CourseService } from '../../../Services/course-service';
import { CourseRequest } from '../../../Interface/course-request';

@Component({
  selector: 'app-course-create-component',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './course-create-component.html',
})
export class CourseCreateComponent {
  request: CourseRequest = {
    crs_Name: '',
    crs_Description: '',
    duration: 1,
    departmentId: null,
    studentDegrees: [],
  };
  errorMessage = '';

  constructor(
    private readonly courseService: CourseService,
    private readonly router: Router
  ) {}

  submit(): void {
    const payload: CourseRequest = {
      crs_Name: this.request.crs_Name.trim(),
      crs_Description: this.request.crs_Description.trim(),
      duration: Number(this.request.duration),
      departmentId: this.request.departmentId ?? null,
      studentDegrees: [],
    };

    if (!payload.crs_Name) {
      this.errorMessage = 'Course name is required.';
      return;
    }

    this.courseService.create(payload).subscribe({
      next: () => this.router.navigate(['/course']),
      error: (err: HttpErrorResponse) => {
        this.errorMessage = 'Create course failed.';
      },
    });
  }
}