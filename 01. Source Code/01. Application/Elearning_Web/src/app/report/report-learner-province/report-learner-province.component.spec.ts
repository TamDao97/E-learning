import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportLearnerProvinceComponent } from './report-learner-province.component';

describe('ReportLearnerProvinceComponent', () => {
  let component: ReportLearnerProvinceComponent;
  let fixture: ComponentFixture<ReportLearnerProvinceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportLearnerProvinceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportLearnerProvinceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
