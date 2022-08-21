import { UserListService } from './../../services/user-list.service';
import { Component, OnInit } from '@angular/core';
import { ProfileModel } from '../../models/profile.model';
import { HeartbeatService } from '../../services/heartbeat.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
})
export class UserListComponent implements OnInit {
  userList: ProfileModel[] = [];

  constructor(
    private userListService: UserListService,
    private heartbeatService: HeartbeatService
  ) {}

  ngOnInit(): void {
    this.sendHeartbeatWithInterval();
    this.getOnlineUsersWithInterval();
  }

  sendHeartbeatWithInterval() {
    this.sendHeartbeat();
    setInterval(() => {
      this.sendHeartbeat();
    }, 60000);
  }

  sendHeartbeat() {
    this.heartbeatService.sendHeartbeat().subscribe({
      next: () => {},
      error: (err: any) => {
        console.log(err);
      },
    });
  }

  getOnlineUsersWithInterval() {
    this.getOnlineUsers();
    setInterval(() => {
      this.getOnlineUsers();
    }, 60000);
  }

  getOnlineUsers() {
    this.userListService.getOnlineUsers().subscribe({
      next: (res) => {
        this.userList = [...res];
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
}
