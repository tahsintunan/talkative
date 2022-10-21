export interface UserModel {
  userId?: string;
  username?: string;
  email?: string;
  dateOfBirth?: Date;
  blocked?: string[];
  blockedBy?: string[];
}

export interface UserUpdateReqModel {
  username?: string;
  email?: string;
  dateOfBirth?: Date;
}
