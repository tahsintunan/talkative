import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import jwtDecode from 'jwt-decode';
import { CookieService } from 'ngx-cookie-service';
import { TweetWriteModel } from 'src/app/core/models/tweet.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { TweetService } from 'src/app/core/services/tweet.service';
import { NotificationStore } from 'src/app/core/store/notification.store';
import { UserStore } from 'src/app/core/store/user.store';
import { PostMakerDialogComponent } from 'src/app/features/tweet/ui/post-maker-dialog/post-maker-dialog.component';

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
      link: 'tweet',
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
    {
      text: 'Admin Panel',
      link: './admin',
      icon: 'admin_panel_settings',
      hidden: true,
    },
  ];

  constructor(
    private authServie: AuthService,
    private userStore: UserStore,
    private notificationStore: NotificationStore,
    private tweetService: TweetService,
    private router: Router,
    public dialog: MatDialog,
    public cookieService: CookieService
  ) {}

  ngOnInit(): void {
    this.userStore.userAuth.subscribe((res) => {
      this.userId = res.userId;
      this.navData[1].link = `./profile/${this.userId}`;

      if (this.checkIfAdmin()) this.navData[3].hidden = false;
    });

    this.notificationStore.notifications.subscribe((res) => {
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

  checkIfAdmin() {
    const token = this.cookieService.get('authorization');
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);
        return decodedToken.role === 'ADMIN';
      } catch (error) {
        console.log(error);
      }
    }
    return false;
  }
}
