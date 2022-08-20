import { Pipe, PipeTransform } from '@angular/core';
import { profileImageList } from '../data/profile-images.data';

@Pipe({
  name: 'generateImage',
})
export class GenerateImagePipe implements PipeTransform {
  transform(value: any, ...args: unknown[]): unknown {
    if (!value) {
      return profileImageList[0];
    }

    value = value.toString();

    let stringWeight = 0;

    for (let i = 0; i < value.length; i++) {
      stringWeight += value.charCodeAt(i);
    }
    return profileImageList[stringWeight % profileImageList.length];
  }
}
