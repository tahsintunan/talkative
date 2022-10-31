import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TweetStore } from 'src/app/shared/store/tweet.store';
import { PaginationModel } from '../../models/pagination.model';
import { TweetModel } from '../../models/tweet.model';
import { UserModel } from '../../models/user.model';
import { RetweetService } from '../../services/retweet.service';
import { SearchService } from '../../services/search.service';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css'],
})
export class SearchResultComponent implements OnInit {
  pagination: PaginationModel = {
    pageNumber: 1,
  };

  userList: UserModel[] = [];
  tweetList: TweetModel[] = [];

  searchType?: 'username' | 'hashtag' | 'quote';
  searchValue?: string;

  constructor(
    private activatedRoute: ActivatedRoute,
    private searchService: SearchService,
    private retweetService: RetweetService,
    private tweetStore: TweetStore
  ) {}

  ngOnInit(): void {
    this.tweetStore.tweetList.subscribe((res) => {
      this.tweetList = res;
    });

    this.activatedRoute.queryParams.subscribe((res) => {
      this.tweetList = [];
      this.userList = [];

      this.searchType = res['type'] ? res['type'] : undefined;
      this.searchValue = res['value'] ? res['value'] : undefined;

      this.getData();
    });
  }

  onScroll() {
    this.pagination.pageNumber++;
    this.getData;
  }

  getData() {
    if (this.searchType == 'username') {
      this.getUserList();
    } else if (this.searchType == 'hashtag') {
      this.getTweetList();
    } else if (this.searchType == 'quote') {
      this.getQuoteList();
    }
  }

  getUserList() {
    this.searchService
      .getUsersByUsername(this.searchValue!, this.pagination)
      .subscribe((res) => {
        if (this.pagination.pageNumber == 1) {
          this.userList = res;
        } else {
          this.userList = this.userList.concat(res);
        }
      });
  }

  getTweetList() {
    this.searchService
      .getTweetsByHashtag(this.searchValue!, this.pagination)
      .subscribe();
  }

  getQuoteList() {
    this.retweetService
      .getQuotes(this.searchValue!, this.pagination)
      .subscribe((res) => {
        console.log(res);
      });
  }
}
