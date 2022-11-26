import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { TimeAgoPipe } from 'src/app/shared/pipes/time-ago.pipe';
import { SharedModule } from 'src/app/shared/shared.module';
import { FeedComponent } from './pages/feed/feed.component';
import { TweetDetailsComponent } from './pages/tweet-details/tweet-details.component';
import { TweetRoutingModule } from './tweet-routing.module';
import { CommentItemComponent } from './ui/comment-item/comment-item.component';
import { LikersRetweetersDialogComponent } from './ui/likers-retweeters-dialog/likers-retweeters-dialog.component';
import { PostMakerDialogComponent } from './ui/post-maker-dialog/post-maker-dialog.component';

@NgModule({
  declarations: [
    FeedComponent,
    PostMakerDialogComponent,
    TweetDetailsComponent,
    CommentItemComponent,
    LikersRetweetersDialogComponent,
  ],
  imports: [
    TweetRoutingModule,
    CommonModule,
    SharedModule,
    InfiniteScrollModule,
    ReactiveFormsModule,
    FormsModule,
    MatDialogModule,
    MatIconModule,
    MatMenuModule,
    MatDividerModule,
    MatButtonModule,
  ],
  providers: [],
})
export class TweetModule {}
