import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UserModel } from 'src/app/core/models/user.model';
import { UserStore } from 'src/app/core/store/user.store';

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
  @Output() onFollowingScroll = new EventEmitter();
  @Output() onFollowersScroll = new EventEmitter();
  @Output() onBlockListScroll = new EventEmitter();

  userAuth?: UserModel;

  constructor(private userStore: UserStore) {}

  ngOnInit(): void {
    this.userStore.userAuth.subscribe((res) => {
      this.userAuth = res;
    });
  }
}
