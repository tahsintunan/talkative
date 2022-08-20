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
      id: '1',
      username: 'John Doe1',
      email: 'doe1@example.com',
      dateOfBirth: '2000-01-01',
    },
    {
      id: '2',
      username: 'John Doe2',
      email: 'doe2@example.com',
      dateOfBirth: '2000-01-01',
    },
    {
      id: '3',
      username: 'John Doe3',
      email: 'doe3@example.com',
      dateOfBirth: '2000-01-01',
    },
    {
      id: '1',
      username: 'John Doe1',
      email: 'doe1@example.com',
      dateOfBirth: '2000-01-01',
    },
    {
      id: '2',
      username: 'John Doe2',
      email: 'doe2@example.com',
      dateOfBirth: '2000-01-01',
    },
    {
      id: '3',
      username: 'John Doe3',
      email: 'doe3@example.com',
      dateOfBirth: '2000-01-01',
    },
    {
      id: '1',
      username: 'John Doe1',
      email: 'doe1@example.com',
      dateOfBirth: '2000-01-01',
    },
    {
      id: '2',
      username: 'John Doe2',
      email: 'doe2@example.com',
      dateOfBirth: '2000-01-01',
    },
    {
      id: '3',
      username: 'John Doe3',
      email: 'doe3@example.com',
      dateOfBirth: '2000-01-01',
    },
    {
      id: '1',
      username: 'John Doe1',
      email: 'doe1@example.com',
      dateOfBirth: '2000-01-01',
    },
    {
      id: '2',
      username: 'John Doe2',
      email: 'doe2@example.com',
      dateOfBirth: '2000-01-01',
    },
    {
      id: '3',
      username: 'John Doe3',
      email: 'doe3@example.com',
      dateOfBirth: '2000-01-01',
    },
    {
      id: '1',
      username: 'John Doe1',
      email: 'doe1@example.com',
      dateOfBirth: '2000-01-01',
    },
    {
      id: '2',
      username: 'John Doe2',
      email: 'doe2@example.com',
      dateOfBirth: '2000-01-01',
    },
    {
      id: '3',
      username: 'John Doe3',
      email: 'doe3@example.com',
      dateOfBirth: '2000-01-01',
    },
  ];

  constructor() {}

  ngOnInit(): void {}

  click(value: any) {
    console.log(value.value);
  }
}
