import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentEditComponent } from './student-edit-component';

describe('StudentEditComponent', () => {
  let component: StudentEditComponent;
  let fixture: ComponentFixture<StudentEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudentEditComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(StudentEditComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});