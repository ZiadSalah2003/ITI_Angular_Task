import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { StudentService } from '../../../Services/student-service';
import { Student } from '../../../Interface/student';

@Component({
  selector: 'app-student-delete-component',
  imports: [CommonModule, RouterLink],
  templateUrl: './student-delete-component.html',
})
export class StudentDeleteComponent implements OnInit {
  student = signal<Student | null>(null);
  errorMessage = signal('');

  constructor(private readonly route: ActivatedRoute,private readonly studentService: StudentService,private readonly router: Router) {}

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

  confirmDelete(): void {
    if (!this.student()) {
      return;
    }

    this.studentService.delete(this.student()!.id).subscribe({
      next: () => this.router.navigate(['/student']),
      error: (err: HttpErrorResponse) => {
        this.errorMessage.set(err.status === 0 ? 'Cannot reach backend server.' : 'Delete student failed.');
      },
    });
  }
}
