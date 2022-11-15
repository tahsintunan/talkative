import { Observable, of } from 'rxjs';
import { PaginationModel } from '../models/pagination.model';
import {
  TweetCreateReqModel,
  TweetModel,
  TweetUpdateReqModel,
} from '../models/tweet.model';
import { UserModel } from '../models/user.model';

export class TweetServiceMock {
  createTweet(tweet: TweetCreateReqModel) {
    return of([
      {
        tweetId: '12',
      },
    ]);
  }

  updateTweet(tweet: TweetUpdateReqModel) {
    return of();
  }

  deleteTweet(tweetId: string) {
    return of();
  }

  getTweetById(id: string) {
    return of();
  }

  getTweets(pagination: PaginationModel) {
    return of();
  }

  getUserTweets(userId: string, pagination: PaginationModel) {
    return of();
  }

  likeTweet(tweetId: string, isLiked: boolean) {
    return of('liked tweet');
  }

  getRetweeters(
    tweetId: string,
    pagination: PaginationModel
  ): Observable<UserModel[]> {
    return of([
      {
        userId: '1',
        username: 'siam',
        email: 'siam@gmail.com',
      },
      {
        userId: '2',
        username: 'Tahsin',
        email: 'tahsin@gmail.com',
      },
      {
        userId: '3',
        username: 'tajbir',
        email: 'tajbir@gmail.com',
      },
    ]);
  }

  getLikedUsers(
    tweetId: string,
    pagination: PaginationModel
  ): Observable<UserModel[]> {
    return of([
      {
        userId: '1',
        username: 'siam',
        email: 'siam@gmail.com',
      },
      {
        userId: '2',
        username: 'Tahsin',
        email: 'tahsin@gmail.com',
      },
      {
        userId: '3',
        username: 'tajbir',
        email: 'tajbir@gmail.com',
      },
    ]);
  }

  getQuotes(tweetId: string, pagination: PaginationModel) {
    return of();
  }

  getTrendingHashtags() {
    return of();
  }

  addToTweets(tweet: TweetModel) {
    // const userAuth = this.userStore.userAuth.getValue();

    // const isFeed = this.router.url.endsWith('/tweet');
    // const isAuthUser =
    //   this.router.url.split('/profile/')[1] === userAuth.userId;

    // if (isFeed || (isAuthUser && tweet.user.userId === userAuth.userId))
    //   this.tweetStore.addTweetToTweetList(tweet);
    return of();
  }
}
