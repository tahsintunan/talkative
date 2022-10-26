import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { TextInputHighlightModule } from 'angular-text-input-highlight';
import { DateAgoPipe } from './pipes/date-ago.pipe';
import { GenerateImagePipe } from './pipes/generate-image.pipe';
import { HashtagPipe } from './pipes/hashtag.pipe';
import { AvatarComponent } from './ui/avatar/avatar.component';
import { HashtagInputComponent } from './ui/hashtag-input/hashtag-input.component';
import { SearchInputComponent } from './ui/search-input/search-input.component';
import { HashtagRendererComponent } from './ui/hashtag-renderer/hashtag-renderer.component';
import { AlertComponent } from './ui/alert/alert.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

@NgModule({
  declarations: [
    DateAgoPipe,
    GenerateImagePipe,
    HashtagPipe,
    AvatarComponent,
    HashtagInputComponent,
    SearchInputComponent,
    HashtagRendererComponent,
    AlertComponent,
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
    DateAgoPipe,
    GenerateImagePipe,
    HashtagPipe,
    AvatarComponent,
    HashtagInputComponent,
    SearchInputComponent,
    HashtagRendererComponent,
    AlertComponent,
  ],
})
export class SharedModule {}
