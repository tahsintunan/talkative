import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FollowService } from 'src/app/core/services/follow.service';
import { FollowStore } from 'src/app/core/store/follow.store';
import { UserModel } from '../../../../core/models/user.model';

@Component({
  selector: 'app-user-list-item',
  templateUrl: './user-list-item.component.html',
  styleUrls: ['./user-list-item.component.css'],
})
export class UserListItemComponent implements OnInit {
  @Input() user?: UserModel;

  isFollowing: boolean = false;

  constructor(
    private followService: FollowService,
    private followStore: FollowStore,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.followStore.userFollowings.subscribe((res) => {
      this.isFollowing = res[this.user?.userId!];
    });
  }

  followUser() {
    this.isFollowing = true;
    this.followService.follow(this.user?.userId!).subscribe();
  }

  unfollowUser() {
    this.isFollowing = false;
    this.followService.unfollow(this.user?.userId!).subscribe();
  }
}
