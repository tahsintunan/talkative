import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { NavigationEnd, Router } from '@angular/router';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { PaginationModel } from 'src/app/core/models/pagination.model';
import {
  SearchChangeModel,
  SearchSuggestionModel,
} from 'src/app/core/models/search.model';

@Component({
  selector: 'app-search-input',
  templateUrl: './search-input.component.html',
  styleUrls: ['./search-input.component.css'],
})
export class SearchInputComponent implements OnInit {
  @Input() searchResults: SearchSuggestionModel[] = [];
  @Output() onSearchChange = new EventEmitter<SearchChangeModel>();
  @Output() onSearchSubmit = new EventEmitter<string>();
  @Output() onSearchResultSelect = new EventEmitter<SearchSuggestionModel>();

  pagination: PaginationModel = {
    pageNumber: 1,
  };

  formControl = new FormControl<string>('');

  focused: boolean = false;

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.router.events.subscribe((res) => {
      if (
        res instanceof NavigationEnd &&
        !res.urlAfterRedirects.includes('/search')
      ) {
        this.formControl.setValue('');
      }
    });

    this.formControl.valueChanges
      .pipe(debounceTime(1000), distinctUntilChanged())
      .subscribe((value) => {
        this.pagination.pageNumber = 1;
        this.onSearchChange.emit({
          value: value?.trim(),
          pagination: this.pagination,
        });
      });
  }

  onSearchResultClick(result: SearchSuggestionModel): void {
    this.formControl.setValue(result.value);
    this.onSearchResultSelect.emit(result);
  }

  onScroll() {
    this.pagination.pageNumber++;
    this.onSearchChange.emit({
      value: this.formControl.value?.trim(),
      pagination: this.pagination,
    });
  }
}
