import { Component, OnInit } from '@angular/core';
import { delay } from 'rxjs';
import { IconService } from './icon.service';
import { LoaderService } from './shared/services/loader.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'client';
  loading = false;

  constructor(
    private iconService: IconService,
    private loaderService: LoaderService
  ) {}

  ngOnInit(): void {
    this.loaderService.loadingStatus.pipe(delay(0)).subscribe((loading) => {
      this.loading = loading;
    });
  }
}
