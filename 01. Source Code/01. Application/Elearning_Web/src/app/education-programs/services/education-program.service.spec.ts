import { TestBed } from '@angular/core/testing';

import { EducationProgramService } from './education-program.service';

describe('EducationProgramService', () => {
  let service: EducationProgramService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EducationProgramService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
