import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DateAgoPipe } from './pipes/date-ago.pipe';
import { GenerateImagePipe } from './pipes/generate-image.pipe';
import { AvaterComponent } from './ui/avater/avater.component';
import { HashtagInputComponent } from './ui/hashtag-input/hashtag-input.component';
import { TextInputHighlightModule } from 'angular-text-input-highlight';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HashtagPipe } from './pipes/hashtag.pipe';
import { SearchInputComponent } from './ui/search-input/search-input.component';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
  declarations: [
    DateAgoPipe,
    GenerateImagePipe,
    HashtagPipe,
    AvaterComponent,
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
    AvaterComponent,
    HashtagInputComponent,
    SearchInputComponent,
  ],
})
export class SharedModule {}
