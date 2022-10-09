import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewEncapsulation,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TweetModel } from '../../models/tweet.model';
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
  @Output() onLikeClick = new EventEmitter();
  @Output() onCommentClick = new EventEmitter();

  userAuth?: UserModel;

  constructor(
    private userService: UserService,
    private tweetService: TweetService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });
  }

  onTagClick(event: any) {
    if (event.target.classList.contains('hashtag')) {
      this.onHashtagClick.emit(event.target.innerText);
    }
  }

  onLike() {
    this.onLikeClick.emit(this.data?.id);
  }

  onComment() {
    this.onCommentClick.emit(this.data?.id);
  }

  onRetweet() {
    console.log(this.data);

    const dialogRef = this.dialog.open(PostMakerDialogComponent, {
      width: '500px',
      data: {
        isRetweet: true,
        retweetId: this.data?.isRetweet ? this.data?.retweetId : this.data?.id,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.tweetService.createTweet(result).subscribe((res) => {
          console.log(res);
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

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.tweetService.updateTweet(result).subscribe((res) => {
          console.log(res);
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
