import { UserModel } from './user.model';

export interface TweetModel {
  id: string;
  text: string;
  hashtags: string[];
  createdAt: Date;
  lastModified?: Date;
  isRetweet: boolean;
  isQuoteRetweet: boolean;
  user: UserModel;
  originalTweetId?: string;
  originalTweet?: TweetModel;
  retweetUsers: string[];
  quoteRetweets: string[];
  likes: string[];
  comments: string[];
}

export interface TweetWriteModel {
  id?: string;
  text?: string;
  hashtags?: string[];
  isQuoteRetweet?: boolean;
  originalTweetId?: string;
}

export interface TweetCreateReqModel {
  text: string;
  hashtags: string[];
}

export interface TweetUpdateReqModel {
  id: string;
  text?: string;
  hashtags?: string[];
}

export interface RetweetReqModel {
  text?: string;
  hashtags?: string[];
  isQuoteRetweet: boolean;
  originalTweetId: string;
}

export interface TrendingHashtagModel {
  hashtag: string;
  count: number;
}
