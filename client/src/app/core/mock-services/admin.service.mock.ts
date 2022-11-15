import { of } from 'rxjs';
import { PaginationModel } from '../models/pagination.model';
import { UserUpdateReqModel } from '../models/user.model';

export class AdminServiceMock {
  getUserList(pagination: PaginationModel) {
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

  banUser(userId: string) {
    return of('banned user ' + userId);
  }

  unbanUser(userId: string) {
    return of('unban user ' + userId);
  }

  searchUser(search: string, pagination: PaginationModel) {
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

  updateUser(user: UserUpdateReqModel) {
    return of('updated user ' + user.userId);
  }
}
