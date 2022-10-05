import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewEncapsulation,
} from '@angular/core';
import { HighlightTag } from 'angular-text-input-highlight';

@Component({
  selector: 'app-hashtag-input',
  templateUrl: './hashtag-input.component.html',
  styleUrls: ['./hashtag-input.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class HashtagInputComponent implements OnInit {
  @Input() classExpression: string = 'w-full p-4 text-lg';
  @Input() rows: Number = 2;
  @Input() placeholder: string = 'Type here...';
  @Input() text: string = '';
  @Output() onChange = new EventEmitter();

  tags: HighlightTag[] = [];

  hashtags: string[] = [];

  constructor() {}

  ngOnInit(): void {
    this.addTags();
  }

  addTags() {
    this.tags = [];
    this.hashtags = [];
    const matchHashtag = /(#\w+) ?/g;
    let hashtag;

    while ((hashtag = matchHashtag.exec(this.text))) {
      this.tags.push({
        indices: {
          start: hashtag.index,
          end: hashtag.index + hashtag[1].length,
        },
        data: hashtag[1],
      });

      this.hashtags.push(hashtag[1]);
    }

    this.onTextChange();
  }

  onTextChange() {
    this.onChange.emit({ text: this.text, hashtags: this.hashtags });
  }
}
