export interface NotificationModel {
  notificationId: string;
  notificationReceiverId: string;
  eventType:
    | 'follow'
    | 'likeTweet'
    | 'likeComment'
    | 'comment'
    | 'retweet'
    | 'quoteRetweet';
  eventTriggererId: string;
  eventTriggererUsername: string;
  eventTriggererProfilePicture?: string;
  tweetId?: string;
  commentId?: string;
  dateTime: Date;
  isRead: boolean;
}
