import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { TweetModel } from '../../home/models/tweet.model';

@Injectable({
  providedIn: 'root',
})
export class TweetStore {
  private readonly tweetListSubject = new BehaviorSubject<TweetModel[]>([]);
  public readonly tweetList = this.tweetListSubject.asObservable();

  constructor() {}

  addTweetToTweetList(tweet: TweetModel) {
    if (tweet.isRetweet && !tweet.originalTweet) return;

    let tweets = this.tweetListSubject.getValue();
    tweets = [tweet, ...tweets];

    this.tweetListSubject.next(tweets);
  }

  removeTweetFromTweetList(tweetId: string) {
    let tweets = this.tweetListSubject.getValue();

    tweets = tweets
      .filter((t) => t.id !== tweetId)
      .map((t) => {
        if (t.originalTweetId === tweetId) {
          t.originalTweet = undefined;
        } else if (t.originalTweet?.originalTweetId === tweetId) {
          t.originalTweet.originalTweet = undefined;
        }
        return t;
      });

    this.addTweetsToTweetList(tweets, 1);
  }

  updateTweetInTweetList(tweet: TweetModel) {
    let tweets = this.tweetListSubject.getValue();

    tweets = tweets.map((t) => {
      if (t.id === tweet.id) {
        return tweet;
      } else if (t.originalTweetId === tweet.id) {
        return {
          ...t,
          originalTweet: tweet,
        };
      } else if (t.originalTweet?.originalTweetId === tweet.id) {
        return {
          ...t,
          originalTweet: {
            ...t.originalTweet,
            originalTweet: tweet,
          },
        };
      } else {
        return t;
      }
    });

    this.addTweetsToTweetList(tweets, 1);
  }

  addTweetsToTweetList(tweets: TweetModel[], page: number) {
    tweets = this.filterInvalidRetweets(tweets);
    const currentTweets = this.tweetListSubject.getValue();
    if (page === 1) {
      this.tweetListSubject.next(tweets);
    } else {
      this.tweetListSubject.next([...currentTweets, ...tweets]);
    }
  }

  filterInvalidRetweets(tweets: TweetModel[]) {
    return tweets.filter((x) => !(x.isRetweet && !x.originalTweet));
  }

  clearTweetList() {
    this.tweetListSubject.next([]);
  }
}
