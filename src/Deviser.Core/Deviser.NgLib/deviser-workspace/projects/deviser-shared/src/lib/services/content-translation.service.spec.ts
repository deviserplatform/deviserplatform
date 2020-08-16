import { TestBed } from '@angular/core/testing';

import { ContentTranslationService } from './content-translation.service';

describe('ContentTranslationService', () => {
  let service: ContentTranslationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ContentTranslationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
