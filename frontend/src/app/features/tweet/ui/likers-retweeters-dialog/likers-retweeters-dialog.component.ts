import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { PaginationModel } from 'src/app/core/models/pagination.model';
import { UserModel } from 'src/app/core/models/user.model';
import { TweetService } from 'src/app/core/services/tweet.service';

interface LikersRetweetersDialogData {
  tweetId: string;
  type: 'likes' | 'retweeters';
}

@Component({
  selector: 'app-likers-retweeters-dialog',
  templateUrl: './likers-retweeters-dialog.component.html',
  styleUrls: ['./likers-retweeters-dialog.component.css'],
})
export class LikersRetweetersDialogComponent implements OnInit {
  pagination: PaginationModel = {
    pageNumber: 1,
  };
  users: UserModel[] = [];

  constructor(
    private router: Router,
    private tweetService: TweetService,
    public dialogRef: MatDialogRef<LikersRetweetersDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: LikersRetweetersDialogData
  ) {}

  ngOnInit(): void {
    this.router.events.subscribe(() => {
      this.dialogRef.close();
    });

    this.pagination.pageNumber = 1;
    if (this.data.type === 'likes') this.getLikers();
    else this.getRetweeters();
  }

  onScroll() {
    this.pagination.pageNumber++;
    if (this.data.type === 'likes') this.getLikers();
    else this.getRetweeters();
  }

  getRetweeters() {
    this.tweetService
      .getRetweeters(this.data.tweetId, this.pagination)
      .subscribe((res) => {
        if (this.pagination.pageNumber === 1) this.users = res;
        else this.users = this.users.concat(res);
      });
  }

  getLikers() {
    this.tweetService
      .getLikedUsers(this.data.tweetId, this.pagination)
      .subscribe((res) => {
        if (this.pagination.pageNumber === 1) this.users = res;
        else this.users = this.users.concat(res);
      });
  }
}
