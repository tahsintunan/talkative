import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { CommentService } from 'src/app/core/services/comment.service';
import { TweetService } from 'src/app/core/services/tweet.service';
import { UserStore } from 'src/app/core/store/user.store';
import {
  CommentLikeModel,
  CommentModel,
  CommentUpdateModel,
} from '../../../../core/models/comment.model';
import { PaginationModel } from '../../../../core/models/pagination.model';
import { TweetModel } from '../../../../core/models/tweet.model';
import { UserModel } from '../../../../core/models/user.model';

@Component({
  selector: 'app-tweet-details',
  templateUrl: './tweet-details.component.html',
  styleUrls: ['./tweet-details.component.css'],
})
export class TweetDetailsComponent implements OnInit {
  detailedView = true;
  commentToHighlight: string = '';
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
    private userStore: UserStore,
    private tweetService: TweetService,
    private commentService: CommentService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.userStore.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.activatedRoute.params.subscribe((params) => {
      this.tweetId = params['tweetId'];

      this.getTweet();
      this.getComments();
    });
  }

  getCommentToShowAtTop() {
    this.activatedRoute.queryParams.subscribe((res) => {
      this.commentToHighlight = res['comment'];

      if (this.commentToHighlight) {
        const index = this.comments.findIndex(
          (c) => c.id === this.commentToHighlight
        );

        if (index !== -1) {
          const commentToShow = this.comments[index];
          this.comments.splice(index, 1);
          this.comments.unshift(commentToShow);
        } else {
          this.commentService
            .getCommentById(this.commentToHighlight)
            .subscribe((res) => {
              if (res) this.comments.unshift(res);
            });
        }
      }
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
          this.getCommentToShowAtTop();
        });
    }
  }

  onTweetDelete() {
    this.router.navigate(['..']);
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

  onCommentEdit(value: CommentUpdateModel) {
    this.commentService.updateComment(value).subscribe(() => {
      this.comments = this.comments.map((comment) => {
        if (comment.id === value.id) {
          comment.text = value.text;
          comment.lastModified = new Date();
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
