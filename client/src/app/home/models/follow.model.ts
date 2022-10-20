export interface FollowModel {
  userId: string;
  username: string;
  email: string;
  dateOfBirth: Date;
  blockedBy: string[];
  blocked: string[];
}
