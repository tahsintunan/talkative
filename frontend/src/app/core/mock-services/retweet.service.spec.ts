import { of } from 'rxjs';
import { RetweetReqModel } from '../models/tweet.model';

export class RetweetServiceMock {
  createRetweet(retweet: RetweetReqModel) {
    return of('created retweet');
  }

  undoRetweet(originalTweetId: string) {
    return of('undo retweet ' + originalTweetId);
  }

  deleteQuoteRetweet(tweetId: string, originalTweetId: string) {
    return of('deleted quote retweet ') + tweetId;
  }
}
