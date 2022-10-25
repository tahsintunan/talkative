export interface UserModel {
  userId?: string;
  username?: string;
  email?: string;
  dateOfBirth?: Date;
  blocked?: string[];
  blockedBy?: string[];
  isBannded?: boolean;
}

export interface UserUpdateReqModel {
  username?: string;
  email?: string;
  dateOfBirth?: Date;
}
