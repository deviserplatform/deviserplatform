import { TestBed } from '@angular/core/testing';

import { PageModuleService } from './page-module.service';

describe('PageModuleService', () => {
  let service: PageModuleService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PageModuleService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
