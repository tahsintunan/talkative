import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EnvService } from 'src/app/shared/services/env.service';
import {
  CommentLikeModel,
  CommentModel,
  CommentUpdateModel,
} from '../models/comment.model';
import { PaginationModel } from '../models/pagination.model';

@Injectable({
  providedIn: 'root',
})
export class CommentService {
  apiUrl = this.env.apiUrl + 'api/Comment';

  constructor(private http: HttpClient, private env: EnvService) {}

  createComment(tweetId: string, text: string) {
    return this.http.post<CommentModel>(this.apiUrl, { tweetId, text });
  }

  getTweetComments(tweetId: string, pagination: PaginationModel) {
    return this.http.get<CommentModel[]>(this.apiUrl + '/tweet/' + tweetId, {
      params: {
        ...pagination,
      },
    });
  }

  getCommentById(commentId: string) {
    return this.http.get<CommentModel>(this.apiUrl + '/' + commentId);
  }

  updateComment(value: CommentUpdateModel) {
    return this.http.patch<CommentModel>(this.apiUrl, value);
  }

  deleteComment(commentId: string) {
    return this.http.delete(this.apiUrl + '/' + commentId);
  }

  likeComment(value: CommentLikeModel) {
    return this.http.patch(this.apiUrl + '/like/' + value.id, value);
  }
}
