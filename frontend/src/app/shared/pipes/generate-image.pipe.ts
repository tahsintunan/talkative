import { Pipe, PipeTransform } from '@angular/core';
import { backgroundImageList, profileImageList } from '../data/images.data';

@Pipe({
  name: 'generateImage',
})
export class GenerateImagePipe implements PipeTransform {
  private images: string[] = [];

  transform(value: any, imageType: string = 'profile') {
    this.updateImages(imageType);

    if (!value) {
      return this.images[0];
    }

    value = value.toString();

    let stringWeight = 0;

    for (let i = 0; i < value.length; i++) {
      stringWeight += value.charCodeAt(i);
    }
    return this.images[stringWeight % this.images.length];
  }

  updateImages(imageType: string) {
    if (imageType === 'profile') {
      this.images = profileImageList;
    } else {
      this.images = backgroundImageList;
    }
  }
}
