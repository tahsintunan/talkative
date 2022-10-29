import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { TextInputHighlightModule } from 'angular-text-input-highlight';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { GenerateImagePipe } from './pipes/generate-image.pipe';
import { HashtagPipe } from './pipes/hashtag.pipe';
import { ShortNumberPipe } from './pipes/short-number.pipe';
import { TimeAgoPipe } from './pipes/time-ago.pipe';
import { AlertComponent } from './ui/alert/alert.component';
import { AvatarComponent } from './ui/avatar/avatar.component';
import { CommentInputComponent } from './ui/comment-input/comment-input.component';
import { EmptyResultOverlayComponent } from './ui/empty-result-overlay/empty-result-overlay.component';
import { HashtagInputComponent } from './ui/hashtag-input/hashtag-input.component';
import { HashtagRendererComponent } from './ui/hashtag-renderer/hashtag-renderer.component';
import { SearchInputComponent } from './ui/search-input/search-input.component';

@NgModule({
  declarations: [
    TimeAgoPipe,
    GenerateImagePipe,
    HashtagPipe,
    AvatarComponent,
    HashtagInputComponent,
    SearchInputComponent,
    CommentInputComponent,
    HashtagRendererComponent,
    AlertComponent,
    EmptyResultOverlayComponent,
    ShortNumberPipe,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TextInputHighlightModule,
    MatIconModule,
    InfiniteScrollModule,
  ],
  exports: [
    TimeAgoPipe,
    GenerateImagePipe,
    HashtagPipe,
    ShortNumberPipe,
    AvatarComponent,
    HashtagInputComponent,
    SearchInputComponent,
    HashtagRendererComponent,
    AlertComponent,
    EmptyResultOverlayComponent,
    CommentInputComponent,
  ],
})
export class SharedModule {}
