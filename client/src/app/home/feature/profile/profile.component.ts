import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TweetModel } from '../../models/tweet.model';
import { UserModel } from '../../models/user.model';
import { TweetService } from '../../services/tweet.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  profileDetails?: UserModel;
  tweets: TweetModel[] = [];

  constructor(
    private userService: UserService,
    private tweetService: TweetService,
    private activeRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.userService.userProfile.subscribe((res) => {
      this.profileDetails = res;
    });

    this.tweetService.userTweets.subscribe((res) => {
      this.tweets = res;
    });

    this.activeRoute.params.subscribe((params) => {
      this.getProfile(params['userId']);
      this.getUserTweets(params['userId']);
    });
  }

  getProfile(userId: string) {
    this.userService.getUser(userId).subscribe();
  }

  getUserTweets(userId: string) {
    this.tweetService.getUserTweets(userId).subscribe();
  }
}
