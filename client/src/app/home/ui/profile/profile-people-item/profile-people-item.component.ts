import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BlockService } from 'src/app/home/services/block.service';
import { UserModel } from '../../../models/user.model';
import { FollowService } from '../../../services/follow.service';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-profile-people-item',
  templateUrl: './profile-people-item.component.html',
  styleUrls: ['./profile-people-item.component.css'],
})
export class ProfilePeopleItemComponent implements OnInit {
  @Input() data?: UserModel;
  @Output() onFollow = new EventEmitter();
  @Output() onUnfollow = new EventEmitter();
  @Output() onBlock = new EventEmitter();
  @Output() onUnblock = new EventEmitter();

  userAuth?: UserModel;
  userBlocked: boolean = false;
  isFollowed: boolean = false;

  constructor(
    private userService: UserService,
    private followService: FollowService
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
      this.userBlocked = !!res.blocked?.some(
        (userId) => userId === this.data?.userId
      );
    });

    this.followService.userFollowings.subscribe((res) => {
      this.isFollowed = res.some(
        (follow) => follow.userId === this.data?.userId
      );
    });
  }
}
