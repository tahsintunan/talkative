import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewEncapsulation,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { RetweetService } from 'src/app/home/services/retweet.service';
import { TweetModel, TweetWriteModel } from '../../../models/tweet.model';
import { UserModel } from '../../../models/user.model';
import { TweetService } from '../../../services/tweet.service';
import { UserService } from '../../../services/user.service';
import { PostMakerDialogComponent } from '../post-maker-dialog/post-maker-dialog.component';

@Component({
  selector: 'app-tweet-item',
  templateUrl: './tweet-item.component.html',
  styleUrls: ['./tweet-item.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class TweetItemComponent implements OnInit {
  @Input() data?: TweetModel;
  @Output() onHashtagClick = new EventEmitter();

  tweet?: TweetModel;

  userAuth?: UserModel;

  alreadyLiked: boolean = false;
  alreadyRetweeted: boolean = false;

  constructor(
    private userService: UserService,
    private tweetService: TweetService,
    private retweetService: RetweetService,
    private dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    if (this.data?.isRetweet) this.tweet = this.data?.originalTweet;
    else this.tweet = this.data;

    this.alreadyLiked = !!this.tweet?.likes?.some(
      (likedBy) => likedBy === this.userAuth?.userId
    );

    this.alreadyRetweeted = !!this.tweet?.retweetUsers?.some(
      (retweetedBy) => retweetedBy === this.userAuth?.userId
    );

    console.log(this.tweet?.retweetUsers);
  }

  onTweetClick() {
    this.router.navigate([`/home/tweet/${this.tweet?.id}`]);
  }

  onTagClick(event: any) {
    if (event.target.classList.contains('hashtag')) {
      this.onHashtagClick.emit(event.target.innerText);
    }
  }

  onLike() {
    this.tweetService
      .likeTweet(this.tweet?.id!, !this.alreadyLiked)
      .subscribe();

    if (this.alreadyLiked && this.tweet) {
      this.tweet.likes = this.tweet.likes.filter(
        (likedBy) => likedBy !== this.userAuth?.userId
      );
    } else {
      this.tweet?.likes?.push(this.userAuth?.userId!);
    }

    this.alreadyLiked = !this.alreadyLiked;
  }

  onComment() {
    this.onTweetClick();
  }

  onRetweet() {
    this.retweetService
      .createRetweet({
        isQuoteRetweet: false,
        originalTweetId: this.tweet?.isQuoteRetweet
          ? this.tweet?.originalTweet?.id!
          : this.tweet?.id!,
      })
      .subscribe();
  }

  onQuote() {
    const dialogRef = this.dialog.open(PostMakerDialogComponent, {
      width: '500px',
      data: {
        isQuoteRetweet: true,
        originalTweetId: this.tweet?.isQuoteRetweet
          ? this.tweet?.originalTweet?.id
          : this.tweet?.id,
      },
    });

    dialogRef.afterClosed().subscribe((result: TweetWriteModel) => {
      if (result) {
        this.retweetService
          .createRetweet({
            isQuoteRetweet: true,
            originalTweetId: result.originalTweetId!,
            text: result.text,
            hashtags: result.hashtags!,
          })
          .subscribe();
      }
    });
  }

  onRetweetUndo() {
    this.retweetService
      .undoRetweet(
        this.tweet?.isQuoteRetweet
          ? this.tweet?.originalTweet?.id!
          : this.tweet?.id!
      )
      .subscribe();
  }

  onEdit() {
    const dialogRef = this.dialog.open(PostMakerDialogComponent, {
      width: '500px',
      data: {
        isEdit: true,
        id: this.tweet?.id,
        text: this.tweet?.text,
        isQuoteRetweet: this.tweet?.isQuoteRetweet,
        originalTweetId: this.tweet?.originalTweet?.id,
      },
    });

    dialogRef.afterClosed().subscribe((result: TweetWriteModel) => {
      if (result) {
        this.tweetService
          .updateTweet({
            id: result.id!,
            text: result.text,
            hashtags: result.hashtags,
          })
          .subscribe();
      }
    });
  }

  onDelete() {
    if (this.tweet?.id) {
      if (this.tweet?.isQuoteRetweet)
        this.retweetService
          .deleteQuoteRetweet(this.tweet.id, this.tweet?.originalTweet?.id!)
          .subscribe();
      else this.tweetService.deleteTweet(this.tweet?.id).subscribe();
    }
  }
}
