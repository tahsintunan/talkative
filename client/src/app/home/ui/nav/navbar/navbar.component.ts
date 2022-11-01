import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/services/auth.service';
import { TweetWriteModel } from 'src/app/home/models/tweet.model';
import { NotificationService } from 'src/app/home/services/notification.service';
import { TweetService } from 'src/app/home/services/tweet.service';
import { UserStore } from 'src/app/shared/store/user.store';
import { PostMakerDialogComponent } from '../../tweet/post-maker-dialog/post-maker-dialog.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
  userId?: string;

  navData = [
    {
      text: 'Explore',
      link: './feed',
      icon: 'explore',
    },
    {
      text: 'Profile',
      link: `./profile/${this.userId}`,
      icon: 'person',
    },
    {
      text: 'Notifications',
      link: './notifications',
      icon: 'notifications',
      notificationCount: 0,
    },
  ];

  constructor(
    private authServie: AuthService,
    private userStore: UserStore,
    private notificationService: NotificationService,
    private tweetService: TweetService,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.userStore.userAuth.subscribe((res) => {
      this.userId = res.userId;
      this.navData[1].link = `./profile/${this.userId}`;
    });

    this.notificationService.notifications.subscribe((res) => {
      this.navData[2].notificationCount = res.reduce(
        (acc, curr) => (curr.isRead ? acc : acc + 1),
        0
      );
    });
  }

  onLogout() {
    this.authServie.signout();
    this.router.navigate(['/auth/signin']);
  }

  openCreatePostDialog() {
    const dialogRef = this.dialog.open(PostMakerDialogComponent, {
      width: '500px',
    });

    dialogRef.afterClosed().subscribe((result: TweetWriteModel) => {
      if (result) {
        this.tweetService
          .createTweet({
            text: result.text!,
            hashtags: result.hashtags || [],
          })
          .subscribe();
      }
    });
  }
}
