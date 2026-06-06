import { TestBed } from '@angular/core/testing';

import { Profit } from './profit';

describe('Profit', () => {
  let service: Profit;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Profit);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
