import { TestBed } from '@angular/core/testing';

import { CompleteStatisticalService } from './complete-statistical.service';

describe('CompleteStatisticalService', () => {
  let service: CompleteStatisticalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CompleteStatisticalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
