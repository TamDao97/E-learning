import { TestBed } from '@angular/core/testing';

import { SystemParamService } from './system-param.service';

describe('SystemParamService', () => {
  let service: SystemParamService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SystemParamService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
