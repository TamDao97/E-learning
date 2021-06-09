import { TestBed } from '@angular/core/testing';

import { ManageUnitService } from './manage-unit.service';

describe('ManageUnitService', () => {
  let service: ManageUnitService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ManageUnitService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
