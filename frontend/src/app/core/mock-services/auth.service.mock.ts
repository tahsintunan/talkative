import { of } from 'rxjs';
import { SignInReqModel } from '../models/signin.model';
import { SignUpReqModel } from '../models/signup.model';

export class AuthServiceMock {
  signup(data: SignUpReqModel) {
    return of('signed up');
  }

  signin(data: SignInReqModel) {
    return of('signed in');
  }

  signout() {
    return of('signed out');
  }

  forgotPassword(email: string) {
    return of('forgot password');
  }
}
