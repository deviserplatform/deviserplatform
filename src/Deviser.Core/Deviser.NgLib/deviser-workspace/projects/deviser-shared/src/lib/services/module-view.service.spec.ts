import { TestBed } from '@angular/core/testing';

import { ModuleViewService } from './module-view.service';

describe('ModuleViewService', () => {
  let service: ModuleViewService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ModuleViewService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
