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
import { TweetModel, TweetWriteModel } from '../../models/tweet.model';
import { UserModel } from '../../models/user.model';
import { TweetService } from '../../services/tweet.service';
import { UserService } from '../../services/user.service';
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

  userAuth?: UserModel;

  alreadyLiked: boolean = false;
  alreadyRetweeted: boolean = false;

  constructor(
    private userService: UserService,
    private tweetService: TweetService,
    private dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.alreadyLiked = !!this.data?.likes?.includes(this.userAuth?.id!);
    this.alreadyRetweeted = !!this.data?.retweetUsers?.includes(
      this.userAuth?.id!
    );
  }

  onTweetClick() {
    this.router.navigate([`/home/tweet/${this.data?.id}`]);
  }

  onTagClick(event: any) {
    if (event.target.classList.contains('hashtag')) {
      this.onHashtagClick.emit(event.target.innerText);
    }
  }

  onLike() {
    this.tweetService.likeTweet(this.data?.id!, !this.alreadyLiked);

    if (this.alreadyLiked && this.data) {
      this.data.likes = this.data.likes.filter(
        (likedBy) => likedBy !== this.userAuth?.id
      );
    } else {
      this.data?.likes?.push(this.userAuth?.id!);
    }

    this.alreadyLiked = !this.alreadyLiked;
  }

  onComment() {
    this.onTweetClick();
  }

  onQuickRetweet() {
    this.tweetService.retweet({
      isRetweet: true,
      retweetId: this.data?.isRetweet
        ? this.data?.retweet?.id!
        : this.data?.id!,
      hashtags: [],
    });
  }

  onQuoteRetweet() {
    const dialogRef = this.dialog.open(PostMakerDialogComponent, {
      width: '500px',
      data: {
        isRetweet: true,
        retweetId: this.data?.isRetweet
          ? this.data?.retweet?.id
          : this.data?.id,
      },
    });

    dialogRef.afterClosed().subscribe((result: TweetWriteModel) => {
      if (result) {
        this.tweetService.retweet({
          isRetweet: result.isRetweet!,
          retweetId: result.retweetId!,
          text: result.text,
          hashtags: result.hashtags!,
        });
      }
    });
  }

  onEdit() {
    const dialogRef = this.dialog.open(PostMakerDialogComponent, {
      width: '500px',
      data: {
        isEdit: true,
        id: this.data?.id,
        text: this.data?.text,
        isRetweet: this.data?.isRetweet,
        retweetId: this.data?.retweet?.id,
      },
    });

    dialogRef.afterClosed().subscribe((result: TweetWriteModel) => {
      if (result) {
        this.tweetService.updateTweet({
          id: result.id!,
          text: result.text,
          hashtags: result.hashtags,
          isRetweet: result.isRetweet,
          retweetId: result.retweetId,
        });
      }
    });
  }

  onDelete() {
    if (this.data?.id) {
      this.tweetService.deleteTweet(this.data?.id);
    }
  }
}
