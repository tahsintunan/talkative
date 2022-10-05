import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewEncapsulation,
} from '@angular/core';
import { TweetModel } from '../../models/tweet.model';
import { UserModel } from '../../models/user.model';
import { TweetService } from '../../services/tweet.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-feed-item',
  templateUrl: './feed-item.component.html',
  styleUrls: ['./feed-item.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class FeedItemComponent implements OnInit {
  @Input() data?: TweetModel;
  @Output() onHashtagClick = new EventEmitter();
  @Output() onLikeClick = new EventEmitter();
  @Output() onCommentClick = new EventEmitter();
  @Output() onRetweetClick = new EventEmitter();
  @Output() onEditClick = new EventEmitter();

  userAuth?: UserModel;

  constructor(
    private userService: UserService,
    private tweetService: TweetService
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });
  }

  onTagClick(event: any) {
    if (event.target.classList.contains('hashtag')) {
      this.onHashtagClick.emit(event.target.innerText);
    }
  }

  onLike() {
    this.onLikeClick.emit(this.data?.id);
  }

  onComment() {
    this.onCommentClick.emit(this.data?.id);
  }

  onRetweet() {
    this.onRetweetClick.emit(
      this.data?.isRetweet ? this.data?.retweetId : this.data?.id
    );
  }

  onEdit() {
    this.onEditClick.emit(this.data?.id);
  }

  onDelete() {
    if (this.data?.id) {
      this.tweetService.deleteTweet(this.data?.id);
    }
  }
}
