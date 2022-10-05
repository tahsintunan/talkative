export interface TweetModel {
  id?: string;
  text: string;
  hashtags: string[];
  userId?: string;
  username?: string;
  createdAt?: Date;
  isRetweet?: boolean;
  retweetId?: string;
  retweet?: TweetModel;
  likes?: string[];
}
