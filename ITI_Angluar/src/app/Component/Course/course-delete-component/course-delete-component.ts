import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CourseService } from '../../../Services/course-service';
import { Course } from '../../../Interface/course';

@Component({
  selector: 'app-course-delete-component',
  imports: [CommonModule, RouterLink],
  templateUrl: './course-delete-component.html',
})
export class CourseDeleteComponent implements OnInit {
  course = signal<Course | null>(null);
  errorMessage = signal('');

  constructor(
    private readonly route: ActivatedRoute,
    private readonly courseService: CourseService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id') ?? 0);
    if (!id) {
      this.errorMessage.set('Invalid course id.');
      return;
    }

    this.courseService.getById(id).subscribe({
      next: (course) => {
        this.course.set(course);
      },
      error: () => {
        this.errorMessage.set('Cannot load course.');
      },
    });
  }

  confirmDelete(): void {
    if (!this.course()) {
      return;
    }

    this.courseService.delete(this.course()!.id).subscribe({
      next: () => this.router.navigate(['/course']),
      error: (err: HttpErrorResponse) => {
        this.errorMessage.set(err.status === 0 ? 'Cannot reach backend server.' : 'Delete course failed.');
      },
    });
  }
}
