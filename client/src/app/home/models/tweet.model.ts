import { UserModel } from './user.model';

export interface TweetModel {
  id?: string;
  text: string;
  hashtags?: string[];
  createdAt?: Date;
  isRetweet?: boolean;
  user: UserModel;
  retweetId?: string;
  retweet?: TweetModel;
  likes?: string[];
  comments?: string[];
}
