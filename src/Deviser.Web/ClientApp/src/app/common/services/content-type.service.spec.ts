import { TestBed } from '@angular/core/testing';

import { ContentTypeService } from './content-type.service';

describe('ContentTypeService', () => {
  let service: ContentTypeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ContentTypeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
