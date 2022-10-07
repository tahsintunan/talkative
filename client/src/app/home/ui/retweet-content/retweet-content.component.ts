import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TweetModel } from '../../models/tweet.model';

@Component({
  selector: 'app-retweet-content',
  templateUrl: './retweet-content.component.html',
  styleUrls: ['./retweet-content.component.css'],
})
export class RetweetContentComponent implements OnInit {
  @Input() data?: TweetModel;
  @Output() onHashtagClick = new EventEmitter();

  constructor() {}

  ngOnInit(): void {}

  onTagClick(event: any) {
    if (event.target.classList.contains('hashtag')) {
      this.onHashtagClick.emit(event.target.innerText);
    }
  }
}
