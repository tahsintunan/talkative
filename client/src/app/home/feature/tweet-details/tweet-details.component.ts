import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import {
  CommentLikeModel,
  CommentModel,
  CommentUpdateModel,
} from '../../models/comment.model';
import { PaginationModel } from '../../models/pagination.model';
import { TweetModel, TweetWriteModel } from '../../models/tweet.model';
import { UserModel } from '../../models/user.model';
import { CommentService } from '../../services/comment.service';
import { RetweetService } from '../../services/retweet.service';
import { TweetService } from '../../services/tweet.service';
import { UserService } from '../../services/user.service';
import { PostMakerDialogComponent } from '../../ui/tweet/post-maker-dialog/post-maker-dialog.component';

@Component({
  selector: 'app-tweet-details',
  templateUrl: './tweet-details.component.html',
  styleUrls: ['./tweet-details.component.css'],
})
export class TweetDetailsComponent implements OnInit {
  pagination: PaginationModel = {
    pageNumber: 1,
  };

  tweetId?: string;
  tweet?: TweetModel;
  comments: CommentModel[] = [];
  userAuth?: UserModel;

  alreadyLiked: boolean = false;
  alreadyRetweeted: boolean = false;

  constructor(
    private userService: UserService,
    private tweetService: TweetService,
    private retweetService: RetweetService,
    private commentService: CommentService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.activatedRoute.params.subscribe((params) => {
      this.tweetId = params['tweetId'];

      this.getTweet();
      this.getComments();
    });
  }

  onScroll() {
    this.pagination.pageNumber++;
    this.getComments();
  }

  getTweet() {
    if (this.tweetId) {
      this.tweetService.getTweetById(this.tweetId).subscribe((res) => {
        this.tweet = res;

        if (res?.isRetweet) {
          this.tweet = res.originalTweet;
          this.tweetId = res.originalTweetId;
        }

        this.alreadyLiked = !!this.tweet?.likes?.some(
          (like) => like === this.userAuth?.userId
        );

        this.alreadyRetweeted = !!this.tweet?.retweetUsers?.some(
          (retweetBy) => retweetBy === this.userAuth?.userId
        );
      });
    }
  }

  getComments() {
    if (this.tweetId) {
      this.commentService
        .getTweetComments(this.tweetId, this.pagination)
        .subscribe((res) => {
          if (this.pagination.pageNumber === 1) {
            this.comments = res;
          } else {
            this.comments = this.comments.concat(res);
          }
        });
    }
  }

  onTagClick(hashtag: string) {
    this.router.navigate(['/home/search'], {
      queryParams: { type: 'hashtag', value: hashtag },
    });
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

  onComment(commentText: string) {
    if (this.tweetId) {
      this.commentService
        .createComment(this.tweetId, commentText)
        .subscribe((res) => {
          this.comments = [res, ...this.comments];
          this.tweet?.comments?.push(res.id);
        });
    }
  }

  onRetweet() {
    this.retweetService
      .createRetweet({
        isQuoteRetweet: false,
        originalTweetId: this.tweet?.id!,
      })
      .subscribe(() => this.getTweet());
  }

  onQuote() {
    const dialogRef = this.dialog.open(PostMakerDialogComponent, {
      width: '500px',
      data: {
        isQuoteRetweet: true,
        originalTweetId: this.tweet?.id,
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
          .subscribe(() => this.getTweet());
      }
    });
  }

  onRetweetUndo() {
    this.retweetService
      .undoRetweet(this.tweet?.id!)
      .subscribe(() => this.getTweet());
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
          .subscribe(() => this.getTweet());
      }
    });
  }

  onDelete() {
    if (this.tweet?.id) {
      if (this.tweet?.isQuoteRetweet)
        this.retweetService
          .deleteQuoteRetweet(this.tweet.id, this.tweet?.originalTweetId!)
          .subscribe();
      else this.tweetService.deleteTweet(this.tweet?.id).subscribe();
      this.router.navigate(['..']);
    }
  }

  onCommentEdit(value: CommentUpdateModel) {
    this.commentService.updateComment(value).subscribe(() => {
      this.comments = this.comments.map((comment) => {
        if (comment.id === value.id) {
          comment.text = value.text;
        }
        return comment;
      });
    });
  }

  onCommentLike(value: CommentLikeModel) {
    this.commentService.likeComment(value).subscribe(() => {
      this.comments = this.comments.map((comment) =>
        comment.id === value.id
          ? {
              ...comment,
              likes: value.isLiked
                ? [...comment.likes, this.userAuth?.userId!]
                : comment.likes.filter(
                    (like) => like !== this.userAuth?.userId
                  ),
            }
          : comment
      );
    });
  }

  onCommentDelete(commentId: string) {
    this.commentService.deleteComment(commentId).subscribe(() => {
      this.tweet?.comments?.splice(this.tweet?.comments?.indexOf(commentId), 1);
      this.comments = this.comments.filter(
        (comment) => comment.id !== commentId
      );
    });
  }
}
