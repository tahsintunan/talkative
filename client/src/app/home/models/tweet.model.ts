import { UserModel } from './user.model';

export interface TweetModel {
  id: string;
  text: string;
  hashtags: string[];
  createdAt: Date;
  isRetweet: boolean;
  user: UserModel;
  retweetId?: string;
  retweet?: TweetModel;
  retweetUsers: string[];
  retweetPosts: string[];
  likes: string[];
  comments: string[];
}

export interface TweetWriteModel {
  id?: string;
  text?: string;
  hashtags?: string[];
  isRetweet?: boolean;
  retweetId?: string;
}

export interface TweetCreateReqModel {
  text: string;
  hashtags: string[];
}

export interface TweetUpdateReqModel {
  id: string;
  text?: string;
  hashtags?: string[];
  isRetweet?: boolean;
  retweetId?: string;
}

export interface TweetRetweetReqModel {
  text?: string;
  hashtags: string[];
  isRetweet: boolean;
  retweetId: string;
}
