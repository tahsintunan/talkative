import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { UserAnalyticsModel, UserModel } from 'src/app/home/models/user.model';
import { BlockService } from 'src/app/home/services/block.service';
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
  @Input() analytics?: UserAnalyticsModel;
  @Output() onProfileEdit = new EventEmitter();
  @Output() onPasswordEdit = new EventEmitter();
  @Output() onFollowClick = new EventEmitter();
  @Output() onUnfollowClick = new EventEmitter();
  @Output() onBlockClick = new EventEmitter();
  @Output() onUnblockClick = new EventEmitter();

  userAuth?: UserModel;
  userBlocked: boolean = false;
  isFollowing: boolean = false;

  constructor(
    private userService: UserService,
    private followService: FollowService,
    private blockService: BlockService,
    protected utilityService: UtilityService
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.followService.userFollowings.subscribe((res) => {
      this.isFollowing = res[this.data?.userId!];
    });

    this.blockService.userBlockList.subscribe((res) => {
      this.userBlocked = res[this.data?.userId!];
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.ngOnInit();
  }
}
