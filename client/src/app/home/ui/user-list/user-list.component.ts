import { UserListService } from './../../services/user-list.service';
import { Component, OnInit } from '@angular/core';

import { ProfileModel } from '../../models/profile.model';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
})
export class UserListComponent implements OnInit {
  userList: ProfileModel[] = [
  ];

  constructor(
    private userListService: UserListService
  ) { }

  ngOnInit(): void {
    this.updateOnlineStatusWithInterval()
    this.getOnlineUsersWithInterval()

  }


  updateOnlineStatusWithInterval() {
    this.updateOnlineStatus()
    setInterval(() => {
      this.updateOnlineStatus()
    }, 30000);
  }

  private updateOnlineStatus() {
    this.userListService.updateCurrentUserOnlineStatus().subscribe({
      next: () => {

      },
      error: (err: any) => {
        console.log(err);

      }
    })
  }


  getOnlineUsersWithInterval() {
    this.getOnlineUsers()
    setInterval(() => {
      this.getOnlineUsers()
    }, 30000);
  }

  private getOnlineUsers() {
    this.userListService.getOnlineUsers().subscribe({
      next: res => {
        this.userList = [...res]
      },
      error: err => {
        console.log(err)
      }
    })
  }

  click(value: any) {
    console.log(value.value);
  }
}
