import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BlockService } from 'src/app/core/services/block.service';
import { FollowService } from 'src/app/core/services/follow.service';
import { BlockStore } from 'src/app/core/store/block.store';
import { FollowStore } from 'src/app/core/store/follow.store';
import { UserStore } from 'src/app/core/store/user.store';
import { UserModel } from '../../../../../core/models/user.model';

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
    private userStore: UserStore,
    private followService: FollowService,
    private followStore: FollowStore,
    private blockService: BlockService,
    private blockStore: BlockStore
  ) {}

  ngOnInit(): void {
    this.userStore.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.followStore.userFollowings.subscribe((res) => {
      this.isFollowing = res[this.data?.userId!];
    });

    this.blockStore.userBlockList.subscribe((res) => {
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
