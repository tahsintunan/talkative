import { PipeTransform, Pipe } from '@angular/core';

@Pipe({ name: 'hashtag' })
export class HashtagPipe implements PipeTransform {
  constructor() {}

  transform(text: string) {
    let transformedText;

    if (text.indexOf('#') !== -1) {
      transformedText = text + ' ';
      const matches = transformedText.match(/#(.*?) /g) || [];
      for (let i = 0; i < matches.length; i++) {
        transformedText = transformedText.replace(
          matches[i],
          '<span class="hashtag">' + matches[i] + '</span>'
        );
      }
    } else {
      transformedText = text;
    }
    return transformedText;
  }
}
