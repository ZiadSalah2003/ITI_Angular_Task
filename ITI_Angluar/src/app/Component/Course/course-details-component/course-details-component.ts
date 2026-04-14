import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CourseService } from '../../../Services/course-service';
import { Course } from '../../../Interface/course';

@Component({
  selector: 'app-course-details-component',
  imports: [CommonModule, RouterLink],
  templateUrl: './course-details-component.html',
})
export class CourseDetailsComponent implements OnInit {
  course = signal<Course | null>(null);
  errorMessage = signal('');

  constructor(
    private readonly route: ActivatedRoute,
    private readonly courseService: CourseService
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
}
