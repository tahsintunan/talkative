export interface UserModel {
  userId?: string;
  username?: string;
  email?: string;
  dateOfBirth?: Date;
  isBanned?: boolean;
}

export interface UserAnalyticsModel {
  tweetCount: number;
  followerCount: number;
  followingCount: number;
}

export interface UserUpdateReqModel {
  userId: string;
  username: string;
  email: string;
  dateOfBirth: Date;
}
