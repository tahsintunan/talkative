import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PaginationModel } from '../../models/pagination.model';
import { TweetModel, TweetWriteModel } from '../../models/tweet.model';
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

  constructor(
    private userService: UserService,
    private tweetService: TweetService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.tweetService.feedTweets.subscribe((res) => {
      this.tweets = res;
    });

    this.tweetService.getTweets(this.pagination).subscribe();
  }

  onScroll() {
    this.pagination.pageNumber++;
    this.tweetService.getTweets(this.pagination).subscribe();
  }

  onScrollToTop() {
    this.pagination.pageNumber = 1;
    this.tweetService.getTweets(this.pagination).subscribe();
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
