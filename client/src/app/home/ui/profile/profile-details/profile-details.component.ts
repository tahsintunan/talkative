import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { UserModel } from 'src/app/home/models/user.model';
import { UtilityService } from 'src/app/shared/services/utility.service';
import { FollowService } from '../../../services/follow.service';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-profile-details',
  templateUrl: './profile-details.component.html',
  styleUrls: ['./profile-details.component.css'],
})
export class ProfileDetailsComponent implements OnInit, OnChanges {
  @Input() data?: UserModel;
  @Input() followers?: UserModel[];
  @Input() followings?: UserModel[];
  @Input() postCount: number = 0;
  @Output() onProfileEdit = new EventEmitter();
  @Output() onPasswordEdit = new EventEmitter();
  @Output() onFollow = new EventEmitter();
  @Output() onUnfollow = new EventEmitter();
  @Output() onBlock = new EventEmitter();
  @Output() onUnblock = new EventEmitter();

  userAuth?: UserModel;
  userBlocked: boolean = false;
  isFollowed: boolean = false;

  constructor(
    private userService: UserService,
    private followService: FollowService,
    protected utilityService: UtilityService
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
      this.userBlocked = !!res.blocked?.some(
        (userId) => userId === this.data?.userId
      );
    });

    this.followService.userFollowings.subscribe((res) => {
      this.isFollowed = res.includes(this.data?.userId!);
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
      this.userBlocked = !!res.blocked?.some(
        (userId) => userId === this.data?.userId
      );
    });

    this.followService.userFollowings.subscribe((res) => {
      this.isFollowed = res.includes(this.data?.userId!);
    });
  }
}
