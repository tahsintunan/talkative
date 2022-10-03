import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-feed-item',
  templateUrl: './feed-item.component.html',
  styleUrls: ['./feed-item.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class FeedItemComponent implements OnInit {
  tweetMessage = `hi huys #my name is Tj`;

  constructor() {}

  ngOnInit(): void {}

  onHashTagClick(event: any) {
    if (event.target.classList.contains('hashtag')) {
      console.log(event.target.innerText);
    }
  }

  onLikeClick() {
    console.log('link clicked');
  }

  onCommentClick() {
    console.log('comment clicked');
  }

  onRetweetClick() {
    console.log('retweet clicked');
  }
}
