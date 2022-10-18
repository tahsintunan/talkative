export interface CommentModel {
  id: string;
  text: string;
  userId: string;
  username: string;
  tweetId: string;
  likes: string[];
  created: Date;
}

export interface CommentCreateModel {
  tweetId: string;
  text: string;
}

export interface CommentUpdateModel {
  commentId: string;
  text: string;
}

export interface CommentLikeModel {
  commentId: string;
  isLiked: boolean;
}
