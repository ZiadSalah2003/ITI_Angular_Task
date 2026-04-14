import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { StudentService } from '../../../Services/student-service';
import { Student } from '../../../Interface/student';

@Component({
  selector: 'app-student-details-component',
  imports: [CommonModule, RouterLink],
  templateUrl: './student-details-component.html',
})
export class StudentDetailsComponent implements OnInit {
  student = signal<Student | null>(null);
  errorMessage = signal('');

  constructor(
    private readonly route: ActivatedRoute,
    private readonly studentService: StudentService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id') ?? '';
    if (!id) {
      this.errorMessage.set('Invalid student id.');
      return;
    }

    this.studentService.getAll().subscribe({
      next: (students) => {
        this.student.set(students.find((item) => item.id === id) ?? null);
        if (!this.student()) {
          this.errorMessage.set('Student not found.');
        }
      },
      error: () => {
        this.errorMessage.set('Cannot load student.');
      },
    });
  }
}
