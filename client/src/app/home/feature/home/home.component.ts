import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {
  SearchChangeModel,
  SearchSuggestionModel as SearchResultModel,
} from '../../models/search.model';
import { UserModel } from '../../models/user.model';
import { BlockService } from '../../services/block.service';
import { FollowService } from '../../services/follow.service';
import { NotificationService } from '../../services/notification.service';
import { SearchService } from '../../services/search.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  userAuth?: UserModel;
  searchResults: SearchResultModel[] = [];

  constructor(
    private userService: UserService,
    private followService: FollowService,
    private blockService: BlockService,
    private searchService: SearchService,
    private notificationService: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.preloadData();
  }

  preloadData(): void {
    this.userService.loadUserAuth();
    this.followService.loadUserFollowings();
    this.blockService.loadUserBlockList();
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
      this.router.navigate(['/home/profile', searchResult.key]);
    } else if (searchResult.type === 'hashtag') {
      this.onSearchSubmit(searchResult.value);
    }
  }

  onSearchSubmit(search: string): void {
    const type = search.startsWith('#') ? 'hashtag' : 'username';
    this.router.navigate(['/home/search'], {
      queryParams: { type, value: search },
    });
  }
}
