import { Routes } from '@angular/router';
import { LoginComponent } from './Component/Auth/Login/login-component/login-component';
import { RegisterComponent } from './Component/Auth/Register/register-component/register-component';
import { StudentComponent } from './Component/Student/student-component/student-component';
import { DepartmentComponent } from './Component/Department/department-component/department-component';
import { CourseComponent } from './Component/Course/course-component/course-component';
import { DepartmentCreateComponent } from './Component/Department/department-create-component/department-create-component';
import { DepartmentEditComponent } from './Component/Department/department-edit-component/department-edit-component';
import { DepartmentDetailsComponent } from './Component/Department/department-details-component/department-details-component';
import { DepartmentDeleteComponent } from './Component/Department/department-delete-component/department-delete-component';
import { CourseCreateComponent } from './Component/Course/course-create-component/course-create-component';
import { CourseEditComponent } from './Component/Course/course-edit-component/course-edit-component';
import { CourseDetailsComponent } from './Component/Course/course-details-component/course-details-component';
import { CourseDeleteComponent } from './Component/Course/course-delete-component/course-delete-component';
import { CourseAssignDegreesComponent } from './Component/Course/course-assign-degrees-component/course-assign-degrees-component';
import { StudentCreateComponent } from './Component/Student/student-create-component/student-create-component';
import { StudentEditComponent } from './Component/Student/student-edit-component/student-edit-component';
import { StudentDetailsComponent } from './Component/Student/student-details-component/student-details-component';
import { StudentDeleteComponent } from './Component/Student/student-delete-component/student-delete-component';
import { adminGuard } from './Guards/admin-guard';
import { authGuard } from './Guards/auth-guard';
import { NotFoundComponent } from './Component/Common/not-found-component/not-found-component';

export const routes: Routes = [
	{ path: '', redirectTo: 'login', pathMatch: 'full' },
	{ path: 'login', component: LoginComponent },
	{ path: 'register', component: RegisterComponent },
	{ path: 'student', component: StudentComponent, canActivate: [authGuard] },
	{ path: 'student/create', component: StudentCreateComponent, canActivate: [authGuard] },
	{ path: 'student/:id/edit', component: StudentEditComponent, canActivate: [authGuard] },
	{ path: 'student/:id/details', component: StudentDetailsComponent, canActivate: [authGuard] },
	{ path: 'student/:id/delete', component: StudentDeleteComponent, canActivate: [authGuard] },
	{ path: 'course', component: CourseComponent, canActivate: [authGuard] },
	{ path: 'course/create', component: CourseCreateComponent, canActivate: [authGuard] },
	{ path: 'course/:id/edit', component: CourseEditComponent, canActivate: [authGuard] },
	{ path: 'course/:id/details', component: CourseDetailsComponent, canActivate: [authGuard] },
	{ path: 'course/:id/assign-degrees', component: CourseAssignDegreesComponent, canActivate: [authGuard] },
	{ path: 'course/:id/delete', component: CourseDeleteComponent, canActivate: [authGuard] },
	{ path: 'department', component: DepartmentComponent, canActivate: [adminGuard] },
	{ path: 'department/create', component: DepartmentCreateComponent, canActivate: [adminGuard] },
	{ path: 'department/:id/edit', component: DepartmentEditComponent, canActivate: [adminGuard] },
	{ path: 'department/:id/details', component: DepartmentDetailsComponent, canActivate: [adminGuard] },
	{ path: 'department/:id/delete', component: DepartmentDeleteComponent, canActivate: [adminGuard] },
	{ path: 'not-found', component: NotFoundComponent },
	{ path: '**', component: NotFoundComponent }
];
