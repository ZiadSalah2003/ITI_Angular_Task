import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CourseService } from '../../../Services/course-service';
import { CourseRequest } from '../../../Interface/course-request';

@Component({
  selector: 'app-course-edit-component',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './course-edit-component.html',
})
export class CourseEditComponent implements OnInit {
  id = 0;
  request: CourseRequest = {
    crs_Name: '',
    crs_Description: '',
    duration: 1,
    departmentId: null,
    studentDegrees: [],
  };
  errorMessage = '';

  constructor(
    private readonly route: ActivatedRoute,
    private readonly courseService: CourseService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id') ?? 0);
    if (!this.id) {
      this.errorMessage = 'Invalid course id.';
      return;
    }

    this.courseService.getById(this.id).subscribe({
      next: (course) => {
        this.request = {
          crs_Name: course.crs_Name ?? '',
          crs_Description: course.crs_Description ?? '',
          duration: course.duration,
          departmentId: course.departmentId ?? null,
          studentDegrees: [],
        };
      },
      error: () => {
        this.errorMessage = 'Cannot load course.';
      },
    });
  }

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

    this.courseService.update(this.id, payload).subscribe({
      next: () => this.router.navigate(['/course']),
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.status === 0 ? 'Cannot reach backend server.' : 'Update course failed.';
      },
    });
  }
}
