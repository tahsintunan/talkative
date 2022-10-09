import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TweetModel } from '../../models/tweet.model';
import { UserModel } from '../../models/user.model';
import { TweetService } from '../../services/tweet.service';
import { UserService } from '../../services/user.service';
import { PostMakerDialogComponent } from '../../ui/post-maker-dialog/post-maker-dialog.component';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css'],
})
export class FeedComponent implements OnInit {
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

    this.tweetService.tweets.subscribe((res) => {
      this.tweets = res;
    });

    this.tweetService.getTweets();
  }

  onCreatePost() {
    const dialogRef = this.dialog.open(PostMakerDialogComponent, {
      width: '500px',
    });

    dialogRef.afterClosed().subscribe((result: TweetModel) => {
      if (result) {
        this.tweetService.createTweet(result);
      }
    });
  }
}
