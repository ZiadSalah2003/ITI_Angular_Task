import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { StudentService } from '../../../Services/student-service';
import { DepartmentService } from '../../../Services/department-service';
import { StudentRequest } from '../../../Interface/student-request';
import { Department } from '../../../Interface/department';

@Component({
  selector: 'app-student-edit-component',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './student-edit-component.html',
})
export class StudentEditComponent implements OnInit {
  id = '';
  departments = signal<Department[]>([]);
  request = signal<StudentRequest>({
    name: '',
    age: 18,
    departmentId: 0,
  });
  errorMessage = signal('');

  constructor(private readonly route: ActivatedRoute,private readonly studentService: StudentService,private readonly departmentService: DepartmentService,private readonly router: Router) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id') ?? '';
    if (!this.id) {
      this.errorMessage.set('Invalid student id.');
      return;
    }

    this.departmentService.getAll().subscribe({
      next: (departments) => {
        this.departments.set(departments);
        this.loadStudent();
      },
      error: () => {
        this.errorMessage.set('Cannot load departments.');
      },
    });
  }

  private loadStudent(): void {
    this.studentService.getAll().subscribe({
      next: (students) => {
        const student = students.find((item) => item.id === this.id);
        if (!student) {
          this.errorMessage.set('Student not found.');
          return;
        }

        const matchedDepartment = this.departments().find(
          (department) => department.name.trim().toLowerCase() === student.departmentName.trim().toLowerCase()
        );

        this.request.set({
          name: student.name,
          age: student.age,
          departmentId: matchedDepartment?.id ?? 0,
        });
      },
      error: () => {
        this.errorMessage.set('Cannot load student.');
      },
    });
  }

  submit(): void {
    this.studentService.update(this.id, this.request()).subscribe({
      next: () => this.router.navigate(['/student']),
      error: (err: HttpErrorResponse) => {
        this.errorMessage.set('Update student failed.');
      },
    });
  }
}
