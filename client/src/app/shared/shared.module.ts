import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DateAgoPipe } from './pipes/date-ago.pipe';
import { GenerateImagePipe } from './pipes/generate-image.pipe';
import { AvaterComponent } from './ui/avater/avater.component';

@NgModule({
  declarations: [DateAgoPipe, GenerateImagePipe, AvaterComponent],
  imports: [CommonModule],
  exports: [DateAgoPipe, GenerateImagePipe, AvaterComponent],
})
export class SharedModule {}
