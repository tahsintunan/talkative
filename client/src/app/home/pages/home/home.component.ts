import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BlockService } from 'src/app/core/services/block.service';
import { FollowService } from 'src/app/core/services/follow.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { SearchService } from 'src/app/core/services/search.service';
import { UserService } from 'src/app/core/services/user.service';
import { UserStore } from 'src/app/core/store/user.store';
import {
  SearchChangeModel,
  SearchSuggestionModel as SearchResultModel,
} from '../../../core/models/search.model';
import { UserModel } from '../../../core/models/user.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  userAuth?: UserModel;
  searchResults: SearchResultModel[] = [];

  constructor(
    private userStore: UserStore,
    private userService: UserService,
    private followService: FollowService,
    private blockService: BlockService,
    private searchService: SearchService,
    private notificationService: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userStore.userAuth.subscribe((res) => {
      this.userAuth = res;
      if (this.userAuth?.userId) this.notificationService.loadNotifications();
    });

    this.preloadData();
  }

  preloadData(): void {
    this.userService.loadUserAuth();
    this.followService.loadUserFollowings();
    this.blockService.loadUserBlockList();
    this.notificationService.initConnection();
  }

  onSearchChange(search: SearchChangeModel): void {
    if (search.value?.trim()) {
      this.searchService
        .getSearchSuggestions(search.value.trim(), search.pagination)
        .subscribe((res) => {
          if (search.pagination.pageNumber === 1) {
            this.searchResults = res;
          } else {
            this.searchResults = this.searchResults.concat(res);
          }
        });
    } else {
      this.searchResults = [];
    }
  }

  onSearchResultSelect(searchResult: SearchResultModel): void {
    if (searchResult.type === 'user') {
      this.router.navigate(['/profile', searchResult.key]);
    } else if (searchResult.type === 'hashtag') {
      this.onSearchSubmit(searchResult.value);
    }
  }

  onSearchSubmit(search: string): void {
    const type = search.startsWith('#') ? 'hashtag' : 'username';
    this.router.navigate(['/search'], {
      queryParams: { type, value: search },
    });
  }
}
