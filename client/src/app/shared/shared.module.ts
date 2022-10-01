import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DateAgoPipe } from './pipes/date-ago.pipe';
import { GenerateImagePipe } from './pipes/generate-image.pipe';
import { AvaterComponent } from './ui/avater/avater.component';
import { HashtagInputComponent } from './ui/hashtag-input/hashtag-input.component';
import { TextInputHighlightModule } from 'angular-text-input-highlight';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    DateAgoPipe,
    GenerateImagePipe,
    AvaterComponent,
    HashtagInputComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TextInputHighlightModule,
  ],
  exports: [
    DateAgoPipe,
    GenerateImagePipe,
    AvaterComponent,
    HashtagInputComponent,
  ],
})
export class SharedModule {}
