import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TweetStore } from '../../../shared/store/tweet.store';
import { PaginationModel } from '../../models/pagination.model';
import {
  TrendingHashtagModel,
  TweetModel,
  TweetWriteModel,
} from '../../models/tweet.model';
import { UserModel } from '../../models/user.model';
import { TweetService } from '../../services/tweet.service';
import { UserService } from '../../services/user.service';
import { PostMakerDialogComponent } from '../../ui/tweet/post-maker-dialog/post-maker-dialog.component';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css'],
})
export class FeedComponent implements OnInit {
  pagination: PaginationModel = {
    pageNumber: 1,
  };

  userAuth?: UserModel;
  tweets?: TweetModel[];
  topUsers: UserModel[] = [];
  trendingHashtags: TrendingHashtagModel[] = [];

  constructor(
    private userService: UserService,
    private tweetService: TweetService,
    private storeService: TweetStore,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.storeService.tweetList.subscribe((res) => {
      this.tweets = res;
    });

    this.tweetService.getTweets(this.pagination).subscribe();

    this.userService.getTopUsers().subscribe((res) => {
      this.topUsers = res.slice(0, 10);
    });

    this.tweetService.getTrendingHashtags().subscribe((res) => {
      this.trendingHashtags = res.slice(0, 10);
    });
  }

  onScroll() {
    this.pagination.pageNumber++;
    this.tweetService.getTweets(this.pagination).subscribe();
  }

  onScrollToTop() {
    this.pagination.pageNumber = 1;
    this.tweetService.getTweets(this.pagination).subscribe();
  }

  onTrendingHashtagClick(hashtag: string) {
    this.router.navigate(['/home/search'], {
      queryParams: { type: 'hashtag', value: hashtag },
    });
  }

  onCreatePost() {
    const dialogRef = this.dialog.open(PostMakerDialogComponent, {
      width: '500px',
    });

    dialogRef.afterClosed().subscribe((result: TweetWriteModel) => {
      if (result) {
        this.tweetService
          .createTweet({
            text: result.text!,
            hashtags: result.hashtags || [],
          })
          .subscribe();
      }
    });
  }
}
