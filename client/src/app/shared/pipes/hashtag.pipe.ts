import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'hashtag' })
export class HashtagPipe implements PipeTransform {
  matchHashtag = /(#\w+) ?/gm;

  constructor() {}

  transform(text: string) {
    const transformedText = text.replace(
      this.matchHashtag,
      (value) => `<span class="hashtag">${value}</span>`
    );

    return transformedText;
  }
}
