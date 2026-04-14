import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { RouterLink } from '@angular/router';
import { CourseService } from '../../../Services/course-service';
import { Course } from '../../../Interface/course';

@Component({
  selector: 'app-course-component',
  imports: [CommonModule, RouterLink],
  templateUrl: './course-component.html',
  styleUrl: './course-component.css',
})
export class CourseComponent implements OnInit {
  courses = signal<Course[]>([]);
  errorMessage = signal('');

  constructor(private readonly courseService: CourseService) {}

  ngOnInit(): void {
    this.loadCourses();
  }

  loadCourses(): void {
    this.errorMessage.set('');

    this.courseService.getAll().subscribe({
      next: (courses) => {
        this.courses.set(courses.map((course) => this.normalizeCourse(course)));
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage.set(this.getErrorMessage(err));
      },
    });
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
      crs_Description: source.crsDescription ?? '',
      duration: source.duration,
      departmentId: source.departmentId,
      departmentName: source.departmentName,
      studentDegrees: source.studentDegrees ?? [],
    };
  }

  private getErrorMessage(error: HttpErrorResponse): string {
    if (error.status === 0) {
      return 'Cannot reach backend server.';
    }

    const response = error.error as { detail?: string; title?: string; errors?: unknown } | null;
    if (response?.detail) {
      return response.detail;
    }

    if (response?.title) {
      return response.title;
    }

    return 'Course request failed.';
  }
}
