import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { SearchInputComponent } from './search-input.component';

describe('Search input Component', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [SearchInputComponent],
    }).compileComponents();
  });

  it('should create the search input component', () => {
    const fixture = TestBed.createComponent(SearchInputComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
