import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { TweetModel } from '../../models/tweet.model';

@Component({
  selector: 'app-retweet-content',
  templateUrl: './retweet-content.component.html',
  styleUrls: ['./retweet-content.component.css'],
})
export class RetweetContentComponent implements OnInit {
  @Input() data?: TweetModel;
  @Output() onHashtagClick = new EventEmitter();

  constructor(private router: Router) {}

  ngOnInit(): void {}

  onTweetClick() {
    this.router.navigate([`/home/tweet/${this.data?.id}`]);
  }

  onTagClick(event: any) {
    if (event.target.classList.contains('hashtag')) {
      this.onHashtagClick.emit(event.target.innerText);
    }
  }
}
