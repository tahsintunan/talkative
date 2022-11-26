import { fakeAsync, tick } from '@angular/core/testing';
import { LoaderService } from './loader.service';

describe('Loader service', () => {
  let service: LoaderService;
  beforeEach(() => {
    service = new LoaderService();
  });

  it('should set loading to url "profile" to true', fakeAsync(() => {
    service.setLoading(true, 'profile');
    let loadingStatus: boolean = false;
    service.loadingStatus.subscribe((res) => {
      loadingStatus = true;
    });
    tick();
    expect(loadingStatus).toBe(true);
  }));
});
