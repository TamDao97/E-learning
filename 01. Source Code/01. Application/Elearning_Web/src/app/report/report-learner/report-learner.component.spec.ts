import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportLearnerComponent } from './report-learner.component';

describe('ReportLearnerComponent', () => {
  let component: ReportLearnerComponent;
  let fixture: ComponentFixture<ReportLearnerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportLearnerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportLearnerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
