import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UserStore } from 'src/app/core/store/user.store';
import {
  CommentLikeModel,
  CommentModel,
  CommentUpdateModel,
} from '../../../../../core/models/comment.model';
import { UserModel } from '../../../../../core/models/user.model';

@Component({
  selector: 'app-comment-item',
  templateUrl: './comment-item.component.html',
  styleUrls: ['./comment-item.component.css'],
})
export class CommentItemComponent implements OnInit {
  @Input() data?: CommentModel;
  @Input() commentToHightlight?: string;
  @Output() onDeleteClick = new EventEmitter();
  @Output() onEditSubmit = new EventEmitter<CommentUpdateModel>();
  @Output() onLikeClick = new EventEmitter<CommentLikeModel>();

  isEditMode = false;

  userAuth?: UserModel;

  alreadyLiked: boolean = false;

  constructor(private userStore: UserStore) {}

  ngOnInit(): void {
    this.userStore.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.alreadyLiked = !!this.data?.likes?.some(
      (likedBy) => likedBy === this.userAuth?.userId
    );
  }

  onLike() {
    this.onLikeClick.emit({
      id: this.data?.id!,
      isLiked: !this.alreadyLiked,
    });
  }

  onEdit(text: string) {
    this.onEditSubmit.emit({
      id: this.data?.id!,
      text,
    });
    this.isEditMode = false;
  }

  onDelete() {
    this.onDeleteClick.emit(this.data?.id);
  }
}
