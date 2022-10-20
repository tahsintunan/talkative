import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FollowModel } from '../../models/follow.model';
import { UserModel } from '../../models/user.model';
import { FollowService } from '../../services/follow.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-following-item',
  templateUrl: './following-item.component.html',
  styleUrls: ['./following-item.component.css'],
})
export class FollowingItemComponent implements OnInit {
  @Input() data?: FollowModel;
  @Output() onFollow = new EventEmitter();
  @Output() onUnfollow = new EventEmitter();

  userAuth?: UserModel;

  isFollowed: boolean = false;

  constructor(
    private userService: UserService,
    private followService: FollowService
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.followService.userFollowings.subscribe((res) => {
      this.isFollowed = res.some(
        (follow) => follow.userId === this.data?.userId
      );
    });
  }
}
