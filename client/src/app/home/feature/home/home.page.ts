import { Component, OnInit } from '@angular/core';
import { ProfileModel } from '../../Models/profile.model';
import jwtDecode from 'jwt-decode';
import { CookieService } from 'ngx-cookie-service';
@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.css'],
})
export class Homepage implements OnInit {
  profile: ProfileModel = jwtDecode(this.cookie.get('authorization'));
  constructor(
    private cookie: CookieService
  ) { }

  ngOnInit(): void {
    let decodedUser: any = jwtDecode(this.cookie.get('authorization'))
    this.profile.username = decodedUser.unique_name
    this.profile.dateOfBirth = decodedUser.birthdate
  }
}
