import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UtilityService } from 'src/app/shared/services/utility.service';
import { FollowModel } from '../../../models/follow.model';
import { UserModel } from '../../../models/user.model';
import { FollowService } from '../../../services/follow.service';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-profile-details',
  templateUrl: './profile-details.component.html',
  styleUrls: ['./profile-details.component.css'],
})
export class ProfileDetailsComponent implements OnInit {
  @Input() data?: UserModel;
  @Input() followers: FollowModel[] = [];
  @Input() followings: FollowModel[] = [];
  @Input() postCount: number = 0;
  @Output() onProfileEdit = new EventEmitter();
  @Output() onFollow = new EventEmitter();
  @Output() onUnfollow = new EventEmitter();

  userAuth?: UserModel;

  isFollowed: boolean = false;

  constructor(
    private userService: UserService,
    private followService: FollowService,
    protected utilityService: UtilityService
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.followService.userFollowings.subscribe((res) => {
      this.isFollowed = res.some((item) => item.userId === this.data?.userId);
    });
  }
}
