import { TestBed } from '@angular/core/testing';

import { HomeSettingService } from './home-setting.service';

describe('HomeSettingService', () => {
  let service: HomeSettingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HomeSettingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
