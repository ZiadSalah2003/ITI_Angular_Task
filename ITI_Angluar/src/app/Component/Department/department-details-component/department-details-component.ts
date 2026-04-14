import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DepartmentService } from '../../../Services/department-service';
import { Department } from '../../../Interface/department';

@Component({
  selector: 'app-department-details-component',
  imports: [CommonModule, RouterLink],
  templateUrl: './department-details-component.html',
})
export class DepartmentDetailsComponent implements OnInit {
  department = signal<Department | null>(null);
  errorMessage = signal('');

  constructor(private readonly route: ActivatedRoute,private readonly departmentService: DepartmentService) {}

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
}
