import { TestBed } from '@angular/core/testing';

import { ReportLearnerProvinceService } from './report-learner-province.service';

describe('ReportLearnerProvinceService', () => {
  let service: ReportLearnerProvinceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReportLearnerProvinceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
