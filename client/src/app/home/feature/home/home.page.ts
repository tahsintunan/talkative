import { Component, OnInit } from '@angular/core';
import { ProfileModel } from '../../models/profile.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.css'],
})
export class Homepage implements OnInit {
  profile: ProfileModel = {
    userId: '1',
    username: 'John Doe',
    email: 'john@example.com',
    dateOfBirth: '2000-01-01',
  };

  selectedUser?: ProfileModel;

  constructor() {}

  ngOnInit(): void {}

  closeChat() {
    console.log('close chat');
    this.selectedUser = undefined;
  }

  onActiveUserClick(user: ProfileModel) {
    console.log('onActiveUserClick', user);
    this.selectedUser = user;
  }
}
