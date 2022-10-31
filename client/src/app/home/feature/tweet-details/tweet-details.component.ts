import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import {
  CommentLikeModel,
  CommentModel,
  CommentUpdateModel,
} from '../../models/comment.model';
import { PaginationModel } from '../../models/pagination.model';
import { TweetModel } from '../../models/tweet.model';
import { UserModel } from '../../models/user.model';
import { CommentService } from '../../services/comment.service';
import { RetweetService } from '../../services/retweet.service';
import { TweetService } from '../../services/tweet.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-tweet-details',
  templateUrl: './tweet-details.component.html',
  styleUrls: ['./tweet-details.component.css'],
})
export class TweetDetailsComponent implements OnInit {
  detailedView = true;
  commentToHighlight: string = "";
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

  getCommentToShowAtTop() {
    this.activatedRoute.queryParams.subscribe((res) => {
      this.commentToHighlight = res['comment'];

      if (this.commentToHighlight) {
        const index = this.comments.findIndex((c) => c.id === this.commentToHighlight);
        console.log(index);

        if (index !== -1) {
          const updatedComment = this.comments[index];
          this.comments.splice(index, 1);
          this.comments.unshift(updatedComment)
        } else {
          this.commentService.getCommentById(this.commentToHighlight).subscribe((res) => {
            if (res) this.comments.unshift(res);
          });
        }
      }


      if (this.commentToHighlight) {
          setTimeout(() => {
            this.commentToHighlight = "";
          }, 5000);
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
