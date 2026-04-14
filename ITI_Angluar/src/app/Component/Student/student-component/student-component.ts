import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { RouterLink } from '@angular/router';
import { StudentService } from '../../../Services/student-service';
import { Student } from '../../../Interface/student';

@Component({
  selector: 'app-student-component',
  imports: [CommonModule, RouterLink],
  templateUrl: './student-component.html',
  styleUrl: './student-component.css',
})
export class StudentComponent implements OnInit {
  students = signal<Student[]>([]);
  errorMessage = signal('');

  constructor(private readonly studentService: StudentService) {}

  ngOnInit(): void {
    this.loadStudents();
  }

  loadStudents(): void {
    this.errorMessage.set('');

    this.studentService.getAll().subscribe({
      next: (students) => {
        const studentsWithDepartment = students.filter((student) => {
          const departmentName = student.departmentName;
          return departmentName !== null && departmentName.trim() !== '';
        });

        this.students.set(studentsWithDepartment);
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

    const response = error.error as { detail?: string; title?: string } | null;
    if (response?.detail) {
      return response.detail;
    }

    if (response?.title) {
      return response.title;
    }

    return 'Student request failed.';
  }
}
