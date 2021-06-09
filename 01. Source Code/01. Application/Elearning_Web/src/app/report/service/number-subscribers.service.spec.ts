import { TestBed } from '@angular/core/testing';

import { NumberSubscribersService } from './number-subscribers.service';

describe('NumberSubscribersService', () => {
  let service: NumberSubscribersService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NumberSubscribersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
