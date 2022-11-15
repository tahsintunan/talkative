import { of } from 'rxjs';
import { UserAnalyticsModel, UserUpdateReqModel } from '../models/user.model';

export class UserServiceMock {
  loadUserAuth() {}

  getUser(userId: string) {
    return of({
      userId: '1',
      username: 'siam',
      email: 'siam@gmail.com',
      dateOfBirth: new Date(1998, 11, 8),
    });
  }

  updateProfile(user: UserUpdateReqModel) {
    return of('updated profile');
  }

  updatePassword(oldPassword: string, newPassword: string) {
    return of('updated password');
  }

  updateCoverImage(image: File) {
    return of('updated cover image');
  }

  updateProfileImage(image: File) {
    return of('updated profile image');
  }

  getUserAnalytics(userId: string) {
    return of({
      tweetCount: 3,
      followerCount: 3,
      followingCount: 3,
    });
  }

  getTopUsers() {
    return of([
      {
        userId: '1',
        username: 'siam',
        email: 'siam@gmail.com',
        dateOfBirth: new Date(1998, 11, 8),
      },
      {
        userId: '2',
        username: 'tahsin',
        email: 'tahsin@gmail.com',
        dateOfBirth: new Date(1998, 10, 8),
      },
      {
        userId: '3',
        username: 'tajbir',
        email: 'tajbir@gmail.com',
        dateOfBirth: new Date(1998, 9, 8),
      },
    ]);
  }
}
