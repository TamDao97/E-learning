import { TestBed } from '@angular/core/testing';

import { HomeLinkService } from './home-link.service';

describe('HomeLinkService', () => {
  let service: HomeLinkService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HomeLinkService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
