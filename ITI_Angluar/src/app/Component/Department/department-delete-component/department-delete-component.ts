import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { DepartmentService } from '../../../Services/department-service';
import { Department } from '../../../Interface/department';

@Component({
  selector: 'app-department-delete-component',
  imports: [CommonModule, RouterLink],
  templateUrl: './department-delete-component.html',
})
export class DepartmentDeleteComponent implements OnInit {
  department = signal<Department | null>(null);
  errorMessage = signal('');

  constructor(private readonly route: ActivatedRoute,private readonly departmentService: DepartmentService,private readonly router: Router) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id') ?? 0);
    if (!id) {
      this.errorMessage.set('Invalid department id.');
      return;
    }

    this.departmentService.getById(id).subscribe({
      next: (department) => {
        this.department.set(department);
      },
      error: () => {
        this.errorMessage.set('Cannot load department.');
      },
    });
  }

  confirmDelete(): void {
    if (!this.department()) {
      return;
    }

    this.departmentService.delete(this.department()!.id).subscribe({
      next: () => this.router.navigate(['/department']),
      error: (err: HttpErrorResponse) => {
        this.errorMessage.set('Delete department failed.');
      },
    });
  }
}
