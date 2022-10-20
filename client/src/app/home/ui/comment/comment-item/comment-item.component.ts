import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  CommentLikeModel,
  CommentModel,
  CommentUpdateModel,
} from '../../../models/comment.model';
import { UserModel } from '../../../models/user.model';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-comment-item',
  templateUrl: './comment-item.component.html',
  styleUrls: ['./comment-item.component.css'],
})
export class CommentItemComponent implements OnInit {
  @Input() data?: CommentModel;
  @Output() onDeleteClick = new EventEmitter();
  @Output() onEditSubmit = new EventEmitter<CommentUpdateModel>();
  @Output() onLikeClick = new EventEmitter<CommentLikeModel>();

  isEditMode = false;

  userAuth?: UserModel;

  alreadyLiked: boolean = false;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.alreadyLiked = !!this.data?.likes?.some(
      (likedBy) => likedBy === this.userAuth?.userId
    );
  }

  onLike() {
    this.onLikeClick.emit({
      commentId: this.data?.id!,
      isLiked: !this.alreadyLiked,
    });
  }

  onEdit(text: string) {
    this.onEditSubmit.emit({
      commentId: this.data?.id!,
      text,
    });
    this.isEditMode = false;
  }

  onDelete() {
    this.onDeleteClick.emit(this.data?.id);
  }
}
