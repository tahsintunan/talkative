import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SearchService } from 'src/app/core/services/search.service';
import { TweetService } from 'src/app/core/services/tweet.service';
import { TweetStore } from 'src/app/core/store/tweet.store';
import { PaginationModel } from '../../../core/models/pagination.model';
import { TweetModel } from '../../../core/models/tweet.model';
import { UserModel } from '../../../core/models/user.model';

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
    private tweetService: TweetService,
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
    this.tweetService
      .getQuotes(this.searchValue!, this.pagination)
      .subscribe((res) => {
        console.log(res);
      });
  }
}
