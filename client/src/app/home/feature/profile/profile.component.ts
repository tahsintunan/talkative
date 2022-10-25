import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { PaginationModel } from '../../models/pagination.model';
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
  tweetPagination: PaginationModel = {
    pageNumber: 1,
  };

  followingPagination: PaginationModel = {
    pageNumber: 1,
  };

  followersPagination: PaginationModel = {
    pageNumber: 1,
  };

  blockListPagination: PaginationModel = {
    pageNumber: 1,
  };

  userAuth?: UserModel;
  profileId: string = '';
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
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.tweetService.userTweets.subscribe((res) => {
      this.tweets = res;
    });

    this.activeRoute.params.subscribe((params) => {
      this.profileId = params['userId'];
      this.tweetPagination.pageNumber = 1;

      if (this.profileId) {
        this.getProfile(this.profileId);
        this.getFollowers(this.profileId);
        this.getFollowings(this.profileId);
        this.getBlockList(this.profileId);
        this.getUserTweets(this.profileId);
      }
    });
  }

  onScroll() {
    this.tweetPagination.pageNumber++;
    this.getUserTweets(this.profileId);
  }

  onFollowingScroll() {
    this.followingPagination.pageNumber++;
    this.getFollowings(this.profileId);
  }

  onFollowersScroll() {
    this.followersPagination.pageNumber++;
    this.getFollowers(this.profileId);
  }

  onBlockListScroll() {
    this.blockListPagination.pageNumber++;
    this.getBlockList(this.profileId);
  }

  getProfile(userId: string) {
    this.userService.getUser(userId).subscribe((res) => {
      this.profileDetails = res;
    });
  }

  getUserTweets(userId: string) {
    this.tweetService.getUserTweets(userId, this.tweetPagination).subscribe();
  }

  getFollowers(userId: string) {
    this.followService
      .getFollowers(userId, this.followersPagination)
      .subscribe((res) => {
        if (this.followersPagination.pageNumber === 1) {
          this.followers = res;
        } else {
          this.followers = this.followers.concat(res);
        }
      });
  }

  getFollowings(userId: string) {
    this.followService
      .getFollowings(userId, this.followingPagination)
      .subscribe((res) => {
        if (this.followingPagination.pageNumber === 1) {
          this.followings = res;
        } else {
          this.followings = this.followings.concat(res);
        }
      });
  }

  getBlockList(userId: string) {
    this.blockService
      .getBlockList(userId, this.blockListPagination)
      .subscribe((res) => {
        if (this.blockListPagination.pageNumber === 1) {
          this.blockList = res;
        } else {
          this.blockList = this.blockList.concat(res);
        }
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
          this.getProfile(this.profileId);
        });
      }
    });
  }

  onFollow(userId: string) {
    this.followService.followUser(userId).subscribe((res) => {
      this.reloadData(this.profileId);
    });
  }

  onUnfollow(userId: string) {
    this.followService.unfollowUser(userId).subscribe((res) => {
      this.reloadData(this.profileId);
    });
  }

  onBlock(userId: string) {
    this.blockService.blockUser(userId).subscribe((res) => {
      this.userService.loadUserAuth();
      this.reloadData(this.profileId);
    });
  }

  onUnblock(userId: string) {
    this.blockService.unblockUser(userId).subscribe((res) => {
      this.userService.loadUserAuth();
      this.reloadData(this.profileId);
    });
  }

  reloadData(userId: string) {
    this.followService.loadUserFollow();
    this.getFollowers(userId);
    this.getFollowings(userId);
    this.getBlockList(userId);
  }
}
