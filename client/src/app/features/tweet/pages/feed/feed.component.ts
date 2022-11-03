import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { PaginationModel } from 'src/app/core/models/pagination.model';
import {
  TrendingHashtagModel,
  TweetModel,
  TweetWriteModel,
} from 'src/app/core/models/tweet.model';
import { UserModel } from 'src/app/core/models/user.model';
import { TweetService } from 'src/app/core/services/tweet.service';
import { UserService } from 'src/app/core/services/user.service';
import { TweetStore } from 'src/app/core/store/tweet.store';
import { UserStore } from 'src/app/core/store/user.store';
import { PostMakerDialogComponent } from '../../ui/post-maker-dialog/post-maker-dialog.component';

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
    private userStore: UserStore,
    private userService: UserService,
    private tweetService: TweetService,
    private tweetStore: TweetStore,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.userStore.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.tweetStore.tweetList.subscribe((res) => {
      this.tweets = res;
    });

    this.tweetService.getTweets(this.pagination).subscribe();

    this.userService.getTopUsers().subscribe((res) => {
      this.topUsers = res.slice(0, 6);
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
    this.router.navigate(['/search'], {
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
