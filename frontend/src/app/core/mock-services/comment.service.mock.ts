import { of } from 'rxjs';
import { CommentUpdateModel, CommentLikeModel } from '../models/comment.model';
import { PaginationModel } from '../models/pagination.model';

export class CommentServiceMock {
  createComment(tweetId: string, text: string) {
    return of('comment added');
  }

  getTweetComments(tweetId: string, pagination: PaginationModel) {
    return of([
      {
        commentId: '1',
        text: 'test data',
        userId: 'test data',
        username: 'test data',
        profilePicture: 'test data',
        tweetId: tweetId,
        likes: ['1', '2'],
        created: new Date(),
      },
      {
        commentId: '2',
        text: 'test data',
        userId: 'test data',
        username: 'test data',
        profilePicture: 'test data',
        tweetId: tweetId,
        likes: ['1', '2'],
        created: new Date(),
      },
    ]);
  }

  getCommentById(commentId: string) {
    return of({
      commentId: '1',
      text: 'test data',
      userId: 'test data',
      username: 'test data',
      profilePicture: 'test data',
      tweetId: 'test data',
      likes: ['1', '2'],
      created: new Date(),
    });
  }

  updateComment(value: CommentUpdateModel) {
    return of('comment updated ' + value.id);
  }

  deleteComment(commentId: string) {
    return of('comment deleted ' + commentId);
  }

  likeComment(value: CommentLikeModel) {
    return of('comment liked');
  }
}
