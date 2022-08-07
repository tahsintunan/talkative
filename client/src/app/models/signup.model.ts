export interface SignUpReqModel {
  username: string;
  email: string;
  dateOfBirth: string;
  password: string;
  confirmPassword: string;
}

export interface SignUpResModel {
  accessToken: string;
}
