import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EnvService {
  public apiUrl = 'http://localhost:5000/';

  constructor() {}
}
