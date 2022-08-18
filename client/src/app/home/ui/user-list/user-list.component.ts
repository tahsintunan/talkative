import { Component, OnInit } from '@angular/core';
import { ProfileModel } from '../../models/profile.model';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
})
export class UserListComponent implements OnInit {
  userList: ProfileModel[] = [
    {
      username: 'John Doe',
      email: '',
      dateOfBirth: '2000-01-01',
    },
    {
      username: 'John Doe',
      email: '',
      dateOfBirth: '2000-01-01',
    },
    {
      username: 'John Doe',
      email: '',
      dateOfBirth: '2000-01-01',
    },
  ];

  constructor() {}

  ngOnInit(): void {}
}
