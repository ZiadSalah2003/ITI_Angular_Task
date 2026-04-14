import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CourseService } from '../../../Services/course-service';
import { AssignCourseDegreesRequest, StudentDegreeRequest } from '../../../Interface/course-request';

@Component({
  selector: 'app-course-assign-degrees-component',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './course-assign-degrees-component.html',
})
export class CourseAssignDegreesComponent implements OnInit {
  payload = signal<AssignCourseDegreesRequest>({
    departmentId: 0,
    courseId: 0,
    studentDegrees: [{ studentId: '', degree: 0 }],
  });
  errorMessage = signal('');
  courseName = signal('');

  constructor(private readonly route: ActivatedRoute,private readonly courseService: CourseService,private readonly router: Router) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id') ?? 0);
    if (!id) {
      this.errorMessage.set('Invalid course id.');
      return;
    }

    this.courseService.getById(id).subscribe({
      next: (course) => {
        const departmentId = course.departmentId;
        if (departmentId == null || departmentId <= 0) {
          this.errorMessage.set('This course has no department');
          return;
        }

        this.courseName.set(course.crs_Name ?? '');
        this.payload.update((current) => ({
          departmentId,
          courseId: id,
          studentDegrees: current.studentDegrees,
        }));
      },
      error: () => {
        this.errorMessage.set('Cannot load course details');
      },
    });
  }

  addStudentDegreeRow(): void {
    this.payload.update((current) => {
      const nextRows = current.studentDegrees.concat([{ studentId: '', degree: 0 }]);
      return {
        departmentId: current.departmentId,
        courseId: current.courseId,
        studentDegrees: nextRows,
      };
    });
  }

  removeStudentDegreeRow(index: number): void {
    this.payload.update((current) => {
      const next = current.studentDegrees.filter((_, i) => i !== index);
      return {
        departmentId: current.departmentId,
        courseId: current.courseId,
        studentDegrees: next.length > 0 ? next : [{ studentId: '', degree: 0 }],
      };
    });
  }

  updateStudentDegree(index: number, field: keyof StudentDegreeRequest, value: string): void {
    this.payload.update((current) => {
      const rows = current.studentDegrees.map((row, i) => {
        if (i !== index) {
          return {
            studentId: row.studentId,
            degree: row.degree,
          };
        }

        if (field === 'degree') {
          return {
            studentId: row.studentId,
            degree: Number(value),
          };
        }

        return {
          studentId: value,
          degree: row.degree,
        };
      });

      return {
        departmentId: current.departmentId,
        courseId: current.courseId,
        studentDegrees: rows,
      };
    });
  }

  submit(): void {
    if (this.payload().departmentId <= 0) {
      this.errorMessage.set('Course department is missing.');
      return;
    }

    const hasInvalidStudent = this.payload().studentDegrees.some((row) => row.studentId.trim().length === 0);
    if (hasInvalidStudent) {
      this.errorMessage.set('Every student degree row needs a student id.');
      return;
    }

    this.courseService.assignDegrees(this.payload()).subscribe({
      next: () => this.router.navigate(['/course', this.payload().courseId, 'details']),
      error: (err: HttpErrorResponse) => {
        this.errorMessage.set('Assign degrees failed.');
      },
    });
  }
}
