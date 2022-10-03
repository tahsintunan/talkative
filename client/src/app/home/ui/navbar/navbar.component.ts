import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { TweetModel } from '../../models/tweet.model';
import { TweetService } from '../../services/tweet.service';
import { PostMakerDialogComponent } from '../post-maker-dialog/post-maker-dialog.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  navData = [
    {
      text: 'Home',
      link: './feed',
      icon: 'home',
    },
    {
      text: 'Profile',
      link: './profile',
      icon: 'person',
    },
    {
      text: 'Notifications',
      link: './notifications',
      icon: 'notifications',
    },
  ];

  constructor(
    private cookieService: CookieService,
    private tweetService: TweetService,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {}

  onLogout() {
    this.cookieService.delete('authorization');
    this.router.navigate(['/auth/signin']);
  }

  openCreatePostDialog() {
    const dialogRef = this.dialog.open(PostMakerDialogComponent, {
      width: '500px',
    });

    dialogRef.afterClosed().subscribe((result: TweetModel) => {
      if (result) {
        this.tweetService.createTweet(result).subscribe((res) => {
          console.log(res);
        });
      }
    });
  }
}
