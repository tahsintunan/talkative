import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TweetModel } from '../../../../../core/models/tweet.model';

@Component({
  selector: 'app-retweet-content',
  templateUrl: './retweet-content.component.html',
  styleUrls: ['./retweet-content.component.css'],
})
export class RetweetContentComponent implements OnInit {
  @Input() data?: TweetModel;

  constructor(private router: Router) {}

  ngOnInit(): void {}

  onTweetClick() {
    this.router.navigate([`/tweet/${this.data?.id}`]);
  }

  onTagClick(hashtag: string) {
    this.router.navigate(['/search'], {
      queryParams: { type: 'hashtag', value: hashtag },
    });
  }
}
