import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  constructor(private cookieService: CookieService) {}

  ngOnInit(): void {}

  onLogout() {
    this.cookieService.delete('accessToken');
  }
}
