import { TestBed } from '@angular/core/testing';

import { HomeSetingService } from './home-seting.service';

describe('HomeSetingService', () => {
  let service: HomeSetingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HomeSetingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
