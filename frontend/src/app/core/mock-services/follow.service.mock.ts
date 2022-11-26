import { HttpClient } from '@microsoft/signalr';
import { of } from 'rxjs';
import { EnvService } from 'src/app/shared/services/env.service';
import { PaginationModel } from '../models/pagination.model';
import { FollowStore } from '../store/follow.store';

export class FollowServiceMock {
  loadUserFollowings() {}

  follow(followingId: string) {
    return of('following ' + followingId);
  }

  unfollow(followingId: string) {
    return of('unfollow ' + followingId);
  }

  getUserFollowings() {
    return of([
      {
        username: 'siam',
        email: 'siam@gmail.com',
      },
      {
        username: 'tahsin',
        email: 'tahsin@gmail.com',
      },
    ]);
  }

  getFollowers(userId: string, pagination: PaginationModel) {
    return of([
      {
        username: 'siam',
        email: 'siam@gmail.com',
      },
      {
        username: 'tajbir',
        email: 'tajbir@gmail.com',
      },
    ]);
  }

  getFollowings(userId: string, pagination: PaginationModel) {
    return of([
      {
        username: 'tajbir',
        email: 'tajbir@gmail.com',
      },
      {
        username: 'tahsin',
        email: 'tahsin@gmail.com',
      },
    ]);
  }
}
