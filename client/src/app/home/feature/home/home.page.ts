import { Component, OnInit } from '@angular/core';
import { ProfileModel } from '../../models/profile.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.css'],
})
export class Homepage implements OnInit {
  profile: ProfileModel = {
    username: 'John Doe',
    email: 'john@example.com',
    dateOfBirth: '2000-01-01',
  };
  constructor() {}

  ngOnInit(): void {}
}
