import { of } from 'rxjs';
import { PaginationModel } from '../models/pagination.model';

export class BlockServiceMock {
  loadUserBlockList() {}

  blockUser(userId: string) {
    return of('blocked ' + userId);
  }

  unblockUser(userId: string) {
    return of('unblocked ' + userId);
  }

  getBlockList(userId: string, pagination: PaginationModel) {
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

  getUserBlockList() {
    return of([
      {
        username: 'tajbir',
        email: 'tajbir@gmail.com',
      },
      {
        username: 'siam',
        email: 'siam@gmail.com',
      },
    ]);
  }
}
