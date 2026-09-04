import { TestBed } from '@angular/core/testing';

import { AppLoaderService } from './loader.service';

describe('LoaderService', () => {
  let service: AppLoaderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AppLoaderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
