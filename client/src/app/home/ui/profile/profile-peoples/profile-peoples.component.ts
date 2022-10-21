import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UserModel } from 'src/app/home/models/user.model';
import { UserService } from 'src/app/home/services/user.service';

@Component({
  selector: 'app-profile-peoples',
  templateUrl: './profile-peoples.component.html',
  styleUrls: ['./profile-peoples.component.css'],
})
export class ProfilePeoplesComponent implements OnInit {
  @Input() profileDetails?: UserModel;
  @Input() followers: UserModel[] = [];
  @Input() followings: UserModel[] = [];
  @Input() blockList: UserModel[] = [];

  @Output() onFollow = new EventEmitter();
  @Output() onUnfollow = new EventEmitter();
  @Output() onBlock = new EventEmitter();
  @Output() onUnblock = new EventEmitter();

  userAuth?: UserModel;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });
  }
}
