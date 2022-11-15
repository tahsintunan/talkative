import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { SearchServiceMock } from 'src/app/core/mock-services/search.service.mock';
import { SearchService } from 'src/app/core/services/search.service';
import { SearchResultComponent } from './search-result.component';

describe('SearchResultComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientModule],
      declarations: [SearchResultComponent],
      providers: [{ provide: SearchService, useClass: SearchServiceMock }],
    }).compileComponents();
  });

  it('should create the search result component', () => {
    const fixture = TestBed.createComponent(SearchResultComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
