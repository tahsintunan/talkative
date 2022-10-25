import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {
  SearchChangeModel,
  SearchSuggestionModel as SearchResultModel,
} from '../../models/search.model';
import { UserModel } from '../../models/user.model';
import { FollowService } from '../../services/follow.service';
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
    private searchService: SearchService,
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
    this.followService.loadUserFollow();
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
    if (search.startsWith('#')) {
      this.router.navigate(['/home/search'], {
        queryParams: { hashtag: search },
      });
    } else {
      this.router.navigate(['/home/search'], {
        queryParams: { username: search },
      });
    }
  }
}
