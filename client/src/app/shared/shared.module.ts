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

@NgModule({
  declarations: [
    DateAgoPipe,
    GenerateImagePipe,
    HashtagPipe,
    AvatarComponent,
    HashtagInputComponent,
    SearchInputComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TextInputHighlightModule,
    MatIconModule,
  ],
  exports: [
    DateAgoPipe,
    GenerateImagePipe,
    HashtagPipe,
    AvatarComponent,
    HashtagInputComponent,
    SearchInputComponent,
  ],
})
export class SharedModule {}
