import { TestBed } from '@angular/core/testing';

import { BackendService } from './backend.service';

describe('FileuploadService', () => {
  let service: BackendService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BackendService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
