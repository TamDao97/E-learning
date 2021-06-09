import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatisticalCompleteCourseComponent } from './statistical-complete-course.component';

describe('StatisticalCompleteCourseComponent', () => {
  let component: StatisticalCompleteCourseComponent;
  let fixture: ComponentFixture<StatisticalCompleteCourseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StatisticalCompleteCourseComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StatisticalCompleteCourseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
