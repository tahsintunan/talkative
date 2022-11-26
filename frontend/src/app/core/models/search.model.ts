import { PaginationModel } from './pagination.model';

export interface SearchSuggestionModel {
  type: string;
  key: string;
  value: string;
  image?: string;
}

export interface SearchChangeModel {
  value?: string;
  pagination: PaginationModel;
}
