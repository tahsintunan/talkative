import { of } from 'rxjs';
import { PaginationModel } from '../models/pagination.model';

export class SearchServiceMock {
  getSearchSuggestions(search: string, pagination: PaginationModel) {
    if (search.startsWith('#')) {
      return of([
        {
          tweetId: '1',
          tweet: '#random random',
        },
        {
          tweetId: '2',
          tweet: '#random random',
        },
        {
          tweetId: '3',
          tweet: '#random random',
        },
      ]);
    } else {
      return of([
        {
          userId: '1',
          username: 'siam',
          email: 'random@gmail.com',
        },
        {
          userId: '2',
          username: 'siam1',
          email: 'random@gmail.com',
        },
      ]);
    }
  }
}
