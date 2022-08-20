import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ProfileModel } from '../../models/profile.model';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
})
export class UserListComponent implements OnInit {
  @Output() onClick = new EventEmitter();

  userList: ProfileModel[] = [
    {
      userId: '1',
      username: 'John Doe',
      email: '',
      dateOfBirth: '2000-01-01',
    },
  ];

  constructor() {}

  ngOnInit(): void {}
}
