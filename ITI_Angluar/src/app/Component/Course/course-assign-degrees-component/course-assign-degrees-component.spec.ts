import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseAssignDegreesComponent } from './course-assign-degrees-component';

describe('CourseAssignDegreesComponent', () => {
  let component: CourseAssignDegreesComponent;
  let fixture: ComponentFixture<CourseAssignDegreesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseAssignDegreesComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(CourseAssignDegreesComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});