export interface CommentModel {
  id: string;
  text: string;
  userId: string;
  username: string;
  profilePicture?: string;
  tweetId: string;
  likes: string[];
  created: Date;
  lastModified?: Date;
}

export interface CommentCreateModel {
  tweetId: string;
  text: string;
}

export interface CommentUpdateModel {
  id: string;
  text: string;
}

export interface CommentLikeModel {
  id: string;
  isLiked: boolean;
}
