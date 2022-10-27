import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserModel } from '../../models/user.model';
import { FollowService } from '../../services/follow.service';

@Component({
  selector: 'app-user-list-item',
  templateUrl: './user-list-item.component.html',
  styleUrls: ['./user-list-item.component.css'],
})
export class UserListItemComponent implements OnInit {
  @Input() user?: UserModel;

  isFollowing: boolean = false;

  constructor(private followService: FollowService, private router: Router) {}

  ngOnInit(): void {
    this.followService.userFollowings.subscribe((res) => {
      this.isFollowing = res[this.user?.userId!];
    });
  }

  followUser() {
    this.followService.followUser(this.user?.userId!).subscribe();
  }

  unfollowUser() {
    this.followService.unfollowUser(this.user?.userId!).subscribe();
  }
}
