import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { TweetModel } from '../../models/tweet.model';
import { UserModel, UserUpdateReqModel } from '../../models/user.model';
import { BlockService } from '../../services/block.service';
import { FollowService } from '../../services/follow.service';
import { TweetService } from '../../services/tweet.service';
import { UserService } from '../../services/user.service';
import { ProfileUpdateDialogComponent } from '../../ui/profile/profile-update-dialog/profile-update-dialog.component';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  profileDetails?: UserModel;
  followers: UserModel[] = [];
  followings: UserModel[] = [];
  blockList: UserModel[] = [];
  tweets: TweetModel[] = [];

  constructor(
    private dialog: MatDialog,
    private userService: UserService,
    private blockService: BlockService,
    private tweetService: TweetService,
    private followService: FollowService,
    private activeRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.blockService.userBlockList.subscribe((res) => {
      this.blockList = res;
    });

    this.tweetService.userTweets.subscribe((res) => {
      this.tweets = res;
    });

    this.activeRoute.params.subscribe((params) => {
      const userId = params['userId'];

      if (userId) {
        this.getProfile(userId);
        this.getUserTweets(userId);
        this.getFollowers(userId);
        this.getFollowings(userId);
      }
    });
  }

  getProfile(userId: string) {
    this.userService.getUser(userId).subscribe((res) => {
      this.profileDetails = res;
    });
  }

  getUserTweets(userId: string) {
    this.tweetService.getUserTweets(userId).subscribe();
  }

  getFollowers(userId: string) {
    this.followService.getFollowers(userId).subscribe((res) => {
      this.followers = res;
    });
  }

  getFollowings(userId: string) {
    this.followService.getFollowings(userId).subscribe((res) => {
      this.followings = res;
    });
  }

  onProfileEdit() {
    const dialogRef = this.dialog.open(ProfileUpdateDialogComponent, {
      width: '500px',
      data: {
        username: this.profileDetails?.username,
        email: this.profileDetails?.email,
        dateOfBirth: this.profileDetails?.dateOfBirth,
      },
    });

    dialogRef.afterClosed().subscribe((result: UserUpdateReqModel) => {
      if (result) {
        this.userService.updateProfile(result).subscribe((res) => {
          this.getProfile(this.profileDetails?.userId!);
        });
      }
    });
  }

  onFollow(userId: string) {
    this.followService.followUser(userId).subscribe((res) => {
      this.getFollowers(this.profileDetails?.userId!);
      this.getFollowings(this.profileDetails?.userId!);
    });
  }

  onUnfollow(userId: string) {
    this.followService.unfollowUser(userId).subscribe((res) => {
      this.getFollowers(this.profileDetails?.userId!);
      this.getFollowings(this.profileDetails?.userId!);
    });
  }

  onBlock(userId: string) {
    this.blockService.blockUser(userId).subscribe((res) => {
      this.getFollowers(this.profileDetails?.userId!);
      this.getFollowings(this.profileDetails?.userId!);
      this.blockService.init();
    });
  }

  onUnblock(userId: string) {
    this.blockService.unblockUser(userId).subscribe((res) => {
      this.getFollowers(this.profileDetails?.userId!);
      this.getFollowings(this.profileDetails?.userId!);
      this.blockService.init();
    });
  }
}
