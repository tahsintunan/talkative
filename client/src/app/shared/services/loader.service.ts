import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoaderService {
  private isLoading = new BehaviorSubject<boolean>(false);

  constructor() {}

  public setIsLoading(): void {
    this.isLoading.next(true);
  }

  public setIsNotLoading(): void {
    this.isLoading.next(false);
  }

  public toggleIsLoading(): void {
    this.isLoading.next(!this.isLoading.value);
  }

  public getIsLoading(): Observable<boolean> {
    return this.isLoading.asObservable();
  }
}
