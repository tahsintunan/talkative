import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoaderService {
  private readonly loadingSubject = new BehaviorSubject<boolean>(false);

  private readonly loadingMap = new Map<string, boolean>();

  public readonly loadingStatus = this.loadingSubject.asObservable();

  constructor() {}

  public setLoading(loading: boolean, url: string): void {
    if (loading === true) {
      this.loadingMap.set(url, loading);
      this.loadingSubject.next(true);
    } else if (loading === false && this.loadingMap.has(url)) {
      this.loadingMap.delete(url);
    }

    if (this.loadingMap.size === 0) {
      this.loadingSubject.next(false);
    }
  }
}
