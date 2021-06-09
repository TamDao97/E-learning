import { TestBed } from '@angular/core/testing';

import { ReportLearnerService } from './report-learner.service';

describe('ReportLearnerService', () => {
  let service: ReportLearnerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReportLearnerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
