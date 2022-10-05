import { Component, OnInit } from '@angular/core';
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
    this.loaderService.getIsLoading().subscribe((res) => {
      setTimeout(() => {
        this.loading = res;
      }, 0);
    });
  }
}
