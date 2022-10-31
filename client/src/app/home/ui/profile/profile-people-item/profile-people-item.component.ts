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
  @Input() showBlockButton: boolean = false;
  
  @Output() onFollow = new EventEmitter();
  @Output() onUnfollow = new EventEmitter();
  @Output() onBlock = new EventEmitter();
  @Output() onUnblock = new EventEmitter();

  userAuth?: UserModel;
  isBlocked: boolean = false;
  isFollowing: boolean = false;

  constructor(
    private userService: UserService,
    private followService: FollowService,
    private blockService: BlockService
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.followService.userFollowings.subscribe((res) => {
      this.isFollowing = res[this.data?.userId!];
    });

    this.blockService.userBlockList.subscribe((res) => {
      this.isBlocked = res[this.data?.userId!];
    });
  }

  follow() {
    this.isFollowing = true;
    this.followService.follow(this.data?.userId!).subscribe((res) => {
      this.onFollow.emit(this.data);
    });
  }

  unfollow() {
    this.isFollowing = false;
    this.followService.unfollow(this.data?.userId!).subscribe((res) => {
      this.onUnfollow.emit(this.data);
    });
  }

  block() {
    this.isBlocked = true;
    this.blockService.blockUser(this.data?.userId!).subscribe((res) => {
      this.onBlock.emit(this.data);
    });
  }

  unblock() {
    this.isBlocked = false;
    this.blockService.unblockUser(this.data?.userId!).subscribe((res) => {
      this.onUnblock.emit(this.data);
    });
  }
}
