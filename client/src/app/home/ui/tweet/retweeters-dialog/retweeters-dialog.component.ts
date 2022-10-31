import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { PaginationModel } from 'src/app/home/models/pagination.model';
import { UserModel } from 'src/app/home/models/user.model';
import { TweetService } from 'src/app/home/services/tweet.service';

@Component({
  selector: 'app-retweeters-dialog',
  templateUrl: './retweeters-dialog.component.html',
  styleUrls: ['./retweeters-dialog.component.css'],
})
export class RetweetersDialogComponent implements OnInit {
  pagination: PaginationModel = {
    pageNumber: 1,
  };
  users: UserModel[] = [];

  constructor(
    private router: Router,
    private tweetService: TweetService,
    public dialogRef: MatDialogRef<RetweetersDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string
  ) {}

  ngOnInit(): void {
    this.router.events.subscribe(() => {
      this.dialogRef.close();
    });

    this.pagination.pageNumber = 1;
    this.getRetweeters();
  }

  onScroll() {
    this.pagination.pageNumber++;
    this.getRetweeters();
  }

  getRetweeters() {
    this.tweetService
      .getRetweeters(this.data, this.pagination)
      .subscribe((res) => {
        if (this.pagination.pageNumber === 1) this.users = res;
        else this.users = this.users.concat(res);
      });
  }
}
