import { TestBed } from '@angular/core/testing';

import { LayoutTypeService } from './layout-type.service';

describe('LayoutTypeService', () => {
  let service: LayoutTypeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LayoutTypeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
