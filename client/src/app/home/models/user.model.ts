export interface UserModel {
  userId?: string;
  username?: string;
  email?: string;
  dateOfBirth?: Date;
}

export interface UserUpdateReqModel {
  username?: string;
  email?: string;
  dateOfBirth?: Date;
}
