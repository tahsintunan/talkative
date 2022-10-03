import { Component } from '@angular/core';
import { IconService } from './icon.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'client';

  constructor(private iconService: IconService) {}
}
