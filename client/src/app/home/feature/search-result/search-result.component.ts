import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PaginationModel } from '../../models/pagination.model';
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

  searchValue?: string;

  constructor(
    private activatedRoute: ActivatedRoute,
    private searchService: SearchService
  ) {}

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe((res) => {
      if (res['username']) {
        this.searchValue = res['username'];
        this.searchService
          .getUsersByUsername(res['username'], this.pagination)
          .subscribe((res) => {
            console.log(res);
          });
      }
      if (res['hashtag']) {
        this.searchValue = res['hashtag'];
        this.searchService
          .getTweetsByHashtag(res['hashtag'], this.pagination)
          .subscribe((res) => {
            console.log(res);
          });
      }
    });
  }
}
